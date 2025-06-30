using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;
using Rift.Repositories;

namespace Rift.Tests.Repositories
{
    [TestClass]
    public class FileRepositoryTests
    {
        private DbContextOptions<FileDbContext> _dbOptions = null!;
        private FileDbContext _dbContext = null!;
        private FileRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            _dbOptions = new DbContextOptionsBuilder<FileDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new FileDbContext(_dbOptions);
            _repository = new FileRepository(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task AddAsync_AddsAndReturnsFile()
        {
            var file = new FileEntity
            {
                FileName = "test.txt",
                Content = "abc",
                UploadedBy = "user",
                Size = 123,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _repository.AddAsync(file);

            Assert.IsNotNull(result);
            Assert.AreEqual("test.txt", result.FileName);

            var dbFile = await _dbContext.Files.FindAsync(result.Id);
            Assert.IsNotNull(dbFile);
            Assert.AreEqual("user", dbFile.UploadedBy);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllFilesAsDtos()
        {
            _dbContext.Files.AddRange(
                new FileEntity { FileName = "a.txt", Content = "A", UploadedBy = "user1", Size = 1, CreatedAt = DateTime.UtcNow },
                new FileEntity { FileName = "b.txt", Content = "B", UploadedBy = "user2", Size = 2, CreatedAt = DateTime.UtcNow }
            );
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            var list = result.ToList();
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("a.txt", list[0].FileName);
            Assert.AreEqual("b.txt", list[1].FileName);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsFile_WhenExists()
        {
            var file = new FileEntity
            {
                FileName = "findme.txt",
                Content = "find",
                UploadedBy = "user",
                Size = 10,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Files.Add(file);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(file.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("findme.txt", result.FileName);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var result = await _repository.GetByIdAsync(9999);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteAsync_RemovesFileAndReturnsId_WhenExists()
        {
            var file = new FileEntity
            {
                FileName = "delete.txt",
                Content = "del",
                UploadedBy = "user",
                Size = 5,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Files.Add(file);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.DeleteAsync(file.Id);

            Assert.AreEqual(file.Id, result);
            Assert.IsNull(await _dbContext.Files.FindAsync(file.Id));
        }

        [TestMethod]
        public async Task DeleteAsync_ReturnsNull_WhenNotExists()
        {
            var result = await _repository.DeleteAsync(12345);
            Assert.IsNull(result);
        }
    }
}