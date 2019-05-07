using BankingApp.DataAccess.Reposiroty;
using BankingApp.DataAccess.Uow;
using BankingApp.DataAccess.UowFactory;
using BankingApp.Models;
using BankingApp.ModelsDTO;
using BankingApp.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BancingApp.UnitTests
{
    [TestClass]
    public class BankingServiceTests
    {
        private readonly Guid valiUserId   = Guid.Parse("070F48C1-8601-4BC7-7C76-08D6A5F9E8B5");
        private readonly Guid invaliUserId = Guid.Parse("9448BD3C-0FD6-452D-7C77-08D6A5F9E8B5");
        private User invalidAmountTansferUser = new User("","");
        private User validAmountTansferUser = new User("", "");
        private readonly double invaliAmount = 1;
        private readonly double valiAmount = 10;
        private readonly double valiDeposit = 2;

        private Mock<BankingService> GetUserService()
        {
            var mockIBankingUOWFactory = new Mock<IBankingUowFactory>();
            var mockUow = new Mock<IBankingUow>();
            var mockUser = new Mock<IUserRepositiry>();
            var mockTransaction = new Mock<ITransactionRepository>();

            mockIBankingUOWFactory.Setup(bank => bank.Create())
                .Returns(mockUow.Object);

            mockUow.Setup(uow => uow.User).Returns(mockUser.Object);
            mockUow.Setup(uow => uow.Transaction).Returns(mockTransaction.Object);

            mockUow.Setup(uow => uow.Save());

            mockTransaction.Setup(trn => trn.Create(It.IsAny<Transaction>()));

            validAmountTansferUser.Amount = valiAmount;
            mockUser.Setup(ur => ur.GetById(It.Is<Guid>(g => g.Equals(valiUserId))))
                .Returns(validAmountTansferUser);

            invalidAmountTansferUser.Amount = invaliAmount;
            mockUser.Setup(ur => ur.GetById(It.Is<Guid>(g => g.Equals(invaliUserId))))
                .Returns(invalidAmountTansferUser);

            mockUser.Setup(ur => ur.GetById(It.Is<Guid>(g => g.Equals(Guid.Empty))));

            var servise = new Mock<BankingService>(mockIBankingUOWFactory.Object);

            return servise;
        }

        [TestMethod]
        public void Deposit_ValiUser_ReturnSuccedeedIsTrue()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.Deposit(new BankOperation(valiUserId, valiDeposit));

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(result.Amount == valiAmount + valiDeposit);
        }

        [TestMethod]
        public void Withdraw_ValiUser_ReturnSuccedeedIsTrue()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.Withdraw(new BankOperation(valiUserId, valiDeposit));

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(result.Amount == valiAmount - valiDeposit);
        }

        [TestMethod]
        public void Withdraw_SenderIdEqualsResipientId_ReturnSuccedeedIsFalse()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.Transfer(new BankOperation(valiUserId, valiDeposit));

            // Assert
            Assert.IsFalse(result.Succeeded);
        }

        [TestMethod]
        public void Transfer_ValidUser_ReturnSuccedeedIsTrue()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.Transfer(new BankOperation(valiUserId, invaliUserId, valiDeposit));

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(result.Amount == valiAmount - valiDeposit);
        }

        [TestMethod]
        public void Transfer_RecipientUserIsNull_ReturnSuccedeedIsFalse()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.Transfer(new BankOperation(valiUserId, Guid.Empty, valiDeposit));

            // Assert
            Assert.IsFalse(result.Succeeded);
        }

        [TestMethod]
        public void Transfer_InvalidAmount_ReturnSuccedeedIsFalse()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.Transfer(new BankOperation(invaliUserId, valiDeposit));

            // Assert
            Assert.IsFalse(result.Succeeded);
        }

        [TestMethod]
        public void Transfer_ZeroAmount_ReturnSuccedeedIsFalse()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.Transfer(new BankOperation(Guid.NewGuid(), 0));

            // Assert
            Assert.IsFalse(result.Succeeded);
        }

        [TestMethod]
        public void Transfer_SenderIdIsGuidEmpty_ReturnSuccedeedIsFalse()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.Transfer(new BankOperation(Guid.Empty, 0));

            // Assert
            Assert.IsFalse(result.Succeeded);
        }

        [TestMethod]
        public void Transfer_UserDTOIsNull_ReturnSuccedeedIsFalse()
        {
            // Arrange
            var service = GetUserService();

            // Act
            var result = service.Object.Transfer(null);

            // Assert
            Assert.IsFalse(result.Succeeded);
        }
    }
}