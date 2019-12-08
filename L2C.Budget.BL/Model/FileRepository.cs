using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace L2C.Budget.BL.Model
{
    /// <summary>
    /// Репозиторий пользователей сохраненных в файле.
    /// </summary>
    public class FileRepository<T> : IRepository<T>
    {
        /// <summary>
        /// Сохранить данные пользователей в файл.
        /// </summary>
        public void Save(List<T> data, string fileName)
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, data);
            }
        }


        /// <summary>
        /// Загрузить пользователей из файла.
        /// </summary>
        /// <returns>Пользователь приложения.</returns>
        public List<T> Get(string fileName)
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                if (fs.Length > 0 && formatter.Deserialize(fs) is List<T> entities)
                    return entities;
                else
                    return new List<T>();
            }
        }
    }
}
