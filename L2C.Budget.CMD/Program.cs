using L2C.Budget.BL.Controller;
using L2C.Budget.BL.Model;
using L2C.Budget.BL.Utils;
using System;
using System.Globalization;
using System.Resources;
using System.Text;

namespace L2C.Budget.CMD
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Основные параметры
            CultureInfo culture;
            var cultureNames = new string[] { "ru-RU", "en-US" };
            int pos = Array.IndexOf(args, cultureNames[0]);
            if (pos > -1)
                culture = CultureInfo.CreateSpecificCulture(args[pos]);
            else
                culture = CultureInfo.CreateSpecificCulture("en-US");

            var resourceManager = new ResourceManager("L2C.Budget.CMD.Resources.Lang", typeof(Program).Assembly);
            Console.OutputEncoding = Encoding.UTF8;
            #endregion

            Console.WriteLine(resourceManager.GetString("Hello", culture));

            var userName = GetUserInput(resourceManager.GetString("EnterName", culture));
            UserController controller = new UserController(new FileRepository<User>(), new FileRepository<UserBudget>());

            //Аутентифицируем пользователя.
            do
            {
                try
                {
                    controller.AuthenUser(userName);
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine(resourceManager.GetString("ErrorWrongUserName", culture));
                    userName = GetUserInput(resourceManager.GetString("EnterName", culture));
                    continue;
                }
                catch (NewUserException)
                {
                    Console.WriteLine(resourceManager.GetString("ErrorUserExist", culture) + userName);
                    var genderName = GetUserInput($"{userName}, " + resourceManager.GetString("RegisterGender", culture));

                    DateTime birthday = GetUserBirthDate(culture, resourceManager);

                    var budgetName = GetUserInput(resourceManager.GetString("RegisterBudgetName", culture));
                    try
                    {
                        controller.CreateNewUser(userName, genderName, birthday, budgetName);
                    }
                    catch (ArgumentException except)
                    {
                        switch (except.Message)
                        {
                            case "userName":
                                Console.WriteLine(resourceManager.GetString("ErrorUserName", culture));
                                break;
                            case "genderName":
                                Console.WriteLine(resourceManager.GetString("ErrorGenderName", culture));
                                break;
                            case "birthday":
                                Console.WriteLine(resourceManager.GetString("ErrorBirthday", culture));
                                break;
                            case "budgetName":
                                Console.WriteLine(resourceManager.GetString("ErrorBudgetName", culture));
                                break;
                            default:
                                break;
                        }
                        Console.WriteLine(resourceManager.GetString("RegisterRestart", culture));
                        userName = Console.ReadLine();
                        continue;
                    }
                }
            }
            while (!controller.IsUserAuthen);

            //Основной  цикл
            while (true)
            {
                try
                {
                    var userState = controller.GetUserBalance();
                    Console.WriteLine(resourceManager.GetString("HelpQuit", culture));
                    Console.WriteLine(resourceManager.GetString("HelpAddMoney", culture));
                    Console.WriteLine(resourceManager.GetString("HelpWithdrawMoney", culture));
                    Console.WriteLine($"{userState.userName}" +
                        resourceManager.GetString("DisplayState", culture) +
                        $" {userState.budgetName} {userState.balance:c}");
                    var key = Console.ReadKey();
                    switch (key.Key)
                    {
                        case ConsoleKey.Q:
                            Environment.Exit(0);
                            break;
                        case ConsoleKey.A:
                            Console.WriteLine(resourceManager.GetString("AddMoney", culture));
                            var addAmount = Console.ReadLine();
                            float AddAmountF = 0f;
                            if (float.TryParse(addAmount, out AddAmountF))
                            {
                                try
                                {
                                    controller.AddMoney(AddAmountF);
                                }
                                catch (ArgumentException ex)
                                {
                                    switch (ex.Message)
                                    {
                                        case "amount":
                                            Console.WriteLine(resourceManager.GetString("ErrorAddMoneyAmount", culture));
                                            break;
                                        case "Name":
                                            Console.WriteLine(resourceManager.GetString("ErrorUser", culture));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                                Console.WriteLine(resourceManager.GetString("ErrorAmount", culture));
                            break;
                        case ConsoleKey.R:
                            Console.WriteLine(resourceManager.GetString("WithdrawMoney", culture));
                            var removeAmount = Console.ReadLine();
                            float removeAmountF = 0f;
                            if (float.TryParse(removeAmount, out removeAmountF))
                            {
                                try
                                {
                                    controller.RemoveMoney(removeAmountF);
                                }

                                catch (ArgumentException ex)
                                {
                                    switch (ex.Message)
                                    {
                                        case "amount":
                                            Console.WriteLine(resourceManager.GetString("ErrorWithdrawMoneyAmount", culture));
                                            break;
                                        case "Name":
                                            Console.WriteLine(resourceManager.GetString("ErrorUser", culture));
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                                Console.WriteLine(resourceManager.GetString("ErrorAmount", culture));
                            break;
                        default:
                            Console.WriteLine(resourceManager.GetString("ErrorInput", culture));
                            break;

                    }
                }
                catch
                {
                    Console.WriteLine(resourceManager.GetString("ErrorUserState", culture));
                    continue;
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

        public static DateTime GetUserBirthDate(CultureInfo culture, ResourceManager resourceManager)
        {
            DateTime result;
            while(true)
            {
                var year = int.Parse(GetUserInput(resourceManager.GetString("RegisterBirthYear", culture)));
                var month = int.Parse(GetUserInput(resourceManager.GetString("RegisterBirthMonth", culture)));
                var day = int.Parse(GetUserInput(resourceManager.GetString("RegisterBirthDay", culture)));

                if (DateTime.TryParse($"{year}.{month}.{day}", out result))
                    break;
                Console.WriteLine(resourceManager.GetString("ErrorBirthDate", culture));
            }

            return result;

        }
    }
}
