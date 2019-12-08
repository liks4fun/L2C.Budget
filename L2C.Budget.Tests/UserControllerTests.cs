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

        //Вынести в отдельные тесты для FileRepository
        //[Test]
        //public void CreateNewUserTest()
        //{
        //    //Arrange
        //    var mUser = new User("mock",
        //            new Gender("MGender"),
        //            DateTime.Today.AddYears(-21),
        //            1,
        //            1)
        //    {
        //        Budget = new UserBudget(1, "mBudget2")
        //    };
        //    var controller = GetUserController();

        //    //Act
        //    controller.CreateNewUser(mUser.Name, 
        //        mUser.Gender.Name, 
        //        mUser.BirthDate, 
        //        mUser.Budget.Name);
        //    controller.AuthenUser(mUser.Name);
        //    var (userName, budgetName, balance) = controller.GetUserBalance();

        //    //Assert
        //    Assert.AreEqual(mUser.Name, userName);
        //    Assert.AreEqual(mUser.Budget.Name, budgetName);
        //    Assert.AreEqual(mUser.Budget.Balance, balance);
        //}

        private UserController GetUserController()
        {
            var mUser1 = new User("mock1",
                    new Gender("M1Gender"),
                    DateTime.Today.AddYears(-20),
                    0);
            mUser1.Budget = new UserBudget("mBudget1");
            mUser1.Budget.Balance += 1000f;
            var mock1 = new Mock<IRepository>();
            mock1.Setup(r => r.GetUser(mUser1.Name)).Returns(mUser1);
            mock1.Setup(r => r.GetBudget(mUser1.Budget.Id)).Returns(mUser1.Budget);
            mock1.Setup(r => r.GetGender(mUser1.Gender.Id)).Returns(mUser1.Gender);
            var controller = new UserController(mock1.Object);
            return controller;
        }
    }
}