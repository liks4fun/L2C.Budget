using System;

namespace L2C.Budget.BL.Model
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Пол пользователя.
        /// </summary>
        public Gender Gender { get; }

        /// <summary>
        /// Дата рождения пользователя.
        /// </summary>
        public DateTime BirthDate { get; }

        /// <summary>
        /// Бюджет пользователя.
        /// </summary>
        public Budget Budget { get; set; }

        /// <summary>
        /// Создать нового пользователя.
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="gender">Пол</param>
        /// <param name="birthDate">Дата рождения</param>
        /// <param name="budget">Бюджет</param>
        public User(string name,
                    Gender gender,
                    DateTime birthDate,
                    Budget budget)
        {
            #region Проверка входящих параметров.
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Имя не может быть пустым или null.");
            }

            if (gender == null)
            {
                throw new ArgumentNullException("Пол пользователя обязателен.");
            }

            if(birthDate == null)
            {
                throw new ArgumentNullException("У пользователя должна быть дата рождения.");
            }
            else if(birthDate.Year < 1900 || birthDate.Year > DateTime.Now.Year)
            {
                throw new ArgumentException(nameof(birthDate), "Год рождения не подходящий.");
            }

            //Бюджет не обязательное поле
            if(budget != null)
            {
                Budget = budget;
            }
            #endregion
            Name = name;
            Gender = gender;
            BirthDate = birthDate;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
