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
                    1,
                    1)
            {
                Budget = new UserBudget(1, "mBudget2")
            };
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
                    0,
                    0);
            mUser1.Budget = new UserBudget(0, "mBudget1");
            mUser1.Budget.Balance += 1000f;
            var mock1 = new Mock<IRepository<User>>();
            mock1.Setup(r => r.Get("users.bin")).Returns(new List<User> { mUser1 });
            var mock2 = new Mock<IRepository<UserBudget>>();
            mock2.Setup(r => r.Get("budgets.bin")).Returns(new List<UserBudget> { mUser1.Budget });
            var controller = new UserController(mock1.Object, mock2.Object);
            return controller;
        }
    }
}