using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace L2C.Budget.BL.Model
{
    /// <summary>
    /// Репозиторий пользователей сохраненных в файле.
    /// </summary>
    public class FileRepository : IRepository
    {
        /// <summary>
        /// Сохранить данные пользователей в файл.
        /// </summary>
        public void SaveUsers(List<User> Users)
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream("users.bin", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Users);
            }
        }


        /// <summary>
        /// Загрузить пользователей из файла.
        /// </summary>
        /// <returns>Пользователь приложения.</returns>
        public List<User> GetUsers()
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
