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
        /// Идентификатор пола.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

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

        public Gender() { }

        public override string ToString()
        {
            return Name;
        }
    }
}