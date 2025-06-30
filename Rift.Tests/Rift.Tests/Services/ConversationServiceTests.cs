using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rift.Models;
using Rift.Repositories;
using Rift.Services;

namespace Rift.Tests.Services
{
    [TestClass]
    public class ConversationServiceTests
    {
        private Mock<IConversationRepository> _repositoryMock = null!;
        private ConversationService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IConversationRepository>();
            _service = new ConversationService(_repositoryMock.Object);
        }

        [TestMethod]
        public async Task GetConversationsForUserAsync_ReturnsConversations()
        {
            var userId = "user1";
            var conversations = new List<Conversation> { new Conversation { Id = 1 }, new Conversation { Id = 2 } };
            _repositoryMock.Setup(r => r.GetConversationsByUserIdAsync(userId)).ReturnsAsync(conversations);

            var result = await _service.GetConversationsForUserAsync(userId);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
        }

        [TestMethod]
        public async Task GetConversationsForSessionAsync_ReturnsConversation()
        {
            var sessionId = "session1";
            var conversation = new Conversation { Id = 1 };
            _repositoryMock.Setup(r => r.GetConversationsBySessionIdAsync(sessionId)).ReturnsAsync(conversation);

            var result = await _service.GetConversationsForSessionAsync(sessionId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task CreateConversationByUserId_ReturnsConversation()
        {
            var userId = "user1";
            var conversation = new Conversation { Id = 1 };
            _repositoryMock.Setup(r => r.CreateConversationByUserId(userId)).ReturnsAsync(conversation);

            var result = await _service.CreateConversationByUserId(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task CreateConversationBySessionId_ReturnsConversation()
        {
            var sessionId = "session1";
            var conversation = new Conversation { Id = 1 };
            _repositoryMock.Setup(r => r.CreateConversationBySessionId(sessionId)).ReturnsAsync(conversation);

            var result = await _service.CreateConversationBySessionId(sessionId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task DeleteConversation_ReturnsConversation_WhenFound()
        {
            var userId = "user1";
            int conversationId = 1;
            var conversation = new Conversation { Id = conversationId };
            _repositoryMock.Setup(r => r.DeleteConversation(userId, conversationId)).ReturnsAsync(conversation);

            var result = await _service.DeleteConversation(userId, conversationId);

            Assert.IsNotNull(result);
            Assert.AreEqual(conversationId, result.Id);
        }

        [TestMethod]
        public async Task DeleteConversation_ReturnsNull_WhenNotFound()
        {
            var userId = "user1";
            int conversationId = 1;
            _repositoryMock.Setup(r => r.DeleteConversation(userId, conversationId)).ReturnsAsync((Conversation?)null);

            var result = await _service.DeleteConversation(userId, conversationId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetConversationById_ReturnsConversation_WhenFound()
        {
            var userId = "user1";
            int conversationId = 1;
            var conversation = new Conversation { Id = conversationId };
            _repositoryMock.Setup(r => r.GetConversationById(userId, conversationId)).ReturnsAsync(conversation);

            var result = await _service.GetConversationById(userId, conversationId);

            Assert.IsNotNull(result);
            Assert.AreEqual(conversationId, result.Id);
        }

        [TestMethod]
        public async Task GetConversationById_ReturnsNull_WhenNotFound()
        {
            var userId = "user1";
            int conversationId = 1;
            _repositoryMock.Setup(r => r.GetConversationById(userId, conversationId)).ReturnsAsync((Conversation?)null);

            var result = await _service.GetConversationById(userId, conversationId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetOrCreateConversationByUserId_Creates_WhenConversationIdIsNull()
        {
            var userId = "user1";
            Conversation conversation = new Conversation { Id = 42 };
            _repositoryMock.Setup(r => r.CreateConversationByUserId(userId)).ReturnsAsync(conversation);

            var result = await _service.GetOrCreateConversationByUserId(userId, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(42, result.Id);
        }

        [TestMethod]
        public async Task GetOrCreateConversationByUserId_Gets_WhenConversationIdIsNotNull()
        {
            var userId = "user1";
            int conversationId = 99;
            Conversation conversation = new Conversation { Id = conversationId };
            _repositoryMock.Setup(r => r.GetConversationById(userId, conversationId)).ReturnsAsync(conversation);

            var result = await _service.GetOrCreateConversationByUserId(userId, conversationId);

            Assert.IsNotNull(result);
            Assert.AreEqual(conversationId, result.Id);
        }
    }
}