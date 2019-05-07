using BankingApp.Models;
using BankingApp.ModelsDTO;
using BankingApp.Services.Interface;
using BankingApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BancingApp.UnitTests
{
    [TestClass]
    public class UserIdentityControllerTests
    {
        private readonly User validUser = new User("name", "password");
        private readonly User invalidUser = new User("", "");
        private readonly OperationDetails validOperationDetails = OperationDetails.Success(1);

        private UserIdentityController GetUserIdentityController()
        {
            var mockIdentityService = new Mock<IUserIdentityService>();
            mockIdentityService.Setup(identit => identit.IdentityUser(It.Is<string>(s => s.Contains(invalidUser.Name)), 
                    It.IsAny<string>())).Returns(invalidUser);

            mockIdentityService.Setup(identit => identit.IdentityUser(It.Is<string>(s => s.Contains(validUser.Name)),
                    It.Is<string>(s => s.Contains(validUser.Password)))).Returns(validUser);

            mockIdentityService.Setup(identit => identit.RegisterUser(It.Is<string>(s => s.Contains(validUser.Name)), 
                    It.IsAny<string>())).Returns(OperationDetails.Error(""));

            mockIdentityService.Setup(identit => identit.RegisterUser(It.Is<string>(s => s.Contains(validUser.Name)), 
                It.Is<string>(s => s.Contains(validUser.Password)))).Returns(validOperationDetails);

            mockIdentityService.Setup(identit => identit.GetIdentity(It.Is<User>(u => u.Name == validUser.Name && u.Password == validUser.Password)))
                .Returns("token");

            var controller = new UserIdentityController(mockIdentityService.Object);

            return controller;
        }

        [TestMethod]
        public void Login_ValidUser_Returne200()
        {
            // Arrange
            var controller = GetUserIdentityController();

            // Act
            IActionResult result = controller.Login(new Identity { Name = validUser.Name, Password = validUser.Password });
            var ok = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
        }

        [TestMethod]
        public void Login_TokenIsNull_Returne400()
        {
            // Arrange
            var controller = GetUserIdentityController();

            // Act
            IActionResult result = controller.Login(new Identity { Name = validUser.Name, Password = invalidUser.Password });
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void Login_UserNotFound_Returne400()
        {
            // Arrange
            var controller = GetUserIdentityController();

            // Act
            IActionResult result = controller.Login(new Identity { Name = invalidUser.Name, Password = invalidUser.Password });
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void Login_UserIsNull_Returne400()
        {
            // Arrange
            var controller = GetUserIdentityController();

            // Act
            IActionResult result = controller.Login(null);
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void RegisterUser_UserIsNull_Return400()
        {
            // Arrange
            var controller = GetUserIdentityController();

            // Act
            var result = controller.RegisterUser(null);
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void RegisterUser_ValidUser_Return200()
        {
            // Arrange
            var controller = GetUserIdentityController();

            // Act
            var result = controller.RegisterUser(new Identity { Name = validUser.Name, Password = validUser.Password });
            var okResult = result as OkObjectResult;
            var operationDetails = okResult.Value as OperationDetails;
                 
            // Assert
            Assert.IsTrue(operationDetails.Equals(validOperationDetails));
        }

        [TestMethod]
        public void RegisterUser_InalidUser_Return400()
        {
            // Arrange
            var controller = GetUserIdentityController();

            // Act
            var result = controller.RegisterUser(new Identity { Name = validUser.Name, Password = invalidUser.Password });
            var badRequest = result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }
    }
}