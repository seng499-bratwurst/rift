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

        [TestMethod]
        public async Task UpdateConversationTitle_UpdatesTitle_WhenConversationExists()
        {
            // Arrange
            var conversation = new Conversation
            {
                Id = 1,
                UserId = "user1",
                Title = null,
                FirstInteraction = DateTime.UtcNow,
                LastInteraction = DateTime.UtcNow
            };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var newTitle = "Cambridge Bay Temperature Analysis";

            // Act
            var result = await _repository.UpdateConversationTitle(1, newTitle);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(newTitle, result.Title);

            // Verify in database
            var dbConversation = await _dbContext.Conversations.FindAsync(1);
            Assert.IsNotNull(dbConversation);
            Assert.AreEqual(newTitle, dbConversation.Title);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_ReturnsNull_WhenConversationDoesNotExist()
        {
            // Act
            var result = await _repository.UpdateConversationTitle(999, "Non Existent Title");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_UpdatesOnlyTitle_LeavesOtherFieldsUnchanged()
        {
            // Arrange
            var originalTime = DateTime.UtcNow.AddHours(-1);
            var conversation = new Conversation
            {
                Id = 2,
                UserId = "user2",
                Title = "Original Title",
                FirstInteraction = originalTime,
                LastInteraction = originalTime
            };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var newTitle = "Ice Conditions Marine Life Impact";

            // Act
            var result = await _repository.UpdateConversationTitle(2, newTitle);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(newTitle, result.Title);
            Assert.AreEqual("user2", result.UserId);
            Assert.AreEqual(originalTime.ToString(), result.FirstInteraction.ToString());
            Assert.AreEqual(originalTime.ToString(), result.LastInteraction.ToString());
        }

        [TestMethod]
        public async Task UpdateConversationTitle_HandlesEmptyTitle()
        {
            // Arrange
            var conversation = new Conversation
            {
                Id = 3,
                UserId = "user3",
                Title = "Original Title",
                FirstInteraction = DateTime.UtcNow,
                LastInteraction = DateTime.UtcNow
            };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.UpdateConversationTitle(3, "");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Id);
            Assert.AreEqual("", result.Title);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_HandlesNullTitle()
        {
            // Arrange
            var conversation = new Conversation
            {
                Id = 4,
                UserId = "user4",
                Title = "Original Title",
                FirstInteraction = DateTime.UtcNow,
                LastInteraction = DateTime.UtcNow
            };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.UpdateConversationTitle(4, null!);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Id);
            Assert.IsNull(result.Title);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_HandlesLongTitle()
        {
            // Arrange
            var conversation = new Conversation
            {
                Id = 5,
                UserId = "user5",
                Title = null,
                FirstInteraction = DateTime.UtcNow,
                LastInteraction = DateTime.UtcNow
            };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var longTitle = new string('A', 500); // Very long title

            // Act
            var result = await _repository.UpdateConversationTitle(5, longTitle);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Id);
            Assert.AreEqual(longTitle, result.Title);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_HandlesSpecialCharacters()
        {
            // Arrange
            var conversation = new Conversation
            {
                Id = 6,
                UserId = "user6",
                Title = null,
                FirstInteraction = DateTime.UtcNow,
                LastInteraction = DateTime.UtcNow
            };
            _dbContext.Conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();

            var titleWithSpecialChars = "Temperature: 2.5°C & Ice Coverage ~85% @Cambridge Bay";

            // Act
            var result = await _repository.UpdateConversationTitle(6, titleWithSpecialChars);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Id);
            Assert.AreEqual(titleWithSpecialChars, result.Title);
        }
    }
}