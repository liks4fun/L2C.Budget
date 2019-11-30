using L2C.Budget.BL.Controller;
using NUnit.Framework;
using System;

namespace Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public void SaveTest()
        {
            //Arrange рандомные данные для нового пользователя.
            var userName = Guid.NewGuid().ToString();
            var genderName = Guid.NewGuid().ToString();
            var birthday = DateTime.Now.AddYears(-18);
            var budgetName = Guid.NewGuid().ToString();

            //Act проверяем сразу 2 конструктора для нового и для существующего пользователя
            UserController controller = new UserController(userName, genderName, birthday, budgetName);
            UserController controller2 = new UserController(userName);

            //Assert
            var user1 = controller.CurrentUser;
            var user2 = controller.CurrentUser;
            Assert.AreEqual(userName, user1.Name);
            Assert.AreEqual(userName, user2.Name);
            Assert.AreEqual(genderName, user1.Gender.Name);
            Assert.AreEqual(genderName, user2.Gender.Name);
            Assert.AreEqual(budgetName, user1.Budget.Name);
            Assert.AreEqual(budgetName, user2.Budget.Name);
            Assert.AreEqual(birthday, user1.BirthDate);
            Assert.AreEqual(birthday, user2.BirthDate);

            //Удаляю тестового пользовтаеля из файла
            controller2.Users.Remove(user2);
            controller2.Save();
        }
    }
}