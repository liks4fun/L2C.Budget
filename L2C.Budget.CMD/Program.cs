using L2C.Budget.BL.Controller;
using System;

namespace L2C.Budget.CMD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в приложение для учёта бюджета - BUDGET-40000");

            Console.WriteLine("Введите имя пользователя:");
            var userName = Console.ReadLine();

            Console.WriteLine("Введите пол:");
            var genderName = Console.ReadLine();

            Console.WriteLine("Введите дату рождения:");
            var birthDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Введите название своего бюджета:");
            var budgetName = Console.ReadLine();

            var userController = new UserController(userName, genderName, birthDate, budgetName);
            userController.Save();
        }
    }
}
