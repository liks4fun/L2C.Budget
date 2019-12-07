using L2C.Budget.BL.Model;
using L2C.Budget.BL.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace L2C.Budget.BL.Controller
{
    /// <summary>
    /// Контроллер пользователя.
    /// </summary>
    public class UserController
    {
        public delegate void BalanceHandler(string mes);
        /// <summary>
        /// Событие для уведомления об изменении баланса.
        /// </summary>
        public event BalanceHandler Notify;

        private IRepository repository;

        /// <summary>
        /// Текущий пользователь.
        /// </summary>
        private User CurrentUser { get; set; }

        /// <summary>
        /// Аутентифицирован ли пользователь.
        /// </summary>
        public bool IsUserAuthen => CurrentUser is User ? true : false;

        /// <summary>
        /// Список всех пользователей.
        /// </summary>
        private List<User> Users { get; set; }

        /// <summary>
        /// Создание нового контроллера пользователя.
        /// </summary>
        /// <param name="user"></param>
        public UserController(IRepository repo)
        {
            repository = repo;
            Users = repository.GetUsers();
        }

        /// <summary>
        /// Создание нового пользователя.
        /// </summary>
        /// <param name="userName">Имя нового пользователя.</param>
        /// <param name="genderName">Пол.</param>
        /// <param name="birthday">Дата рождения.</param>
        /// <param name="budgetName">Имя бюджета.</param>
        public void CreateNewUser(string userName, string genderName, DateTime birthday, string budgetName)
        {
            if(string.IsNullOrWhiteSpace(genderName))
                throw new ArgumentNullException(nameof(genderName));
            if (string.IsNullOrWhiteSpace(budgetName))
                throw new ArgumentNullException(nameof(budgetName));
            if (birthday.Year < 1900 || birthday > DateTime.Now)
                throw new ArgumentException(nameof(birthday));
            if (Users.Contains(Users.SingleOrDefault(u => u.Name == userName)))
                throw new ArgumentException(nameof(userName));
            Gender gender = new Gender(genderName);
            L2C.Budget.BL.Model.Budget budget = new L2C.Budget.BL.Model.Budget(budgetName);
            var user = new User(userName, gender, birthday, budget);
            Users.Add(user);
            repository.SaveUsers(Users);
        }

        /// <summary>
        /// Аутентифицирует текущего пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        public void AuthenUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));
            var user = repository.GetUsers().SingleOrDefault(u => u.Name == userName);
            if (user == null)
                throw new NewUserException(nameof(userName));
            else
                CurrentUser = user;
        }

        /// <summary>
        /// Добавляет дене в бюджет пользователя.
        /// </summary>
        /// <param name="amount">Кол-во денег.</param>
        public void AddMoney(float amount)
        {
            if (amount <= 0)
                throw new ArgumentException(nameof(amount));
            if (CurrentUser == null || CurrentUser.Budget == null)
                throw new ArgumentNullException(nameof(CurrentUser.Name));
            CurrentUser.Budget.Balance += amount;
            repository.SaveUsers(Users);
            Notify?.Invoke($"{CurrentUser.Name} {CurrentUser.Budget.Name} {amount:c}. {CurrentUser.Budget.Balance:c}");
        }

        /// <summary>
        /// Снимает деньги с бюджета пользователя.
        /// </summary>
        /// <param name="amount">Кол-во денег.</param>
        public void RemoveMoney(float amount)
        {
            if (amount <= 0)
                throw new ArgumentException(nameof(amount));
            if (CurrentUser == null || CurrentUser.Budget == null)
                throw new ArgumentNullException(nameof(CurrentUser));
            CurrentUser.Budget.Balance -= amount;
            repository.SaveUsers(Users);
            Notify?.Invoke($"{CurrentUser.Name} {CurrentUser.Budget.Name} {amount:c}. {CurrentUser.Budget.Balance:c}");
        }

        /// <summary>
        /// Получить состояние баланса пользователя.
        /// </summary>
        /// <returns>Кортеж (имя пользователя, баланс пользователя)</returns>
        public (string userName, string budgetName, float balance) GetUserBalance()
        {
            if (CurrentUser == null)
                throw new ArgumentNullException();
            return (userName: CurrentUser.Name, budgetName: CurrentUser.Budget.Name, balance: CurrentUser.Budget.Balance);
        }

    }
}
