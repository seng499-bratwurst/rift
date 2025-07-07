using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Rift.Models;
using Rift.Services;
using Rift.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Rift.Tests
{
    [TestClass]
    public class AuthControllerTests
    {
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<SignInManager<User>> _signInManagerMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        [TestInitialize]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                null, null, null, null);

            // Use a real IConfiguration with in-memory settings for JWT
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "testkey123456789012345678901234567890"},
                {"Jwt:Issuer", "testissuer"}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x[It.IsAny<string>()])
                .Returns((string key) => configuration[key]);
            _configurationMock.Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns((string key) => configuration.GetSection(key));

            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        }

        [TestMethod]
        public async Task Register_ReturnsBadRequest_WhenUserAlreadyExists()
        {
            var existingUser = new User { Email = "test@example.com", Name = "Test" };
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(existingUser);

            var controller = new AuthController(_userManagerMock.Object, _signInManagerMock.Object, _configurationMock.Object, _webHostEnvironmentMock.Object);

            var model = new AuthController.RegisterModel
            {
                Name = "Test",
                Email = "test@example.com",
                Password = "password"
            };

            var result = await controller.Register(model);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Register_ReturnsOk_WhenRegistrationSucceeds()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), "User"))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { "User" });

            var controller = new AuthController(_userManagerMock.Object, _signInManagerMock.Object, _configurationMock.Object, _webHostEnvironmentMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var model = new AuthController.RegisterModel
            {
                Name = "Test",
                Email = "test@example.com",
                Password = "password"
            };

            var result = await controller.Register(model);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

            var controller = new AuthController(_userManagerMock.Object, _signInManagerMock.Object, _configurationMock.Object, _webHostEnvironmentMock.Object);

            var model = new AuthController.RegisterModel
            {
                Name = "Test",
                Email = "test@example.com",
                Password = "password"
            };

            var result = await controller.Register(model);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var controller = new AuthController(_userManagerMock.Object, _signInManagerMock.Object, _configurationMock.Object, _webHostEnvironmentMock.Object);

            var model = new AuthController.LoginModel
            {
                Email = "test@example.com",
                Password = "password"
            };

            var result = await controller.Login(model);

            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task Login_ReturnsUnauthorized_WhenPasswordIsIncorrect()
        {
            var user = new User { Email = "test@example.com", Name = "Test" };
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, It.IsAny<string>(), false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var controller = new AuthController(_userManagerMock.Object, _signInManagerMock.Object, _configurationMock.Object, _webHostEnvironmentMock.Object);

            var model = new AuthController.LoginModel
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var result = await controller.Login(model);

            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task Login_ReturnsOk_WhenLoginSucceeds()
        {
            var user = new User { Email = "test@example.com", Name = "Test" };
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, It.IsAny<string>(), false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            var controller = new AuthController(_userManagerMock.Object, _signInManagerMock.Object, _configurationMock.Object, _webHostEnvironmentMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var model = new AuthController.LoginModel
            {
                Email = "test@example.com",
                Password = "password"
            };

            var result = await controller.Login(model);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}