using BankingApp.DataAccess.Reposiroty;
using BankingApp.DataAccess.Uow;
using BankingApp.DataAccess.UowFactory;
using BankingApp.Models;
using BankingApp.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BancingApp.UnitTests
{
    [TestClass]
    public class UserServiсeTests
    {
        private readonly Guid validGuid = Guid.Parse("070F48C1-8601-4BC7-7C76-08D6A5F9E8B5");
        private User validUser = new User("name", "password");

        private Mock<UserService> GetUserService()
        {
            var mockIBankingUowFactory = new Mock<IBankingUowFactory>();
            var mockUow = new Mock<IBankingUow>();
            var mockUser = new Mock<IUserRepositiry>();

            mockIBankingUowFactory.Setup(bank => bank.Create())
                .Returns(mockUow.Object);

            mockUow.Setup(uow => uow.User).Returns(mockUser.Object);

            mockUser.Setup(ur => ur.GetById(It.Is<Guid>(g => g.Equals(validGuid))))
                .Returns(validUser);

            var servise = new Mock<UserService>(mockIBankingUowFactory.Object);

            return servise;
        }

        [TestMethod]
        public void GetUser_GuidEmpty_ReturnReturnNull()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.GetUser(Guid.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUser_GuidNewGuid_ReturnNull()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.GetUser(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUser_ValidGuid_ReturnUser()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.GetUser(validGuid);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Equals(validUser));
        }
    }
}