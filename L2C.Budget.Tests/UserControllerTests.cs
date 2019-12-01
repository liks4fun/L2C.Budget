using L2C.Budget.BL.Controller;
using L2C.Budget.BL.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public void AddMoneyTest()
        {
            //Arrange
            var controller = GetUserController();
            float monneyCount = 666.6f;
            float moneyCountExpected = 1000 + monneyCount;

            //Act юзер mock1 устанавливается в GetUserController
            controller.AuthenUser("mock1");
            controller.AddMoney(monneyCount);
            var (_, _, balance) = controller.GetUserBalance();

            //Assert
            Assert.AreEqual(moneyCountExpected, balance);
        }

        [Test]
        public void RemoveMoneyTest()
        {
            //Arrange
            var controller = GetUserController();
            float monneyCount = 666.6f;
            float moneyCountExpected = 1000 - monneyCount;

            //Act юзер mock1 устанавливается в GetUserController
            controller.AuthenUser("mock1");
            controller.RemoveMoney(monneyCount);
            var (_, _, balance) = controller.GetUserBalance();

            //Assert
            Assert.AreEqual(moneyCountExpected, balance);
        }

        [Test]
        public void CreateNewUserTest()
        {
            //Arrange
            var mUser = new User("mock",
                    new Gender("MGender"),
                    DateTime.Today.AddYears(-21),
                    new Budget("MBudget"));
            var controller = GetUserController();

            //Act
            controller.CreateNewUser(mUser.Name, 
                mUser.Gender.Name, 
                mUser.BirthDate, 
                mUser.Budget.Name);
            controller.AuthenUser(mUser.Name);
            var (userName, budgetName, balance) = controller.GetUserBalance();

            //Assert
            Assert.AreEqual(mUser.Name, userName);
            Assert.AreEqual(mUser.Budget.Name, budgetName);
            Assert.AreEqual(mUser.Budget.Balance, balance);
        }

        private UserController GetUserController()
        {
            var mUser1 = new User("mock1",
                    new Gender("M1Gender"),
                    DateTime.Today.AddYears(-20),
                    new Budget("M1Budget"));
            mUser1.Budget.Balance += 1000f;
            var mock = new Mock<IRepository>();
            mock.Setup(r => r.GetUsers()).Returns(new List<User> { mUser1 });
            var controller = new UserController(mock.Object);
            return controller;
        }
    }
}