using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rift.Models;
using Rift.Repositories;

namespace Rift.Tests.Repositories
{
    [TestClass]
    public class AdminRepositoryTests
    {
        private Mock<UserManager<User>> _userManagerMock = null!;
        private AdminRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                store.Object, null, null, null, null, null, null, null, null
            );
            _repository = new AdminRepository(_userManagerMock.Object);
        }

        [TestMethod]
        public async Task GetUsersWithRolesAsync_ReturnsUsersAndRoles()
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

            // Act
            var result = await _repository.GetUsersWithRolesAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Alice", result[0].user.Name);
            CollectionAssert.AreEqual(new List<string> { "User" }, result[0].roles.ToList());
            Assert.AreEqual("Bob", result[1].user.Name);
            CollectionAssert.AreEqual(new List<string> { "Admin" }, result[1].roles.ToList());
        }

        [TestMethod]
        public async Task ChangeUserRoleAsync_ReturnsSuccess_WhenSuccessful()
        {
            // Arrange
            var user = new User { Id = "1", Name = "Alice" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(x => x.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(user, "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _repository.ChangeUserRoleAsync("1", "Admin");

            // Assert
            Assert.AreEqual(RoleChangeResult.Success, result);
        }

        [TestMethod]
        public async Task ChangeUserRoleAsync_ReturnsUserNotFound_WhenUserDoesNotExist()
        {
            _userManagerMock.Setup(x => x.FindByIdAsync("99")).ReturnsAsync((User)null);

            var result = await _repository.ChangeUserRoleAsync("99", "Admin");

            Assert.AreEqual(RoleChangeResult.UserNotFound, result);
        }

        [TestMethod]
        public async Task ChangeUserRoleAsync_ReturnsRemoveRolesFailed_WhenRemoveRolesFails()
        {
            var user = new User { Id = "1", Name = "Alice" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(x => x.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "fail" }));

            var result = await _repository.ChangeUserRoleAsync("1", "Admin");

            Assert.AreEqual(RoleChangeResult.RemoveRolesFailed, result);
        }

        [TestMethod]
        public async Task ChangeUserRoleAsync_ReturnsAddRoleFailed_WhenAddRoleFails()
        {
            var user = new User { Id = "1", Name = "Alice" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(x => x.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(user, "Admin"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "fail" }));

            var result = await _repository.ChangeUserRoleAsync("1", "Admin");

            Assert.AreEqual(RoleChangeResult.AddRoleFailed, result);
        }
    }
}