using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace L2C.Budget.BL.Model
{
    /// <summary>
    /// Репозиторий пользователей сохраненных в файле.
    /// </summary>
    public class FileRepository : IRepository
    {
        /// <summary>
        /// Список всех пользователей.
        /// </summary>
        private List<User> Users { get; set; }
        /// <summary>
        /// Список всех полов.
        /// </summary>
        private List<Gender> Genders { get; set; }

        /// <summary>
        /// Список всех бюджетов.
        /// </summary>
        private List<UserBudget> Budgets { get; set; }

        /// <summary>
        /// Сохранить данные пользователей в файл.
        /// </summary>
        public void SaveUsers()
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream("users.bin", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Users);
            }
        }

        /// <summary>
        /// Сохранить данные полов в файл.
        /// </summary>
        public void SaveGenders()
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream("genders.bin", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Genders);
            }
        }

        /// <summary>
        /// Сохранить данные бюджетов в файл.
        /// </summary>
        public void SaveBudgets()
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream("budgets.bin", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Budgets);
            }
        }


        /// <summary>
        /// Загрузить пользователя из файла.
        /// </summary>
        /// <returns>Пользователь приложения.</returns>
        public User GetUser(string userName)
        {
            return Users.SingleOrDefault(u => u.Name == userName);
        }

        /// <summary>
        /// Загрузить пол пользователя из файла.
        /// </summary>
        /// <returns>Пол пользователя приложения.</returns>
        public Gender GetGender(int id)
        {
            return Genders.SingleOrDefault(g => g.Id == id);
        }

        /// <summary>
        /// Загрузить бюджет пользователя из файла.
        /// </summary>
        /// <returns>бюджет пользователя приложения.</returns>
        public UserBudget GetBudget(int id)
        {
            return Budgets.SingleOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Сохранить нового пользователя.
        /// </summary>
        public void CreateUser(string userName, string genderName, DateTime birthday, string budgetName)
        {
            if (string.IsNullOrWhiteSpace(genderName))
                throw new ArgumentNullException(nameof(genderName));
            if (string.IsNullOrWhiteSpace(budgetName))
                throw new ArgumentNullException(nameof(budgetName));
            if (birthday.Year < 1900 || birthday > DateTime.Now)
                throw new ArgumentException(nameof(birthday));
            if (Users.Contains(Users.SingleOrDefault(u => u.Name == userName)))
                throw new ArgumentException(nameof(userName));
            UserBudget budget = Budgets.SingleOrDefault(b => b.Name == budgetName);
            if (budget == null)
            {
                budget = new UserBudget(budgetName);
                Budgets.Add(budget);
                SaveBudgets();
            }
            Gender gender = Genders.SingleOrDefault(g => g.Name == genderName);
            if (gender == null)
            {
                gender = new Gender(genderName);
                Genders.Add(gender);
                SaveGenders();
            }
            
            var user = new User(userName, gender, birthday, budget.Id);
            Users.Add(user);
            SaveUsers();
        }

        /// <summary>
        /// Конструктор инициализирующий коллекции.
        /// </summary>
        public FileRepository()
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream("users.bin", FileMode.OpenOrCreate))
            {
                if (fs.Length > 0 && formatter.Deserialize(fs) is List<User> users)
                    Users = users;
                else
                    Users = new List<User>();
            }
            using (var fs = new FileStream("genders.bin", FileMode.OpenOrCreate))
            {
                if (fs.Length > 0 && formatter.Deserialize(fs) is List<Gender> genders)
                    Genders = genders;
                else
                    Genders = new List<Gender>();
            }
            using (var fs = new FileStream("budgets.bin", FileMode.OpenOrCreate))
            {
                if (fs.Length > 0 && formatter.Deserialize(fs) is List<UserBudget> budgets)
                    Budgets = budgets;
                else
                    Budgets = new List<UserBudget>();
            }
        }
    }
}
