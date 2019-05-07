using BankingApp.ModelsDTO;
using BankingApp.Services.Interface;
using BankingApp.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Security.Claims;

namespace BancingApp.UnitTests
{
    [TestClass]
    public class BankingControllerTests
    {
        private readonly OperationDetails valiOperationDetails = OperationDetails.Success(5);
        private readonly OperationDetails invaliOperationDetails = OperationDetails.Error("Operation error");
        private BankOperation validUser = new BankOperation(Guid.Empty, 1);
        private BankOperation invalidUser = new BankOperation(Guid.NewGuid(), 0);
        private BankOperation invalidTransferUser = new BankOperation(Guid.NewGuid(), Guid.Empty, 1);
        private BankOperation validTransferUser = new BankOperation(Guid.NewGuid(), Guid.Empty, 1);

        private BankingController GetBankingController()
        {
            var mockBankingService = new Mock<IBankingService>();
            mockBankingService.Setup(bank => bank.Withdraw(It.IsAny<BankOperation>()))
                .Returns(invaliOperationDetails);

            mockBankingService.Setup(bank => bank.Withdraw(It.Is<BankOperation>(us => us.Amount == validUser.Amount)))
                .Returns(valiOperationDetails);

            mockBankingService.Setup(bank => bank.Deposit(It.IsAny<BankOperation>()))
                .Returns(invaliOperationDetails);

            mockBankingService.Setup(bank => bank.Deposit(It.Is<BankOperation>(us => us.Amount == validUser.Amount)))
                .Returns(valiOperationDetails);

            mockBankingService.Setup(bank => bank.Transfer(It.IsAny<BankOperation>()))
                .Returns(invaliOperationDetails);

            mockBankingService.Setup(bank => bank.Transfer(It.Is<BankOperation>(us => us.Equals(validTransferUser))))
                .Returns(valiOperationDetails);

            mockBankingService.Setup(bank => bank.GetBankOperationMinAmoun())
                .Returns(1);

            var mockidentityServise = new Mock<IUserIdentityService>();

            var controller = new BankingController(mockBankingService.Object, mockidentityServise.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim( "UserId", "0"),
            }));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            return controller;
        }

        [TestMethod]
        public void Transfer_ZeroAmount_Return400()
        {
            // Arrange
            var controller = GetBankingController();

            // Act
            var result = controller.Transfer(invalidUser);
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void Transfer_ValidUset_ReturnSuccedeedIsTrue()
        {
            // Arrange
            var controller = GetBankingController();

            // Act
            var result = controller.Transfer(validTransferUser);
            var okResult = result as OkObjectResult;
            var operationDetails = okResult?.Value as OperationDetails;

            // Assert
            Assert.IsNotNull(operationDetails);
            Assert.IsTrue(operationDetails.Equals(valiOperationDetails));
        }

        [TestMethod]
        public void Transfer_InvalidUset_ReturnSuccedeedIsFalse()
        {
            // Arrange
            var controller = GetBankingController();

            // Act
            var result = controller.Transfer(invalidTransferUser);
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void Transfer_ModelIsNull_Return400()
        {
            // Arrange
            var controller = GetBankingController();

            // Act
            var result = controller.Transfer(null);
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void Withdraw_ZeroAmount_Return400()
        {
            // Arrange
            var controller = GetBankingController();

            // Act
            var result = controller.Withdraw(0);
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void Withdraw_ValidAmount_ReturnSuccedeedIsTrue()
        {
            // Arrange
            var controller = GetBankingController();

            // Act
            var result = controller.Withdraw(validUser.Amount);
            var okResult = result as OkObjectResult;
            var operationDetails = okResult?.Value as OperationDetails;

            // Assert
            Assert.IsNotNull(operationDetails);
            Assert.IsTrue(operationDetails.Equals((valiOperationDetails)));
        }

        [TestMethod]
        public void Deposit_ZeroAmount_Return400()
        {
            // Arrange
            var controller = GetBankingController();

            // Act
            var result = controller.Deposit(0);
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void Deposit_ValidAmount_ReturnSuccedeedIsTrue()
        {
            // Arrange
            var controller = GetBankingController();

            //// Act
            var result = controller.Deposit(validUser.Amount);
            var okResult = result as OkObjectResult;
            var operationDetails = okResult?.Value as OperationDetails;

            // Assert
            Assert.IsNotNull(operationDetails);
            Assert.IsTrue(operationDetails.Equals((valiOperationDetails)));
        }
    }
}