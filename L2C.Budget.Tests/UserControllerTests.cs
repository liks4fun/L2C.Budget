using L2C.Budget.BL.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace L2C.Budget.Tests
{
    [TestClass]
    class UserControllerTests
    {
        [TestMethod]
        public void SaveTest()
        {
            //Arrange рандомные данные для нового пользователя.
            var userName = Guid.NewGuid().ToString();
            var genderName = Guid.NewGuid().ToString();
            var birthday = DateTime.Now.AddYears(-18);
            var budgetName = Guid.NewGuid().ToString();

            //Act
            UserController controller = new UserController(userName, genderName, birthday, budgetName);
            UserController controller2 = new UserController(userName);

            //Assert
            var user1 = controller.CurrentUser;
            var user2 = controller.CurrentUser;
            Assert.AreEqual(user1.Name, userName);
            Assert.AreEqual(user2.Name, userName);
            Assert.AreEqual(user1.Gender.Name, genderName);
            Assert.AreEqual(user2.Gender.Name, genderName);
            Assert.AreEqual(user1.Budget.Name, budgetName);
            Assert.AreEqual(user2.Budget.Name, budgetName);
            Assert.AreEqual(user1.BirthDate, birthday);
            Assert.AreEqual(user2.BirthDate, birthday);

            //Удаляю тестового пользовтаеля из файла
            controller2.Users.Remove(user2);
            controller2.Save();
        }
    }
}
