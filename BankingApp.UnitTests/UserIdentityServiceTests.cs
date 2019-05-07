using BankingApp.DataAccess.Reposiroty;
using BankingApp.DataAccess.Uow;
using BankingApp.DataAccess.UowFactory;
using BankingApp.Models;
using BankingApp.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BancingApp.UnitTests
{
    [TestClass]
    public class UserIdentityServiceTests
    {
        private readonly User validUser = new User("name", "password");
        private List<User> users = new List<User>();

        private Mock<UserIdentityService> GetUserService()
        {
            var mockIBankingUowFactory = new Mock<IBankingUowFactory>();
            var mockUow = new Mock<IBankingUow>();
            var mockUser = new Mock<IUserRepositiry>();

            mockIBankingUowFactory.Setup(bank => bank.Create()).Returns(mockUow.Object);

            mockUow.Setup(uow => uow.User).Returns(mockUser.Object);
            mockUow.Setup(uow => uow.Save());

            mockUser.Setup(us => us.Create(It.IsAny<User>()));
            mockUser.Setup(us => us.Get(It.IsAny<Expression<Func<User, bool>>>(), null)).Returns(users);

            var servise = new Mock<UserIdentityService>(mockIBankingUowFactory.Object);

            return servise;
        }

        [TestMethod]
        public void GetIdentity_UserIsNull_ReturnNull()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.GetIdentity(null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RegisterUsery_UserIsNull_ReturnSuccedeedIsFalse()
        {
            // Arrange
            users.Add(validUser);
            var service = GetUserService();

            // Act
            var result = service.Object.RegisterUser(validUser.Name, validUser.Password);

            // Assert
            Assert.IsFalse(result.Succeeded);
        }

        [TestMethod]
        public void RegisterUsery_PasswordIsNull_ReturnSuccedeedIsFalse()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.RegisterUser(validUser.Name, null);

            // Assert
            Assert.IsFalse(result.Succeeded);
        }

        [TestMethod]
        public void RegisterUsery_ValidData_ReturnSuccedeedIsTrue()
        {
            // Arrange
            var users = new List<User>();
            var service = GetUserService();

            // Act
            var result = service.Object.RegisterUser(validUser.Name, validUser.Name);

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(result.Amount == 0);
        }

        [TestMethod]
        public void RegisterUsery_PasswordIsEmpty_ReturnSuccedeedIsFalse()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.RegisterUser(validUser.Name, "");

            // Assert
            Assert.IsFalse(result.Succeeded);
        }

        [TestMethod]
        public void RegisterUsery_UsernameIsEmpty_ReturnSuccedeedIsFalse()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.RegisterUser("", validUser.Password);

            // Assert
            Assert.IsFalse(result.Succeeded);
        }

        [TestMethod]
        public void RegisterUsery_UsernameIsNull_ReturnSuccedeedIsFalse()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.RegisterUser(null, validUser.Password);

            // Assert
            Assert.IsFalse(result.Succeeded);
        }
    }
}