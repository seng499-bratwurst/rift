using Moq;
using Rift.Models;
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

        [TestMethod]
        public async Task UpdateLastInteractionTime_ReturnsConversation_WhenFound()
        {
            int conversationId = 17;
            var updatedConversation = new Conversation { Id = conversationId, LastInteraction = DateTime.UtcNow };
            _repositoryMock.Setup(r => r.UpdateLastInteractionTime(conversationId)).ReturnsAsync(updatedConversation);

            var result = await _service.UpdateLastInteractionTime(conversationId);

            Assert.IsNotNull(result);
            Assert.AreEqual(conversationId, result.Id);
            Assert.AreEqual(updatedConversation.LastInteraction, result.LastInteraction);
        }

        [TestMethod]
        public async Task UpdateLastInteractionTime_ReturnsNull_WhenNotFound()
        {
            int conversationId = 404;
            _repositoryMock.Setup(r => r.UpdateLastInteractionTime(conversationId)).ReturnsAsync((Conversation?)null);

            var result = await _service.UpdateLastInteractionTime(conversationId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_ReturnsConversation_WhenFound()
        {
            // Arrange
            int conversationId = 1;
            string title = "New Conversation Title";
            var updatedConversation = new Conversation { Id = conversationId, Title = title };
            _repositoryMock.Setup(r => r.UpdateConversationTitle(conversationId, title)).ReturnsAsync(updatedConversation);

            // Act
            var result = await _service.UpdateConversationTitle(conversationId, title);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(conversationId, result.Id);
            Assert.AreEqual(title, result.Title);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_ReturnsNull_WhenNotFound()
        {
            // Arrange
            int conversationId = 404;
            string title = "New Title";
            _repositoryMock.Setup(r => r.UpdateConversationTitle(conversationId, title)).ReturnsAsync((Conversation?)null);

            // Act
            var result = await _service.UpdateConversationTitle(conversationId, title);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetConversationByIdOnly_ReturnsConversation_WhenFound()
        {
            // Arrange
            int conversationId = 1;
            var conversation = new Conversation { Id = conversationId, Title = "Test Title" };
            _repositoryMock.Setup(r => r.GetConversationByIdOnly(conversationId)).ReturnsAsync(conversation);

            // Act
            var result = await _service.GetConversationByIdOnly(conversationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(conversationId, result.Id);
            Assert.AreEqual("Test Title", result.Title);
        }

        [TestMethod]
        public async Task GetConversationByIdOnly_ReturnsNull_WhenNotFound()
        {
            // Arrange
            int conversationId = 404;
            _repositoryMock.Setup(r => r.GetConversationByIdOnly(conversationId)).ReturnsAsync((Conversation?)null);

            // Act
            var result = await _service.GetConversationByIdOnly(conversationId);

            // Assert
            Assert.IsNull(result);
        }
    }
}