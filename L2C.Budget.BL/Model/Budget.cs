using System;

namespace L2C.Budget.BL.Model
{
    /// <summary>
    /// Бюджет
    /// </summary>
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

            Balance = 0.0f;
        }

        /// <summary>
        /// Добавить денег на баланс.
        /// </summary>
        /// <param name="money">Кол-во денег.</param>
        public void AddMoney(float money)
        {
            if(money <= 0)
            {
                throw new ArgumentException(nameof(money), "Нельзя добавить к бюджету отрицательное или равное нулю значение");
            }

            Balance += money;
        }

        /// <summary>
        /// Снять денег с баланса.
        /// </summary>
        /// <param name="money">Кол-во денег.</param>
        public void RemoveMoney(float money)
        {
            if(money >= 0)
            {
                throw new ArgumentException(nameof(money), "Нельзя снять с баланса положительное или равное нулю число");
            }

            Balance -= money;
        }
    }
}
