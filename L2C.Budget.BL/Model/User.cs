﻿using System;

namespace L2C.Budget.BL.Model
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int Id { get; }
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

        public int Age { get
            {
                var today = DateTime.Today;
                int age = today.Year - BirthDate.Year;
                if (BirthDate > today.AddYears(-age)) age--;
                return age;
            }
        }

        [field: NonSerialized]
        /// <summary>
        /// Бюджет пользователя.
        /// </summary>
        public UserBudget Budget { get; set; }
        public int BudgetId { get; }

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
                    int budgetId,
                    int id)
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

            #endregion
            Name = name;
            Gender = gender;
            BirthDate = birthDate;
            BudgetId = budgetId;
            Id = id;
        }

        public override string ToString()
        {
            return Name + " " + Age;
        }
    }
}
