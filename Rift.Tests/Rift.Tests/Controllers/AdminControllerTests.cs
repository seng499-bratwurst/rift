using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rift.Controllers;
using Rift.Models;
using Rift.Services;

namespace Rift.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTests
    {
        private Mock<IAdminService> _adminServiceMock = null!;
        private AdminController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _adminServiceMock = new Mock<IAdminService>();
            _controller = new AdminController(_adminServiceMock.Object);
        }

        [TestMethod]
        public async Task GetUsersWithRoles_ReturnsUserList()
        {
            // Arrange
            var usersWithRoles = new List<(User user, IList<string> roles)>
            {
                (new User { Id = "1", Name = "Alice", Email = "alice@email.com" }, new List<string> { "User" }),
                (new User { Id = "2", Name = "Bob", Email = "bob@email.com" }, new List<string> { "Admin" })
            };
            _adminServiceMock.Setup(s => s.GetUsersWithRolesAsync()).ReturnsAsync(usersWithRoles);

            // Act
            var result = await _controller.GetUsersWithRoles();

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
            _adminServiceMock.Setup(s => s.ChangeUserRoleAsync("1", "Admin"))
                .ReturnsAsync(AdminController.RoleChangeResult.Success);

            var request = new AdminController.ChangeRoleRequest { NewRole = "Admin" };
            var result = await _controller.ChangeUserRole("1", request);

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
            _adminServiceMock.Setup(s => s.ChangeUserRoleAsync("99", "Admin"))
                .ReturnsAsync(AdminController.RoleChangeResult.UserNotFound);

            var request = new AdminController.ChangeRoleRequest { NewRole = "Admin" };
            var result = await _controller.ChangeUserRole("99", request);

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
            _adminServiceMock.Setup(s => s.ChangeUserRoleAsync("1", "Admin"))
                .ReturnsAsync(AdminController.RoleChangeResult.RemoveRolesFailed);

            var request = new AdminController.ChangeRoleRequest { NewRole = "Admin" };
            var result = await _controller.ChangeUserRole("1", request);

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
            _adminServiceMock.Setup(s => s.ChangeUserRoleAsync("1", "Admin"))
                .ReturnsAsync(AdminController.RoleChangeResult.AddRoleFailed);

            var request = new AdminController.ChangeRoleRequest { NewRole = "Admin" };
            var result = await _controller.ChangeUserRole("1", request);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            var data = badRequest.Value as ApiResponse<object>;
            Assert.IsNotNull(data);
            Assert.IsFalse(data.Success);
            Assert.AreEqual("Failed to add new role.", data.Error);
        }

        [TestMethod]
        public async Task ChangeUserRole_ReturnsBadRequest_WhenUnknownError()
        {
            _adminServiceMock.Setup(s => s.ChangeUserRoleAsync("1", "Admin"))
                .ReturnsAsync((AdminController.RoleChangeResult)999);

            var request = new AdminController.ChangeRoleRequest { NewRole = "Admin" };
            var result = await _controller.ChangeUserRole("1", request);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            var data = badRequest.Value as ApiResponse<object>;
            Assert.IsNotNull(data);
            Assert.IsFalse(data.Success);
            Assert.AreEqual("Unknown error.", data.Error);
        }
    }
}