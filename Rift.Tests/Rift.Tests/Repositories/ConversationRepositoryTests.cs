using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;
using Rift.Repositories;

namespace Rift.Tests.Repositories
{
    [TestClass]
    public class ConversationRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions = null!;
        private ApplicationDbContext _dbContext = null!;
        private ConversationRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ApplicationDbContext(_dbOptions);
            _repository = new ConversationRepository(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task GetConversationsByUserIdAsync_ReturnsUserConversations()
        {
            var userId = "user-1";
            _dbContext.Conversations.AddRange(
                new Conversation { Id = 1, UserId = userId, FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow },
                new Conversation { Id = 2, UserId = userId, FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow },
                new Conversation { Id = 3, UserId = "other", FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow }
            );
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetConversationsByUserIdAsync(userId);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(c => c.UserId == userId));
        }

        [TestMethod]
        public async Task GetConversationsBySessionIdAsync_ReturnsConversation()
        {
            var sessionId = "session-123";
            var conversation = new Conversation { Id = 10, SessionId = sessionId, FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetConversationsBySessionIdAsync(sessionId);

            Assert.IsNotNull(result);
            Assert.AreEqual(sessionId, result.SessionId);
        }

        [TestMethod]
        public async Task CreateConversationByUserId_AddsAndReturnsConversation()
        {
            var userId = "user-2";
            var result = await _repository.CreateConversationByUserId(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserId);

            var dbConversation = await _dbContext.Conversations.FindAsync(result.Id);
            Assert.IsNotNull(dbConversation);
        }

        [TestMethod]
        public async Task CreateConversationBySessionId_AddsAndReturnsConversation()
        {
            var sessionId = "session-456";
            var result = await _repository.CreateConversationBySessionId(sessionId);

            Assert.IsNotNull(result);
            Assert.AreEqual(sessionId, result.SessionId);

            var dbConversation = await _dbContext.Conversations.FindAsync(result.Id);
            Assert.IsNotNull(dbConversation);
        }

        [TestMethod]
        public async Task DeleteConversation_RemovesConversationAndMessages()
        {
            var userId = "user-3";
            var conversation = new Conversation { Id = 20, UserId = userId, FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow };
            var message1 = new Message { Id = 1, ConversationId = 20, Content = "A", Role = "user", XCoordinate = 0, YCoordinate = 0, CreatedAt = DateTime.UtcNow };
            var message2 = new Message { Id = 2, ConversationId = 20, Content = "B", Role = "assistant", XCoordinate = 1, YCoordinate = 1, CreatedAt = DateTime.UtcNow };
            _dbContext.Conversations.Add(conversation);
            _dbContext.Messages.AddRange(message1, message2);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.DeleteConversation(userId, 20);

            Assert.IsNotNull(result);
            Assert.AreEqual(20, result.Id);
            Assert.IsNull(await _dbContext.Conversations.FindAsync(20));
            Assert.AreEqual(0, _dbContext.Messages.Count(m => m.ConversationId == 20));
        }

        [TestMethod]
        public async Task DeleteConversation_ReturnsNull_WhenNotFound()
        {
            var result = await _repository.DeleteConversation("missing-user", 999);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetConversationById_ReturnsConversation_WhenExists()
        {
            var userId = "user-4";
            var conversation = new Conversation { Id = 30, UserId = userId, FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetConversationById(userId, 30);

            Assert.IsNotNull(result);
            Assert.AreEqual(30, result.Id);
        }

        [TestMethod]
        public async Task GetConversationById_ReturnsNull_WhenNotExists()
        {
            var result = await _repository.GetConversationById("user-x", 12345);
            Assert.IsNull(result);
        }
    }
}