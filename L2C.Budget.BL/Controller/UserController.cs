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
        /// <summary>
        /// Пользователь приложения.
        /// </summary>
        public List<User> Users { get; }
        public User CurrentUser { get; }

        /// <summary>
        /// Создание нового контроллера пользователя.
        /// </summary>
        /// <param name="user"></param>
        public UserController(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName), "Имя пользователя не может быть пустым");
            Users = GetUsersFromFile();
            CurrentUser = Users.SingleOrDefault(u => u.Name == userName);
            if (CurrentUser == null)
                throw new NewUserException($"Пользователя с именем {userName} нет в базе данных");
        }

        /// <summary>
        /// Инициализация контроллера с регистрацией нового пользователя.
        /// </summary>
        /// <param name="userName">Имя нового пользователя.</param>
        /// <param name="genderName">Пол.</param>
        /// <param name="birthday">Дата рождения.</param>
        /// <param name="budgetName">Имя бюджета.</param>
        public UserController(string userName, string genderName, DateTime birthday, string budgetName)
        {
            if(string.IsNullOrWhiteSpace(genderName))
                throw new ArgumentNullException(nameof(genderName), "Пол пользователя не может быть пустым.");
            if (string.IsNullOrWhiteSpace(budgetName))
                throw new ArgumentNullException(nameof(budgetName), "Имя бюджета не может быть пустым.");
            if (birthday.Year < 1900 || birthday > DateTime.Now)
                throw new ArgumentException(nameof(birthday), "Не верная дата рождения.");
            Gender gender = new Gender(genderName);
            L2C.Budget.BL.Model.Budget budget = new L2C.Budget.BL.Model.Budget(budgetName);
            CurrentUser = new User(userName, gender, birthday, budget);
            Users = GetUsersFromFile();
            Users.Add(CurrentUser);
            Save();

        }

        /// <summary>
        /// Сохранить данные пользователя в файл.
        /// </summary>
        public void Save()
        {
            var formatter = new BinaryFormatter();
            using(var fs = new FileStream("users.bin", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Users);
            }
        }

        /// <summary>
        /// Загрузить пользователей из файла.
        /// </summary>
        /// <returns>Пользователь приложения.</returns>
        public List<User> GetUsersFromFile()
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream("users.bin", FileMode.OpenOrCreate))
            {
                if (fs.Length > 0 && formatter.Deserialize(fs) is List<User> users)
                    return users;
                else
                    return new List<User>();
            }
        }

    }
}
