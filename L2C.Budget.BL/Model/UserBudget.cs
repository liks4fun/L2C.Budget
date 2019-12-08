using System;
using System.Collections.Generic;

namespace L2C.Budget.BL.Model
{
    /// <summary>
    /// Бюджет
    /// </summary>
    [Serializable]
    public class UserBudget
    {
        /// <summary>
        /// Идентификатор бюджета.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Создатель бюджета.
        /// </summary>
        public int OwnerId { get; }

        /// <summary>
        /// Список пользователей бюджета.
        /// </summary>
        public List<int> BudgetUsersIds { get; }

        /// <summary>
        /// Баланс.
        /// </summary>
        public float Balance { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Создать новый бюджет.
        /// </summary>
        /// <param name="name">Имя бюджета.</param>
        public UserBudget(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Имя не может быть пустым или null", nameof(name));
            }
            Name = name;
            Id = id;
        }
    }
}
