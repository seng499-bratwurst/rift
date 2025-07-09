using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rift.Controllers;
using Rift.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Rift.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTests
    {
        private Mock<UserManager<User>> _userManagerMock;

        [TestInitialize]
        public void Setup()
        {
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                store.Object, null, null, null, null, null, null, null, null
            );
        }

        private AdminController CreateControllerWithAdmin()
        {
            var adminClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "admin1"),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));
            var controller = new AdminController(_userManagerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = adminClaims }
                }
            };
            return controller;
        }

        [TestMethod]
        public async Task GetUsersWithRoles_ReturnsUserList()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", Name = "Alice", Email = "alice@email.com" },
                new User { Id = "2", Name = "Bob", Email = "bob@email.com" }
            }.AsQueryable();

            _userManagerMock.Setup(x => x.Users).Returns(users);
            _userManagerMock.Setup(x => x.GetRolesAsync(It.Is<User>(u => u.Id == "1")))
                .ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(x => x.GetRolesAsync(It.Is<User>(u => u.Id == "2")))
                .ReturnsAsync(new List<string> { "Admin" });

            var controller = CreateControllerWithAdmin();

            // Act
            var result = await controller.GetUsersWithRoles();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var data = okResult.Value as ApiResponse<List<AdminController.UserWithRolesDto>>;
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Success);
            Assert.AreEqual(2, data.Data.Count);
            Assert.AreEqual("Alice", data.Data[0].Name);
            Assert.AreEqual("User", data.Data[0].Roles[0]);
            Assert.AreEqual("Bob", data.Data[1].Name);
            Assert.AreEqual("Admin", data.Data[1].Roles[0]);
        }

        [TestMethod]
        public async Task ChangeUserRole_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var user = new User { Id = "1", Name = "Alice", Email = "alice@email.com" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(x => x.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(user, "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            var controller = CreateControllerWithAdmin();
            var request = new AdminController.ChangeRoleRequest { NewRole = "Admin" };

            // Act
            var result = await controller.ChangeUserRole("1", request);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var data = okResult.Value as ApiResponse<string>;
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Success);
            Assert.AreEqual("User role changed to Admin.", data.Data);
        }

        [TestMethod]
        public async Task ChangeUserRole_ReturnsNotFound_WhenUserNotFound()
        {
            _userManagerMock.Setup(x => x.FindByIdAsync("99")).ReturnsAsync((User)null);

            var controller = CreateControllerWithAdmin();
            var request = new AdminController.ChangeRoleRequest { NewRole = "Admin" };

            var result = await controller.ChangeUserRole("99", request);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            var data = notFound.Value as ApiResponse<object>;
            Assert.IsNotNull(data);
            Assert.IsFalse(data.Success);
            Assert.AreEqual("User not found.", data.Error);
        }

        [TestMethod]
        public async Task ChangeUserRole_ReturnsBadRequest_WhenRemoveRolesFails()
        {
            var user = new User { Id = "1", Name = "Alice", Email = "alice@email.com" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(x => x.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "fail" }));

            var controller = CreateControllerWithAdmin();
            var request = new AdminController.ChangeRoleRequest { NewRole = "Admin" };

            var result = await controller.ChangeUserRole("1", request);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            var data = badRequest.Value as ApiResponse<object>;
            Assert.IsNotNull(data);
            Assert.IsFalse(data.Success);
            Assert.AreEqual("Failed to remove existing roles.", data.Error);
        }

        [TestMethod]
        public async Task ChangeUserRole_ReturnsBadRequest_WhenAddRoleFails()
        {
            var user = new User { Id = "1", Name = "Alice", Email = "alice@email.com" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(x => x.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(user, "Admin"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "fail" }));

            var controller = CreateControllerWithAdmin();
            var request = new AdminController.ChangeRoleRequest { NewRole = "Admin" };

            var result = await controller.ChangeUserRole("1", request);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            var data = badRequest.Value as ApiResponse<object>;
            Assert.IsNotNull(data);
            Assert.IsFalse(data.Success);
            Assert.AreEqual("Failed to add new role.", data.Error);
        }
    }
}