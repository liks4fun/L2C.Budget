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
            CultureInfo culture;
            var cultureNames = new string[] { "ru-RU", "en-US" };
            int pos = Array.IndexOf(args, cultureNames[0]);
            if (pos > -1)
                culture = CultureInfo.CreateSpecificCulture(args[pos]);
            else if ((pos = Array.IndexOf(args, cultureNames[1])) > -1)
                culture = CultureInfo.CreateSpecificCulture(args[pos]);
            else
                culture = CultureInfo.CurrentCulture;

            var resourceManager = new ResourceManager("L2C.Budget.CMD.Resources.Lang", typeof(Program).Assembly);
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(resourceManager.GetString("Hello", culture));

            var userName = GetUserInput(resourceManager.GetString("EnterName", culture));
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
                    userName = GetUserInput(resourceManager.GetString("EnterName", culture));
                    continue;
                }
                catch (NewUserException ex)
                {
                    Console.WriteLine(ex.Message);
                    var genderName = GetUserInput($"{userName}, " + resourceManager.GetString("RegisterGender", culture));

                    //TODO: вынести в отдельный метод с проверкой.
                    var year = int.Parse(GetUserInput(resourceManager.GetString("RegisterBirthYear", culture)));
                    var month = int.Parse(GetUserInput(resourceManager.GetString("RegisterBirthMonth", culture)));
                    var day = int.Parse(GetUserInput(resourceManager.GetString("RegisterBirthDay", culture)));
                    DateTime birthday = new DateTime(year, month, day);

                    var budgetName = GetUserInput(resourceManager.GetString("RegisterBudgetName", culture));
                    try
                    {
                        controller.CreateNewUser(userName, genderName, birthday, budgetName);
                    }
                    catch (Exception except)
                    {
                        Console.WriteLine(except.Message);
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
                var userState = controller.GetUserBalance();
                Console.WriteLine(resourceManager.GetString("HelpQuit", culture));
                Console.WriteLine(resourceManager.GetString("HelpAddMoney", culture));
                Console.WriteLine(resourceManager.GetString("HelpWithdrawMoney", culture));
                Console.WriteLine($"{userState.userName}" + 
                    resourceManager.GetString("DisplayState", culture) + 
                    $" {userState.budgetName} {userState.balance:c}");
                switch(Console.ReadLine())
                {
                    case "q":
                        Environment.Exit(0);
                        break;
                    case "a":
                        Console.WriteLine(resourceManager.GetString("AddMoney", culture));
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
                            Console.WriteLine(resourceManager.GetString("ErrorAmount", culture));
                        break;
                    case "r":
                            Console.WriteLine(resourceManager.GetString("WithdrawMoney", culture));
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
                                Console.WriteLine(resourceManager.GetString("ErrorAmount", culture));
                            break;
                    default:
                        Console.WriteLine(resourceManager.GetString("ErrorInput", culture));
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
