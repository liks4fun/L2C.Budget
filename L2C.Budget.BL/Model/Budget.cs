using System;

namespace L2C.Budget.BL.Model
{
    /// <summary>
    /// Бюджет
    /// </summary>
    [Serializable]
    public class Budget
    {
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
        public Budget(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Имя не может быть пустым или null", nameof(name));
            }
            Name = name;
            Balance = 0.0f;
        }
    }
}
