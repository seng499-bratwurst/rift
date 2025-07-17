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
            Assert.AreEqual("test.md", result.FileName); // Filename should be normalized from .txt to .md
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

        [TestMethod]
        public async Task UploadFileAsync_NormalizesFileExtension_TxtToMd()
        {
            var file = new FileEntity { Id = 1, FileName = "document.txt", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("document.md", result.FileName);
        }

        [TestMethod]
        public async Task UploadFileAsync_PreservesFileExtension_WhenAlreadyMd()
        {
            var file = new FileEntity { Id = 1, FileName = "document.md", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("document.md", result.FileName);
        }

        [TestMethod]
        public async Task UploadFileAsync_PreservesFileExtension_WhenNotTxt()
        {
            var file = new FileEntity { Id = 1, FileName = "document.pdf", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("document.pdf", result.FileName);
        }

        [TestMethod]
        public async Task UploadFileAsync_NormalizesFileExtension_CaseInsensitive()
        {
            var file = new FileEntity { Id = 1, FileName = "document.TXT", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("document.md", result.FileName);
        }

        #region Edge Case Tests for File Extension Normalization

        [TestMethod]
        public async Task UploadFileAsync_HandlesNullFileName()
        {
            var file = new FileEntity { Id = 1, FileName = null!, Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.IsNull(result.FileName); // Should remain null
        }

        [TestMethod]
        public async Task UploadFileAsync_HandlesEmptyFileName()
        {
            var file = new FileEntity { Id = 1, FileName = "", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("", result.FileName); // Should remain empty
        }

        [TestMethod]
        public async Task UploadFileAsync_HandlesWhitespaceOnlyFileName()
        {
            var file = new FileEntity { Id = 1, FileName = "   ", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("   ", result.FileName); // Should remain unchanged
        }

        [TestMethod]
        public async Task UploadFileAsync_HandlesFileNameWithoutExtension()
        {
            var file = new FileEntity { Id = 1, FileName = "document", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("document", result.FileName); // Should remain unchanged
        }

        [TestMethod]
        public async Task UploadFileAsync_HandlesFileNameWithOnlyDot()
        {
            var file = new FileEntity { Id = 1, FileName = "document.", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("document.", result.FileName); // Should remain unchanged
        }

        [TestMethod]
        public async Task UploadFileAsync_HandlesFileNameWithMultipleDots()
        {
            var file = new FileEntity { Id = 1, FileName = "document.backup.txt", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("document.backup.md", result.FileName); // Should normalize the final extension
        }

        [TestMethod]
        public async Task UploadFileAsync_HandlesFileNameStartingWithDot()
        {
            var file = new FileEntity { Id = 1, FileName = ".txt", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual(".md", result.FileName); // Should normalize to .md
        }

        [TestMethod]
        public async Task UploadFileAsync_HandlesVeryLongFileName()
        {
            var longBaseName = new string('a', 250);
            var file = new FileEntity { Id = 1, FileName = $"{longBaseName}.txt", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual($"{longBaseName}.md", result.FileName); // Should normalize extension even for long names
        }

        [TestMethod]
        public async Task UploadFileAsync_HandlesFileNameWithSpecialCharacters()
        {
            var file = new FileEntity { Id = 1, FileName = "file-name_with@special#chars.txt", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("file-name_with@special#chars.md", result.FileName); // Should normalize extension
        }

        [TestMethod]
        public async Task UploadFileAsync_HandlesFileNameWithUnicodeCharacters()
        {
            var file = new FileEntity { Id = 1, FileName = "файл测试文档.txt", Content = "content", UploadedBy = "user" };
            _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

            var result = await _service.UploadFileAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("файл测试文档.md", result.FileName); // Should normalize extension with unicode
        }

        [TestMethod]
        public async Task UploadFileAsync_HandlesFileNameWithDifferentTxtCasing()
        {
            var testCases = new[] { ".txt", ".TXT", ".Txt", ".tXt", ".TxT" };
            
            foreach (var extension in testCases)
            {
                var file = new FileEntity { Id = 1, FileName = $"document{extension}", Content = "content", UploadedBy = "user" };
                _fileRepositoryMock.Setup(r => r.AddAsync(It.IsAny<FileEntity>())).ReturnsAsync(file);

                var result = await _service.UploadFileAsync(file);

                Assert.IsNotNull(result);
                Assert.AreEqual("document.md", result.FileName, $"Failed for extension: {extension}");
            }
        }

        #endregion

        // Note: PDF extraction test is possible but would require a real PDF byte array and PdfPig dependency.
        // You can add a test for PDF if you want to mock PdfDocument.Open and its GetPages method.
    }
}