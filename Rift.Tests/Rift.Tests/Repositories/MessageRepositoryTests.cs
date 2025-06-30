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
    public class MessageRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions = null!;
        private ApplicationDbContext _dbContext = null!;
        private MessageRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ApplicationDbContext(_dbOptions);
            _repository = new MessageRepository(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task CreateAsync_AddsAndReturnsMessage()
        {
            var conversation = new Conversation { Id = 1, UserId = "user1", FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var message = new Message
            {
                ConversationId = 1,
                Content = "Hello",
                Role = "user",
                XCoordinate = 0,
                YCoordinate = 0,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _repository.CreateAsync(message);

            Assert.IsNotNull(result);
            Assert.AreEqual("Hello", result.Content);

            var dbMessage = await _dbContext.Messages.FindAsync(result.Id);
            Assert.IsNotNull(dbMessage);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsMessage_WhenExistsAndUserMatches()
        {
            var conversation = new Conversation { Id = 2, UserId = "user2", FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var message = new Message
            {
                ConversationId = 2,
                Content = "Test",
                Role = "assistant",
                XCoordinate = 1,
                YCoordinate = 1,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetByIdAsync("user2", message.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Content);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotMatch()
        {
            var conversation = new Conversation { Id = 3, UserId = "user3", FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var message = new Message
            {
                ConversationId = 3,
                Content = "ShouldNotFind",
                Role = "user",
                XCoordinate = 2,
                YCoordinate = 2,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetByIdAsync("wronguser", message.Id);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesMessage()
        {
            var conversation = new Conversation { Id = 4, UserId = "user4", FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var message = new Message
            {
                ConversationId = 4,
                Content = "Before",
                Role = "user",
                XCoordinate = 3,
                YCoordinate = 3,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            message.Content = "After";
            var result = await _repository.UpdateAsync(message);

            Assert.IsNotNull(result);
            Assert.AreEqual("After", result.Content);

            var dbMessage = await _dbContext.Messages.FindAsync(message.Id);
            Assert.AreEqual("After", dbMessage.Content);
        }

        [TestMethod]
        public async Task GetMessagesByConversationIdAsync_ReturnsMessagesForUserAndConversation()
        {
            var conversation = new Conversation { Id = 5, UserId = "user5", FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var msg1 = new Message
            {
                ConversationId = 5,
                Content = "First",
                Role = "user",
                XCoordinate = 0,
                YCoordinate = 0,
                CreatedAt = DateTime.UtcNow.AddMinutes(-1)
            };
            var msg2 = new Message
            {
                ConversationId = 5,
                Content = "Second",
                Role = "assistant",
                XCoordinate = 1,
                YCoordinate = 1,
                CreatedAt = DateTime.UtcNow
            };
            var msgOther = new Message
            {
                ConversationId = 99,
                Content = "Other",
                Role = "user",
                XCoordinate = 2,
                YCoordinate = 2,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Messages.AddRange(msg1, msg2, msgOther);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetUserConversationMessagesAsync("user5", 5);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("First", result[0].Content);
            Assert.AreEqual("Second", result[1].Content);
        }

        [TestMethod]
        public async Task GetMessagesByConversationIdAsync_ReturnsEmpty_WhenUserDoesNotMatch()
        {
            var conversation = new Conversation { Id = 6, UserId = "user6", FirstInteraction = DateTime.UtcNow, LastInteraction = DateTime.UtcNow };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var msg = new Message
            {
                ConversationId = 6,
                Content = "Nope",
                Role = "user",
                XCoordinate = 0,
                YCoordinate = 0,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Messages.Add(msg);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetUserConversationMessagesAsync("wronguser", 6);

            Assert.AreEqual(0, result.Count);
        }
    }
}