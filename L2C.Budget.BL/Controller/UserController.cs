using L2C.Budget.BL.Model;
using System;
using System.IO;
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
        public User User { get; }

        /// <summary>
        /// Создание нового контроллера пользователя.
        /// </summary>
        /// <param name="user"></param>
        public UserController(string userName, string genderName, DateTime birthDay, string budgetname)
        {
            //TODO: сделать проверку входящих значений

            var gender = new Gender(genderName);
            var budget = new Budget.BL.Model.Budget(budgetname);
            User = new User(userName, gender, birthDay, budget);
            
        }

        /// <summary>
        /// Сохранить данные пользователя в файл.
        /// </summary>
        public void Save()
        {
            var formatter = new BinaryFormatter();
            using(var fs = new FileStream("users.bin", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, User);
            }
        }

        /// <summary>
        /// Загрузить пользователя из файла.
        /// </summary>
        /// <returns>Пользователь приложения.</returns>
        public UserController()
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream("users.bin", FileMode.OpenOrCreate))
            {
                if(formatter.Deserialize(fs) is User user)
                {
                    User = user;
                }
            }
        }

    }
}
