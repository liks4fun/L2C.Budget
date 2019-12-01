using L2C.Budget.BL.Controller;
using L2C.Budget.BL.Model;
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
            UserController controller = new UserController(new FileRepository());

            //Аутентифицируем пользователя.
            do
            {
                try
                {
                    controller.AuthenUser(userName);
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
                        controller.CreateNewUser(userName, genderName, birthday, budgetName);
                    }
                    catch (Exception except)
                    {
                        Console.WriteLine(except.Message);
                        Console.WriteLine("Давай попробуем ещё раз, введите имя пользователя:");
                        userName = Console.ReadLine();
                        continue;
                    }
                }
            }
            while (!controller.IsUserAuthen);

            while (true)
            {
                var userState = controller.GetUserBalance();
                Console.WriteLine("q - выйти из приложения");
                Console.WriteLine("а - добавить денег на баланс");
                Console.WriteLine("r - снять денег с баланса");
                Console.WriteLine($"{userState.userName}, в Вашем бюджете {userState.budgetName} {userState.balance}uah");
                switch(Console.ReadLine())
                {
                    case "q":
                        Environment.Exit(0);
                        break;
                    case "a":
                        Console.WriteLine("Введите сумму для зачисления:");
                        var addAmount = Console.ReadLine();
                        float AddAmountF = 0f;
                        if (float.TryParse(addAmount, out AddAmountF))
                        {
                            try
                            {
                                controller.AddMoney(AddAmountF);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                            Console.WriteLine("Не верная сумма.");
                        break;
                    case "r":
                            Console.WriteLine("Введите сумму для снятия:");
                            var removeAmount = Console.ReadLine();
                            float removeAmountF = 0f;
                            if (float.TryParse(removeAmount, out removeAmountF))
                            {
                                try
                                {
                                    controller.RemoveMoney(removeAmountF);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            else
                                Console.WriteLine("Не верная сумма.");
                            break;
                    default:
                        Console.WriteLine("Не верная команда.");
                        break;

                }
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
