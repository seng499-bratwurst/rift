using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Rift.Models;
using Rift.Controllers;

namespace Rift.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
        }

        private UserController CreateControllerWithUser(string userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));
            var controller = new UserController(_userServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };
            return controller;
        }

        private UserController CreateControllerWithoutUser()
        {
            var controller = new UserController(_userServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            return controller;
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsUnauthorized_WhenUserIdMissing()
        {
            var controller = CreateControllerWithoutUser();
            var update = new UserController.UserUpdate { Name = "Test", Email = "test@example.com", ONCApiToken = "token" };

            var result = await controller.UpdateUser(update);

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsNotFound_WhenUserIsNull()
        {
            _userServiceMock.Setup(s => s.UpdateUser("user1", "Test", "test@example.com", "token"))
                .ReturnsAsync((User)null);

            var controller = CreateControllerWithUser("user1");
            var update = new UserController.UserUpdate { Name = "Test", Email = "test@example.com", ONCApiToken = "token" };

            var result = await controller.UpdateUser(update);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            var apiResponse = notFound.Value as ApiResponse<User>;
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("User not found", apiResponse.Error);
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsOk_WhenUserIsUpdated()
        {
            var user = new User { Id = "user1", Name = "Test", Email = "test@example.com" };
            _userServiceMock.Setup(s => s.UpdateUser("user1", "Test", "test@example.com", "token"))
                .ReturnsAsync(user);

            var controller = CreateControllerWithUser("user1");
            var update = new UserController.UserUpdate { Name = "Test", Email = "test@example.com", ONCApiToken = "token" };

            var result = await controller.UpdateUser(update);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<User>;
            Assert.IsTrue(apiResponse.Success);
            Assert.AreEqual(user, apiResponse.Data);
        }
    }
}