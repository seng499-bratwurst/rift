using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Rift.Controllers;
using Rift.Services;
using Rift.Models;
using Rift.App.Clients;
using Microsoft.Extensions.Logging;

namespace Rift.Tests
{
    [TestClass]
    public class FileControllerTests
    {
        private Mock<IFileService> _fileServiceMock;
        private Mock<HttpMessageHandler> _httpHandlerMock;

        private HttpClient _httpClient;
        private Mock<ILogger<ChromaDBClient>> _loggerMock;

        private ChromaDBClient _client;


        [TestInitialize]
        public void Setup()
        {
            _fileServiceMock = new Mock<IFileService>();
            _httpHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpHandlerMock.Object);
            _loggerMock = new Mock<ILogger<ChromaDBClient>>();
            _client = new ChromaDBClient(_httpClient, _loggerMock.Object, "http://test");

        }

        private FileController CreateControllerWithUser(string userId, bool isAdmin)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            if (isAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            var identity = new ClaimsIdentity(claims, "mock");
            var user = new ClaimsPrincipal(identity);

            var controller = new FileController(_fileServiceMock.Object, _client)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };
            return controller;
        }

        private FileController CreateControllerWithoutUser()
        {
            var controller = new FileController(_fileServiceMock.Object, _client)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            return controller;
        }

        [TestMethod]
        public async Task UploadFile_ReturnsForbid_WhenUserNotAdmin()
        {
            var controller = CreateControllerWithUser("user1", false);
            var fileMock = new Mock<IFormFile>();

            var uploadRequest = new FileController.UploadFileRequest
            {
                File = fileMock.Object,
                SourceLink = "http://example.com/",
                SourceType = "test_source"
            };
            var result = await controller.UploadFile(uploadRequest);

            Assert.IsInstanceOfType(result, typeof(ForbidResult));
        }

        [TestMethod]
        public async Task GetAllFiles_ReturnsForbid_WhenUserNotAdmin()
        {
            var controller = CreateControllerWithUser("user1", false);

            var result = await controller.GetAllFiles();

            Assert.IsInstanceOfType(result.Result, typeof(ForbidResult));
        }

        [TestMethod]
        public async Task GetAllFiles_ReturnsOk_WhenAdmin()
        {
            var controller = CreateControllerWithUser("admin1", true);
            var files = new List<FileEntityDto>
            {
                new FileEntityDto
                {
                    Id = 1,
                    Name = "a.txt",
                    UploadedBy = "admin1",
                    SourceType = "cambridge_bay_papers",
                    SourceLink = "http://example.com/"
                }
            };
            _fileServiceMock.Setup(s => s.GetAllFilesAsync()).ReturnsAsync(files);

            var result = await controller.GetAllFiles();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<IEnumerable<FileEntityDto>>;
            Assert.IsTrue(apiResponse.Success);
            Assert.AreEqual(1, ((List<FileEntityDto>)apiResponse.Data).Count);
        }

        [TestMethod]
        public async Task DeleteFile_ReturnsForbid_WhenUserNotAdmin()
        {
            var controller = CreateControllerWithUser("user1", false);

            var result = await controller.DeleteFile(1);

            Assert.IsInstanceOfType(result, typeof(ForbidResult));
        }

        [TestMethod]
        public async Task DeleteFile_ReturnsNotFound_WhenFileNotFound()
        {
            var controller = CreateControllerWithUser("admin1", true);
            _fileServiceMock.Setup(s => s.DeleteFileByIdAsync(1)).ReturnsAsync((int?)null);

            var result = await controller.DeleteFile(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}