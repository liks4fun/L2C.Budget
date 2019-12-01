using L2C.Budget.BL.Controller;
using L2C.Budget.BL.Utils;
using System;
using System.Text;

namespace L2C.Budget.CMD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Добро пожаловать в приложение для учёта бюджета - BUDGET-40000");

            var userName = GetUserInput("Введите Ваше имя пользователя:");
            UserController controller = null;

            while (true)
            {
                #region создаем контроллер
                if(controller == null)
                {
                    try
                    {
                        controller = new UserController(userName);
                    }
                    catch (ArgumentNullException ex)
                    {
                        Console.WriteLine(ex.Message);
                        userName = GetUserInput("Введите Ваше имя пользователя:");
                        continue;
                    }
                    catch (NewUserException ex)
                    {
                        Console.WriteLine(ex.Message);
                        var genderName = GetUserInput($"{userName}, давайте зарегистриуем вас. Введите свой пол:");

                        //TODO: вынести в отдельный метод с проверкой.
                        var year = int.Parse(GetUserInput($"Введите год Вашего рождения:"));
                        var month = int.Parse(GetUserInput("Введите месяц Вашего рождения в цифарх:"));
                        var day = int.Parse(GetUserInput("Введите день Вашего рождения в цифрах:"));
                        DateTime birthday = new DateTime(year, month, day);

                        var budgetName = GetUserInput("Введите имя для своего бюджета:");
                        try
                        {
                            controller = new UserController(userName, genderName, birthday, budgetName);
                        }
                        catch (Exception except)
                        {
                            Console.WriteLine(except.Message);
                            continue;
                        }
                    }
                }
                #endregion
                var user = controller.CurrentUser;
                Console.WriteLine(user);
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Получить введенную пользователем строку.
        /// </summary>
        /// <param name="message">Сообщение пользователю перед вводом.</param>
        /// <returns>Ввод пользователя.</returns>
        public static string GetUserInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
    }
}
