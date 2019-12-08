using System;
using System.Collections.Generic;

namespace L2C.Budget.BL.Model
{
    public interface IRepository
    {
        void SaveUsers();
        void SaveGenders();
        void SaveBudgets();
        User GetUser(string userName);
        Gender GetGender(int genderId);
        UserBudget GetBudget(int budgetId);

        void CreateUser(string userName, string genderName, DateTime birthday, string budgetName);
    }
}