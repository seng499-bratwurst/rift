using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rift.Controllers;
using Rift.Services;
using Rift.Models;

namespace Rift.Tests
{
    [TestClass]
    public class FileControllerTests
    {
        private Mock<IFileService> _fileServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _fileServiceMock = new Mock<IFileService>();
        }

        private FileController CreateControllerWithUser(string userId, bool isAdmin)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            if (isAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            var identity = new ClaimsIdentity(claims, "mock");
            var user = new ClaimsPrincipal(identity);

            var controller = new FileController(_fileServiceMock.Object)
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
            var controller = new FileController(_fileServiceMock.Object)
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
        public async Task UploadFile_ReturnsOk_WhenAdmin()
        {
            var controller = CreateControllerWithUser("admin1", true);
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.txt");
            fileMock.Setup(f => f.Length).Returns(123);

            _fileServiceMock.Setup(s => s.ExtractTextAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("file content");
            _fileServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<FileEntity>()))
                .ReturnsAsync(new FileEntity
                {
                    Id = 42,
                    FileName = "test.txt",
                    Content = "file content",
                    UploadedBy = "admin1",
                    Size = 123,
                    CreatedAt = System.DateTime.UtcNow
                });

            var uploadRequest = new FileController.UploadFileRequest
            {
                File = fileMock.Object,
                SourceLink = "http://example.com/",
                SourceType = "test_source"
            };

            var result = await controller.UploadFile(uploadRequest);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<int>;
            Assert.IsTrue(apiResponse.Success);
            Assert.AreEqual(42, apiResponse.Data);
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
                    FileName = "a.txt",
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

        [TestMethod]
        public async Task DeleteFile_ReturnsOk_WhenFileDeleted()
        {
            var controller = CreateControllerWithUser("admin1", true);
            _fileServiceMock.Setup(s => s.DeleteFileByIdAsync(1)).ReturnsAsync(1);

            var result = await controller.DeleteFile(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var apiResponse = okResult.Value as ApiResponse<object>;
            Assert.IsNotNull(apiResponse);
            Assert.IsTrue(apiResponse.Success);
            Assert.IsNotNull(apiResponse.Data);

            // Use reflection to get DeletedId from anonymous object
            var deletedIdProp = apiResponse.Data.GetType().GetProperty("DeletedId");
            Assert.IsNotNull(deletedIdProp, "DeletedId property not found on Data");
            var deletedIdValue = deletedIdProp.GetValue(apiResponse.Data);
            Assert.AreEqual(1, deletedIdValue);
        }
    }
}