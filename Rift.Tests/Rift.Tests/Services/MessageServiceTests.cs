using System;
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
    public class MessageServiceTests
    {
        private Mock<IMessageRepository> _messageRepositoryMock = null!;
        private Mock<IMessageFilesRepository> _messageFilesRepositoryMock = null!;
        private Mock<IFileRepository> _fileRepositoryMock = null!;
        private MessageService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();
            _messageFilesRepositoryMock = new Mock<IMessageFilesRepository>();
            _fileRepositoryMock = new Mock<IFileRepository>();
            _service = new MessageService(_messageRepositoryMock.Object, _messageFilesRepositoryMock.Object, _fileRepositoryMock.Object);
        }

        [TestMethod]
        public async Task CreateMessageAsync_ReturnsCreatedMessage()
        {
            var message = new Message
            {
                ConversationId = 1,
                PromptMessageId = 2,
                Content = "Hello",
                Role = "user",
                XCoordinate = 1.1f,
                YCoordinate = 2.2f,
                CreatedAt = DateTime.UtcNow
            };

            _messageRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Message>()))
                .ReturnsAsync((Message m) => m);

            var result = await _service.CreateMessageAsync(
                1, 2, "Hello", "user", 1.1f, 2.2f);

            Assert.IsNotNull(result);
            Assert.AreEqual("Hello", result.Content);
            Assert.AreEqual("user", result.Role);
            Assert.AreEqual(1.1f, result.XCoordinate);
            Assert.AreEqual(2.2f, result.YCoordinate);
            Assert.AreEqual(1, result.ConversationId);
            Assert.AreEqual(2, result.PromptMessageId);
        }

        [TestMethod]
        public async Task GetMessagesForConversationAsync_ReturnsMessages()
        {
            var userId = "user1";
            int conversationId = 5;
            var messages = new List<Message>
            {
                new Message { Id = 1, Content = "A", XCoordinate = 0f, YCoordinate = 0f },
                new Message { Id = 2, Content = "B", XCoordinate = 0f, YCoordinate = 0f }
            };
            _messageRepositoryMock.Setup(r => r.GetUserConversationMessagesAsync(userId, conversationId))
                .ReturnsAsync(messages);

            var result = await _service.GetMessagesForConversationAsync(userId, conversationId);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("A", result[0].Content);
            Assert.AreEqual("B", result[1].Content);
        }

        [TestMethod]
        public async Task UpdateMessageAsync_UpdatesCoordinates_WhenMessageExists()
        {
            var userId = "user1";
            int messageId = 10;
            var message = new Message
            {
                Id = messageId,
                XCoordinate = 0f,
                YCoordinate = 0f
            };
            _messageRepositoryMock.Setup(r => r.GetByIdAsync(userId, messageId))
                .ReturnsAsync(message);
            _messageRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Message>()))
                .ReturnsAsync((Message m) => m);

            var result = await _service.UpdateMessageAsync(messageId, userId, 3.3f, 4.4f);

            Assert.IsNotNull(result);
            Assert.AreEqual(3.3f, result.XCoordinate);
            Assert.AreEqual(4.4f, result.YCoordinate);
        }

        [TestMethod]
        public async Task UpdateMessageAsync_ReturnsNull_WhenMessageNotFound()
        {
            var userId = "user1";
            int messageId = 10;
            _messageRepositoryMock.Setup(r => r.GetByIdAsync(userId, messageId))
                .ReturnsAsync((Message?)null);

            var result = await _service.UpdateMessageAsync(messageId, userId, 1.1f, 2.2f);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateMessageFeedbackAsync_UpdatesIsHelpful_WhenMessageExists()
        {
            var userId = "user1";
            int messageId = 10;
            var message = new Message
            {
                Id = messageId,
                IsHelpful = null,
                XCoordinate = 0f,
                YCoordinate = 0f
            };
            _messageRepositoryMock.Setup(r => r.GetByIdAsync(userId, messageId))
                .ReturnsAsync(message);
            _messageRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Message>()))
                .ReturnsAsync((Message m) => m);

            var result = await _service.UpdateMessageFeedbackAsync(userId, messageId, true);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsHelpful);
        }

        [TestMethod]
        public async Task UpdateMessageFeedbackAsync_UpdatesIsHelpfulToFalse_WhenMessageExists()
        {
            var userId = "user1";
            int messageId = 10;
            var message = new Message
            {
                Id = messageId,
                IsHelpful = true,
                XCoordinate = 0f,
                YCoordinate = 0f
            };
            _messageRepositoryMock.Setup(r => r.GetByIdAsync(userId, messageId))
                .ReturnsAsync(message);
            _messageRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Message>()))
                .ReturnsAsync((Message m) => m);

            var result = await _service.UpdateMessageFeedbackAsync(userId, messageId, false);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsHelpful);
        }

        [TestMethod]
        public async Task UpdateMessageFeedbackAsync_ReturnsNull_WhenMessageNotFound()
        {
            var userId = "user1";
            int messageId = 10;
            _messageRepositoryMock.Setup(r => r.GetByIdAsync(userId, messageId))
                .ReturnsAsync((Message?)null);

            var result = await _service.UpdateMessageFeedbackAsync(userId, messageId, true);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateMessageFeedbackAsync_VerifiesRepositoryCall_WhenMessageExists()
        {
            var userId = "user1";
            int messageId = 10;
            var message = new Message
            {
                Id = messageId,
                IsHelpful = null,
                XCoordinate = 0f,
                YCoordinate = 0f
            };
            _messageRepositoryMock.Setup(r => r.GetByIdAsync(userId, messageId))
                .ReturnsAsync(message);
            _messageRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Message>()))
                .ReturnsAsync((Message m) => m);

            await _service.UpdateMessageFeedbackAsync(userId, messageId, true);

            _messageRepositoryMock.Verify(r => r.GetByIdAsync(userId, messageId), Times.Once);
            _messageRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Message>(m => m.Id == messageId && m.IsHelpful == true)), Times.Once);
        }
    }
}
