using System.Collections.Generic;

namespace L2C.Budget.BL.Model
{
    public interface IRepository
    {
        void SaveUsers(List<User> users);
        List<User> GetUsers();
    }
}