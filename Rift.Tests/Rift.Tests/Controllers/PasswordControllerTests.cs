using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Rift.Models;
using Rift.Controllers;
using System.Linq;

namespace Rift.Tests
{
    [TestClass]
    public class PasswordControllerTests
    {
        private Mock<UserManager<User>> _userManagerMock;

        [TestInitialize]
        public void Setup()
        {
            _userManagerMock = MockUserManager();
        }

        private PasswordController CreateControllerWithUser(string userId)
        {
            var controller = new PasswordController(_userManagerMock.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = user }
            };
            return controller;
        }

        private PasswordController CreateControllerWithoutUser()
        {
            var controller = new PasswordController(_userManagerMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext()
            };
            return controller;
        }

        [TestMethod]
        public async Task PasswordReset_ReturnsBadRequest_WhenModelStateInvalid()
        {
            var controller = CreateControllerWithUser("user-1");
            controller.ModelState.AddModelError("Any", "Error");

            var result = await controller.PasswordReset(new PasswordController.PasswordResetModel
            {
                oldPassword = "old",
                newPassword = "new",
                NewPasswordConfirmed = "new"
            });

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task PasswordReset_ReturnsBadRequest_WhenPasswordsDoNotMatch()
        {
            var controller = CreateControllerWithUser("user-1");

            var model = new PasswordController.PasswordResetModel
            {
                oldPassword = "old",
                newPassword = "new1",
                NewPasswordConfirmed = "new2"
            };

            var result = await controller.PasswordReset(model);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsTrue(badRequest.Value.ToString().Contains("do not match"));
        }

        [TestMethod]
        public async Task PasswordReset_ReturnsUnauthorized_WhenNoUserId()
        {
            var controller = CreateControllerWithoutUser();

            var model = new PasswordController.PasswordResetModel
            {
                oldPassword = "old",
                newPassword = "new",
                NewPasswordConfirmed = "new"
            };

            var result = await controller.PasswordReset(model);

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task PasswordReset_ReturnsNotFound_WhenUserNotFound()
        {
            _userManagerMock.Setup(um => um.FindByIdAsync("user-1"))
                .ReturnsAsync((User)null);

            var controller = CreateControllerWithUser("user-1");

            var model = new PasswordController.PasswordResetModel
            {
                oldPassword = "old",
                newPassword = "new",
                NewPasswordConfirmed = "new"
            };

            var result = await controller.PasswordReset(model);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task PasswordReset_ReturnsBadRequest_WhenChangePasswordFails()
        {
            var user = new User { Name = "TestUser" };
            _userManagerMock.Setup(um => um.FindByIdAsync("user-1"))
                .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.ChangePasswordAsync(user, "old", "new"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

            var controller = CreateControllerWithUser("user-1");

            var model = new PasswordController.PasswordResetModel
            {
                oldPassword = "old",
                newPassword = "new",
                NewPasswordConfirmed = "new"
            };

            var result = await controller.PasswordReset(model);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public async Task PasswordReset_ReturnsOk_WhenSuccess()
        {
            var user = new User { Name = "TestUser" };
            _userManagerMock.Setup(um => um.FindByIdAsync("user-1"))
                .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.ChangePasswordAsync(user, "old", "new"))
                .ReturnsAsync(IdentityResult.Success);

            var controller = CreateControllerWithUser("user-1");

            var model = new PasswordController.PasswordResetModel
            {
                oldPassword = "old",
                newPassword = "new",
                NewPasswordConfirmed = "new"
            };

            var result = await controller.PasswordReset(model);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Value.ToString().Contains("successfully"));
        }

        private Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(
                store.Object, null, null, null, null, null, null, null, null
            );
        }
    }
}