using L2C.Budget.BL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace L2C.Budget.BL.Controller
{
    public class EFRepository : IRepository
    {
        readonly AppDbContext db;


        public void CreateUser(string userName, string genderName, DateTime birthday, string budgetName)
        { 
            List<User> users = db.Users.ToList();
            List<Gender> genders = db.Genders.ToList();
            List<UserBudget> budgets = db.Budgets.ToList();
            if (string.IsNullOrWhiteSpace(genderName))
                throw new ArgumentNullException(nameof(genderName));
            if (string.IsNullOrWhiteSpace(budgetName))
                throw new ArgumentNullException(nameof(budgetName));
            if (birthday.Year < 1900 || birthday > DateTime.Now)
                throw new ArgumentException(nameof(birthday));
            if (users.Contains(users.SingleOrDefault(u => u.Name == userName)))
            {
                throw new ArgumentException(nameof(userName));
            }

            UserBudget budget = budgets.SingleOrDefault(b => b.Name == budgetName);
            if (budget == null)
            {
                budget = new UserBudget(budgetName);
                db.Budgets.Add(budget);
                db.Entry(budget).State = EntityState.Added;
                SaveBudgets();
            }
            Gender gender = genders.SingleOrDefault(g => g.Name == genderName);
            if (gender == null)
            {
                gender = new Gender(genderName);
                db.Genders.Add(gender);
                db.Entry(gender).State = EntityState.Added;
                SaveGenders();
            }
            var user = new User(userName, gender, birthday, budget.Id);
            db.Users.Add(user);
            db.Entry(user).State = EntityState.Added;
            db.SaveChanges();
        }

        public UserBudget GetBudget(int budgetId)
        {
            return db.Budgets.ToList().SingleOrDefault(b => b.Id == budgetId);
        }

        public Gender GetGender(int genderId)
        {
            return db.Genders.ToList().SingleOrDefault(g => g.Id == genderId);
        }

        public User GetUser(string userName)
        {
            return db.Users.ToList().SingleOrDefault(u => u.Name == userName);
        }

        public void SaveBudgets()
        {
            db.SaveChanges();
        }

        public void SaveGenders()
        {
            db.SaveChanges();
        }

        public void SaveUsers()
        {
            db.SaveChanges();
        }

        public EFRepository()
        {
            db = new AppDbContext();
        }
    }
}
