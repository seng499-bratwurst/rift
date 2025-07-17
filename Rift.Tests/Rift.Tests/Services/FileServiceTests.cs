using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rift.Models;
using Rift.Repositories;
using Rift.Services;

namespace Rift.Tests.Services
{
    [TestClass]
    public class FileServiceTests
    {
        private Mock<IFileRepository> _fileRepositoryMock = null!;
        private FileService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _fileRepositoryMock = new Mock<IFileRepository>();
            _service = new FileService(_fileRepositoryMock.Object);
        }

        [TestMethod]
        public async Task UploadFileAsync_ReturnsFileEntity()
        {
            var file = new FileEntity { Id = 1, Name = "test.txt", Content = "abc", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(file)).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("test.txt", result.Name);
        }

        [TestMethod]
        public async Task GetAllFilesAsync_ReturnsFileEntityDtos()
        {
            var files = new List<FileEntityDto>
            {
                new FileEntityDto { Id = 1, FileName = "a.txt", UploadedBy = "user", SourceType = "cambridge_bay_papers", SourceLink = "http://example.com/" },
                new FileEntityDto { Id = 2, FileName = "b.txt", UploadedBy = "user", SourceType = "cambridge_bay_papers", SourceLink = "http://example.com/"  }
            };
            _fileRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(files);

            var result = await _service.GetAllFilesAsync();

            Assert.AreEqual(2, ((List<FileEntityDto>)result).Count);
            Assert.AreEqual("a.txt", ((List<FileEntityDto>)result)[0].FileName);
        }

        [TestMethod]
        public async Task DeleteFileByIdAsync_ReturnsId_WhenDeleted()
        {
            _fileRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

            var result = await _service.DeleteFileByIdAsync(1);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task DeleteFileByIdAsync_ReturnsNull_WhenNotFound()
        {
            _fileRepositoryMock.Setup(r => r.DeleteAsync(2)).ReturnsAsync((int?)null);

            var result = await _service.DeleteFileByIdAsync(2);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task ExtractTextAsync_ReturnsText_ForTxtFile()
        {
            var content = "Hello, world!";
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            fileMock.Setup(f => f.FileName).Returns("test.txt");
            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

            var result = await _service.ExtractTextAsync(fileMock.Object);

            Assert.AreEqual(content, result);
        }

        [TestMethod]
        public async Task ExtractTextAsync_ReturnsText_ForMdFile()
        {
            var content = "# Markdown";
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            fileMock.Setup(f => f.FileName).Returns("test.md");
            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

            var result = await _service.ExtractTextAsync(fileMock.Object);

            Assert.AreEqual(content, result);
        }

        [TestMethod]
        public async Task ExtractTextAsync_ReturnsEmpty_ForUnsupportedExtension()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.exe");
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

            var result = await _service.ExtractTextAsync(fileMock.Object);

            Assert.AreEqual(string.Empty, result);
        }

        // Note: PDF extraction test is possible but would require a real PDF byte array and PdfPig dependency.
        // You can add a test for PDF if you want to mock PdfDocument.Open and its GetPages method.
    }
}