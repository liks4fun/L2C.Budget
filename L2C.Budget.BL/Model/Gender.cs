using System;

namespace L2C.Budget.BL.Model
{
    /// <summary>
    /// Пол.
    /// </summary>
    [Serializable]
    public class Gender
    {
        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Создать новый пол.
        /// </summary>
        /// <param name="name">Название пола.</param>
        public Gender (string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Пол не может быть пустым или null");
            }

            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}