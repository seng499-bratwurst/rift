using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rift.Models;
using Rift.Services;

namespace Rift.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<UserManager<User>> _userManagerMock = null!;
        private UserService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                store.Object, null, null, null, null, null, null, null, null
            );
            _service = new UserService(_userManagerMock.Object);
        }

        [TestMethod]
        public async Task UpdateUser_UpdatesNameAndEmail_WhenFound()
        {
            var user = new User { Id = "1", Name = "Old Name", Email = "old@email.com" };
            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await _service.UpdateUser("1", name: "New Name", email: "new@email.com");

            Assert.IsNotNull(result);
            Assert.AreEqual("New Name", result.Name);
            Assert.AreEqual("new@email.com", result.Email);
        }

        [TestMethod]
        public async Task UpdateUser_UpdatesOncApiToken_WhenProvided()
        {
            var user = new User { Id = "2", Name = "User", Email = "user@email.com" };
            _userManagerMock.Setup(m => m.FindByIdAsync("2")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await _service.UpdateUser("2", oncApiToken: "token123");

            Assert.IsNotNull(result);
            Assert.AreEqual("token123", user.ONCApiToken);
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsNull_WhenUserNotFound()
        {
            _userManagerMock.Setup(m => m.FindByIdAsync("3")).ReturnsAsync((User?)null);

            var result = await _service.UpdateUser("3", name: "Name");

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsNull_WhenUpdateFails()
        {
            var user = new User { Id = "4", Name = "User", Email = "user@email.com" };
            _userManagerMock.Setup(m => m.FindByIdAsync("4")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Failed());

            var result = await _service.UpdateUser("4", name: "Name");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GatherMessageHistoryAsync_ThrowsNotImplemented()
        {
            var user = new User { Id = "5", Name = "User", Email = "user@email.com" };
            Assert.ThrowsException<System.NotImplementedException>(() => _service.GatherMessageHistoryAsync(user));
        }
    }
}