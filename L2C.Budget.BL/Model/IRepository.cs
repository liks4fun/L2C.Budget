using System.Collections.Generic;

namespace L2C.Budget.BL.Model
{
    public interface IRepository<T>
    {
        void Save(List<T> list, string fileName);
        List<T> Get(string fileName);
    }
}