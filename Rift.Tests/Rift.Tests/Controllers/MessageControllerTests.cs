using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rift.Controllers;
using Rift.Services;
using Rift.LLM;
using Rift.Models;
using System.Text.Json;

namespace Rift.Tests
{
    [TestClass]
    public class MessageControllerTests
    {
        private Mock<IMessageService> _messageServiceMock;
        private Mock<IMessageEdgeService> _messageEdgeServiceMock;
        private Mock<IConversationService> _conversationServiceMock;
        private Mock<IRAGService> _ragServiceMock;




        [TestInitialize]
        public void Setup()
        {
            _messageServiceMock = new Mock<IMessageService>();
            _messageEdgeServiceMock = new Mock<IMessageEdgeService>();
            _conversationServiceMock = new Mock<IConversationService>();
            _ragServiceMock = new Mock<IRAGService>();
        }

        private MessageController CreateControllerWithUser(string userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("ONCApiToken", "token")
            }, "mock"));
            var controller = new MessageController(
                _messageServiceMock.Object,
                _conversationServiceMock.Object,
                _ragServiceMock.Object,
                _messageEdgeServiceMock.Object
                // _rateLimitingServiceMock.Object  // Inject the rate limiting service mock
            );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            return controller;
        }

        private MessageController CreateControllerWithoutUser()
        {
            var controller = new MessageController(
                _messageServiceMock.Object,
                _conversationServiceMock.Object,
                _ragServiceMock.Object,
                _messageEdgeServiceMock.Object
                // _rateLimitingServiceMock.Object  // Inject the rate limiting service mock
            );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            return controller;
        }

        [TestMethod]
        public async Task CreateMessage_ReturnsBadRequest_WhenContentIsEmpty()
        {
            var controller = CreateControllerWithUser("user1");
            var request = new MessageController.CreateMessageRequest { Content = "" };

            var result = await controller.CreateMessage(request);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            var apiResponse = badRequest.Value as ApiResponse<Message>;
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("Message content cannot be empty.", apiResponse.Error);
        }

        [TestMethod]
        public async Task CreateMessage_ReturnsBadRequest_WhenUserIdMissing()
        {
            var controller = CreateControllerWithoutUser();
            var request = new MessageController.CreateMessageRequest { Content = "Hello" };

            var result = await controller.CreateMessage(request);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            var apiResponse = badRequest.Value as ApiResponse<object>;
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("User ID is required for authenticated requests", apiResponse.Error);
        }

        [TestMethod]
        public async Task CreateMessage_ReturnsNotFound_WhenConversationNull()
        {
            _conversationServiceMock
                .Setup(x => x.GetOrCreateConversationByUserId(It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync((Conversation?)null);


            var controller = CreateControllerWithUser("user1");
            var request = new MessageController.CreateMessageRequest { Content = "Hello" };

            var result = await controller.CreateMessage(request);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            var apiResponse = notFound.Value as ApiResponse<object>;
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("Conversation not found.", apiResponse.Error);
        }

        [TestMethod]
        public async Task CreateMessage_ReturnsOk_WithValidData()
        {
            var userId = "user1";
            var conversation = new Conversation { Id = 5, UserId = userId };
            var promptMessage = new Message { Id = 10, Content = "Hello", Role = "user", XCoordinate = 0, YCoordinate = 0 };
            var responseMessage = new Message { Id = 11, Content = "Hi!", Role = "assistant", XCoordinate = 1, YCoordinate = 1 };
            var edge = new MessageEdge { SourceMessageId = 10, TargetMessageId = 11 };

            _conversationServiceMock.Setup(s => s.GetOrCreateConversationByUserId(userId, null))
                .ReturnsAsync(conversation);

            _ragServiceMock.Setup(l => l.GenerateResponseAsync("Hello", null)).ReturnsAsync("Hi!");

            _messageServiceMock.Setup(m => m.CreateMessageAsync(conversation.Id, null, "Hello", "user", 0, 0))
                .ReturnsAsync(promptMessage);
            _messageServiceMock.Setup(m => m.CreateMessageAsync(conversation.Id, promptMessage.Id, "Hi!", "assistant", 0, 0))
                .ReturnsAsync(responseMessage);

            _messageEdgeServiceMock.Setup(e => e.CreateEdgeAsync(It.IsAny<MessageEdge>()))
                .ReturnsAsync(edge);
            _messageEdgeServiceMock.Setup(e => e.CreateMessageEdgesFromSourcesAsync(It.IsAny<int>(), It.IsAny<PartialMessageEdge[]>()))
                .ReturnsAsync(new List<MessageEdge>());

            var controller = CreateControllerWithUser(userId);
            var request = new MessageController.CreateMessageRequest { Content = "Hello" };

            var result = await controller.CreateMessage(request);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<object>;
            Assert.IsTrue(apiResponse.Success);
            Assert.AreEqual(conversation.Id, (int)apiResponse.Data.GetType().GetProperty("ConversationId").GetValue(apiResponse.Data));
        }

        [TestMethod]
        public async Task GetMessages_ReturnsUnauthorized_WhenUserIdMissing()
        {
            var controller = CreateControllerWithoutUser();

            var result = await controller.GetMessages(1);

            var unauthorized = result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorized);
            var apiResponse = unauthorized.Value as ApiResponse<List<Message>>;
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("Unauthorized", apiResponse.Error);
        }

        [TestMethod]
        public async Task GetMessages_ReturnsOk_WithMessages()
        {
            var userId = "user1";
            var messages = new List<Message>
            {
                new Message { Id = 1, Content = "Hi", Role = "user", XCoordinate = 0, YCoordinate = 0 },
                new Message { Id = 2, Content = "Hello", Role = "assistant", XCoordinate = 1, YCoordinate = 1 }
            };
            _messageServiceMock.Setup(m => m.GetMessagesForConversationAsync(userId, 1))
                .ReturnsAsync(messages);

            var controller = CreateControllerWithUser(userId);

            var result = await controller.GetMessages(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<List<Message>>;
            Assert.IsTrue(apiResponse.Success);
            Assert.AreEqual(2, apiResponse.Data.Count);
        }
    }
}