using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rift.Models;
using Rift.Repositories;
using Rift.Services;

namespace Rift.Tests.Services
{
    [TestClass]
    public class AdminServiceTests
    {
        private Mock<IAdminRepository> _adminRepositoryMock = null!;
        private AdminService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _adminRepositoryMock = new Mock<IAdminRepository>();
            _service = new AdminService(_adminRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetUsersWithRolesAsync_ReturnsUsersAndRoles()
        {
            // Arrange
            var expected = new List<(User user, IList<string> roles)>
            {
                (new User { Id = "1", Name = "Alice" }, new List<string> { "User" }),
                (new User { Id = "2", Name = "Bob" }, new List<string> { "Admin" })
            };
            _adminRepositoryMock.Setup(r => r.GetUsersWithRolesAsync()).ReturnsAsync(expected);

            // Act
            var result = await _service.GetUsersWithRolesAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Alice", result[0].user.Name);
            CollectionAssert.AreEqual(new List<string> { "User" }, (System.Collections.ICollection)result[0].roles);
            Assert.AreEqual("Bob", result[1].user.Name);
            CollectionAssert.AreEqual(new List<string> { "Admin" }, (System.Collections.ICollection)result[1].roles);
            _adminRepositoryMock.Verify(r => r.GetUsersWithRolesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ChangeUserRoleAsync_ForwardsSuccess()
        {
            _adminRepositoryMock.Setup(r => r.ChangeUserRoleAsync("1", "Admin"))
                .ReturnsAsync(RoleChangeResult.Success);

            var result = await _service.ChangeUserRoleAsync("1", "Admin");

            Assert.AreEqual(RoleChangeResult.Success, result);
            _adminRepositoryMock.Verify(r => r.ChangeUserRoleAsync("1", "Admin"), Times.Once);
        }

        [TestMethod]
        public async Task ChangeUserRoleAsync_ForwardsUserNotFound()
        {
            _adminRepositoryMock.Setup(r => r.ChangeUserRoleAsync("1", "Admin"))
                .ReturnsAsync(RoleChangeResult.UserNotFound);

            var result = await _service.ChangeUserRoleAsync("1", "Admin");

            Assert.AreEqual(RoleChangeResult.UserNotFound, result);
        }

        [TestMethod]
        public async Task ChangeUserRoleAsync_ForwardsRemoveRolesFailed()
        {
            _adminRepositoryMock.Setup(r => r.ChangeUserRoleAsync("1", "Admin"))
                .ReturnsAsync(RoleChangeResult.RemoveRolesFailed);

            var result = await _service.ChangeUserRoleAsync("1", "Admin");

            Assert.AreEqual(RoleChangeResult.RemoveRolesFailed, result);
        }

        [TestMethod]
        public async Task ChangeUserRoleAsync_ForwardsAddRoleFailed()
        {
            _adminRepositoryMock.Setup(r => r.ChangeUserRoleAsync("1", "Admin"))
                .ReturnsAsync(RoleChangeResult.AddRoleFailed);

            var result = await _service.ChangeUserRoleAsync("1", "Admin");

            Assert.AreEqual(RoleChangeResult.AddRoleFailed, result);
        }
    }
}