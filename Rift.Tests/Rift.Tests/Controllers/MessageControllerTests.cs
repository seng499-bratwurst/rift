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
        private Mock<IFileService> _fileServiceMock;
        private Mock<IMessageFilesService> _messageFilesServiceMock;
        [TestInitialize]
        public void Setup()
        {
            _messageServiceMock = new Mock<IMessageService>();
            _messageEdgeServiceMock = new Mock<IMessageEdgeService>();
            _conversationServiceMock = new Mock<IConversationService>();
            _ragServiceMock = new Mock<IRAGService>();
            _fileServiceMock = new Mock<IFileService>();
            _messageFilesServiceMock = new Mock<IMessageFilesService>();
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
                _messageEdgeServiceMock.Object,
                _fileServiceMock.Object,
                _messageFilesServiceMock.Object
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
                _messageEdgeServiceMock.Object,
                _fileServiceMock.Object,
                _messageFilesServiceMock.Object
            );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            return controller;
        }

        private static Dictionary<string, object> DeserializeApiResponseData(ApiResponse<object> apiResponse)
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(
                JsonSerializer.Serialize(apiResponse.Data))!;
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

        // [TestMethod]
        // public async Task CreateMessage_ReturnsOk_WithValidData()
        // {
        //     var userId = "user1";
        //     var conversation = new Conversation { Id = 5, UserId = userId };
        //     var promptMessage = new Message { Id = 10, Content = "Hello", Role = "user", XCoordinate = 0, YCoordinate = 0 };
        //     var responseMessage = new Message { Id = 11, Content = "Hi!", Role = "assistant", XCoordinate = 1, YCoordinate = 1 };
        //     var edge = new MessageEdge { SourceMessageId = 10, TargetMessageId = 11 };

        //     _conversationServiceMock.Setup(s => s.GetOrCreateConversationByUserId(userId, null))
        //         .ReturnsAsync(conversation);

        //     _messageServiceMock.Setup(m => m.GetMessagesForConversationAsync(userId, conversation.Id))
        //         .ReturnsAsync(new List<Message>());

        //     _ragServiceMock.Setup(l => l.GenerateResponseAsync("Hello", It.IsAny<List<Message>>())).ReturnsAsync("Hi!");

        //     _messageServiceMock.Setup(m => m.CreateMessageAsync(conversation.Id, null, "Hello", "user", 0, 0))
        //         .ReturnsAsync(promptMessage);
        //     _messageServiceMock.Setup(m => m.CreateMessageAsync(conversation.Id, promptMessage.Id, "Hi!", "assistant", 0, 0))
        //         .ReturnsAsync(responseMessage);

        //     _messageEdgeServiceMock.Setup(e => e.CreateEdgeAsync(It.IsAny<MessageEdge>()))
        //         .ReturnsAsync(edge);
        //     _messageEdgeServiceMock.Setup(e => e.CreateMessageEdgesFromSourcesAsync(It.IsAny<int>(), It.IsAny<PartialMessageEdge[]>()))
        //         .ReturnsAsync(new List<MessageEdge>());

        //     _conversationServiceMock.Setup(c => c.UpdateLastInteractionTime(It.IsAny<int>()))
        //         .ReturnsAsync(conversation);

        //     var controller = CreateControllerWithUser(userId);

        //     // Create a real HttpContext with a memory stream for the response body
        //     var httpContext = new DefaultHttpContext();
        //     httpContext.Response.Body = new MemoryStream();
        //     httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        //     {
        //         new Claim(ClaimTypes.NameIdentifier, userId),
        //         new Claim("ONCApiToken", "token")
        //     }, "mock"));

        //     controller.ControllerContext.HttpContext = httpContext;

        //     var request = new MessageController.CreateMessageRequest { Content = "Hello" };

        //     var result = await controller.CreateMessage(request);

        //     // Since streaming is enabled by default, expect EmptyResult
        //     var emptyResult = result as EmptyResult;
        //     Assert.IsNotNull(emptyResult, "Expected EmptyResult due to streaming being enabled");

        //     // Verify that the services were called correctly
        //     _conversationServiceMock.Verify(s => s.GetOrCreateConversationByUserId(userId, null), Times.Once);
        //     _ragServiceMock.Verify(l => l.GenerateResponseAsync("Hello", It.IsAny<List<Message>>()), Times.Once);
        //     _messageServiceMock.Verify(m => m.CreateMessageAsync(conversation.Id, null, "Hello", "user", 0, 0), Times.Once);
        //     _messageServiceMock.Verify(m => m.CreateMessageAsync(conversation.Id, promptMessage.Id, "Hi!", "assistant", 0, 0), Times.Once);
        //     _messageEdgeServiceMock.Verify(e => e.CreateEdgeAsync(It.IsAny<MessageEdge>()), Times.Once);
        //     _conversationServiceMock.Verify(c => c.UpdateLastInteractionTime(conversation.Id), Times.Once);
        // }

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

        [TestMethod]
        public async Task UpdateMessageFeedback_ReturnsOk_WhenMessageExistsAndFeedbackIsTrue()
        {
            var userId = "user1";
            int messageId = 123;
            var updatedMessage = new Message 
            { 
                Id = messageId, 
                IsHelpful = true,
                XCoordinate = 0f,
                YCoordinate = 0f
            };
            
            _messageServiceMock.Setup(m => m.UpdateMessageFeedbackAsync(userId, messageId, true))
                .ReturnsAsync(updatedMessage);

            var controller = CreateControllerWithUser(userId);
            var request = new MessageController.UpdateFeedbackRequest { IsHelpful = true };

            var result = await controller.UpdateMessageFeedback(messageId, request);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<object>;
            Assert.IsNotNull(apiResponse);
            Assert.IsTrue(apiResponse!.Success);
            Assert.IsNull(apiResponse.Error);
            
            // Verify the response data
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(
                JsonSerializer.Serialize(apiResponse.Data));
            Assert.IsNotNull(data);
            Assert.AreEqual(messageId, ((JsonElement)data!["Id"]).GetInt32());
            Assert.AreEqual(true, ((JsonElement)data["IsHelpful"]).GetBoolean());
        }

        [TestMethod]
        public async Task UpdateMessageFeedback_ReturnsOk_WhenMessageExistsAndFeedbackIsFalse()
        {
            var userId = "user1";
            int messageId = 123;
            var updatedMessage = new Message 
            { 
                Id = messageId, 
                IsHelpful = false,
                XCoordinate = 0f,
                YCoordinate = 0f
            };
            
            _messageServiceMock.Setup(m => m.UpdateMessageFeedbackAsync(userId, messageId, false))
                .ReturnsAsync(updatedMessage);

            var controller = CreateControllerWithUser(userId);
            var request = new MessageController.UpdateFeedbackRequest { IsHelpful = false };

            var result = await controller.UpdateMessageFeedback(messageId, request);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<object>;
            Assert.IsNotNull(apiResponse);
            Assert.IsTrue(apiResponse!.Success);
            Assert.IsNull(apiResponse.Error);
            
            // Verify the response data
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(
                JsonSerializer.Serialize(apiResponse.Data));
            Assert.IsNotNull(data);
            Assert.AreEqual(messageId, ((JsonElement)data!["Id"]).GetInt32());
            Assert.AreEqual(false, ((JsonElement)data["IsHelpful"]).GetBoolean());
        }

        [TestMethod]
        public async Task UpdateMessageFeedback_ReturnsUnauthorized_WhenUserNotAuthenticated()
        {
            var controller = CreateControllerWithoutUser();
            var request = new MessageController.UpdateFeedbackRequest { IsHelpful = true };

            var result = await controller.UpdateMessageFeedback(123, request);

            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            var apiResponse = unauthorizedResult.Value as ApiResponse<object>;
            Assert.IsNotNull(apiResponse);
            Assert.IsFalse(apiResponse!.Success);
            Assert.AreEqual("Unauthorized", apiResponse.Error);
        }

        [TestMethod]
        public async Task UpdateMessageFeedback_ReturnsNotFound_WhenMessageNotFound()
        {
            var userId = "user1";
            int messageId = 999;
            
            _messageServiceMock.Setup(m => m.UpdateMessageFeedbackAsync(userId, messageId, true))
                .ReturnsAsync((Message?)null);

            var controller = CreateControllerWithUser(userId);
            var request = new MessageController.UpdateFeedbackRequest { IsHelpful = true };

            var result = await controller.UpdateMessageFeedback(messageId, request);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            var apiResponse = notFoundResult.Value as ApiResponse<object>;
            Assert.IsNotNull(apiResponse);
            Assert.IsFalse(apiResponse!.Success);
            Assert.AreEqual("Message not found or permission denied.", apiResponse.Error);
        }

        [TestMethod]
        public async Task UpdateMessageFeedback_VerifiesServiceCall_WithCorrectParameters()
        {
            var userId = "user1";
            int messageId = 123;
            var updatedMessage = new Message 
            { 
                Id = messageId, 
                IsHelpful = true,
                XCoordinate = 0f,
                YCoordinate = 0f
            };
            
            _messageServiceMock.Setup(m => m.UpdateMessageFeedbackAsync(userId, messageId, true))
                .ReturnsAsync(updatedMessage);

            var controller = CreateControllerWithUser(userId);
            var request = new MessageController.UpdateFeedbackRequest { IsHelpful = true };

            await controller.UpdateMessageFeedback(messageId, request);

            _messageServiceMock.Verify(m => m.UpdateMessageFeedbackAsync(userId, messageId, true), Times.Once);
        }

        [TestMethod]
        public async Task UpdateMessageFeedback_VerifiesServiceCall_WithFalseParameter()
        {
            var userId = "user1";
            int messageId = 123;
            var updatedMessage = new Message 
            { 
                Id = messageId, 
                IsHelpful = false,
                XCoordinate = 0f,
                YCoordinate = 0f
            };
            
            _messageServiceMock.Setup(m => m.UpdateMessageFeedbackAsync(userId, messageId, false))
                .ReturnsAsync(updatedMessage);

            var controller = CreateControllerWithUser(userId);
            var request = new MessageController.UpdateFeedbackRequest { IsHelpful = false };

            await controller.UpdateMessageFeedback(messageId, request);

            _messageServiceMock.Verify(m => m.UpdateMessageFeedbackAsync(userId, messageId, false), Times.Once);
        }

        [TestMethod]
        public async Task UpdateMessageFeedback_HandlesEdgeCase_WhenFeedbackChangesFromFalseToTrue()
        {
            var userId = "user1";
            int messageId = 123;
            var updatedMessage = new Message 
            { 
                Id = messageId, 
                IsHelpful = true, // Changed from false to true
                XCoordinate = 0f,
                YCoordinate = 0f
            };
            
            _messageServiceMock.Setup(m => m.UpdateMessageFeedbackAsync(userId, messageId, true))
                .ReturnsAsync(updatedMessage);

            var controller = CreateControllerWithUser(userId);
            var request = new MessageController.UpdateFeedbackRequest { IsHelpful = true };

            var result = await controller.UpdateMessageFeedback(messageId, request);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<object>;
            Assert.IsNotNull(apiResponse);
            Assert.IsTrue(apiResponse!.Success);
            
            // Verify the response data
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(
                JsonSerializer.Serialize(apiResponse.Data));
            Assert.IsNotNull(data);
            Assert.AreEqual(true, ((JsonElement)data!["IsHelpful"]).GetBoolean());
        }

        [TestMethod]
        public async Task UpdateMessageFeedback_HandlesEdgeCase_WhenFeedbackChangesFromTrueToFalse()
        {
            var userId = "user1";
            int messageId = 123;
            var updatedMessage = new Message 
            { 
                Id = messageId, 
                IsHelpful = false, // Changed from true to false
                XCoordinate = 0f,
                YCoordinate = 0f
            };
            
            _messageServiceMock.Setup(m => m.UpdateMessageFeedbackAsync(userId, messageId, false))
                .ReturnsAsync(updatedMessage);

            var controller = CreateControllerWithUser(userId);
            var request = new MessageController.UpdateFeedbackRequest { IsHelpful = false };

            var result = await controller.UpdateMessageFeedback(messageId, request);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<object>;
            Assert.IsNotNull(apiResponse);
            Assert.IsTrue(apiResponse!.Success);
            
            // Verify the response data
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(
                JsonSerializer.Serialize(apiResponse.Data));
            Assert.IsNotNull(data);
            Assert.AreEqual(false, ((JsonElement)data!["IsHelpful"]).GetBoolean());
        }

        [TestMethod]
        public async Task UpdateMessageFeedback_HandlesMultipleUpdatesCorrectly()
        {
            var userId = "user1";
            int messageId = 123;
            var controller = CreateControllerWithUser(userId);

            // First update: set to true
            var updatedMessage1 = new Message { Id = messageId, IsHelpful = true, XCoordinate = 0f, YCoordinate = 0f };
            _messageServiceMock.Setup(m => m.UpdateMessageFeedbackAsync(userId, messageId, true))
                .ReturnsAsync(updatedMessage1);

            var request1 = new MessageController.UpdateFeedbackRequest { IsHelpful = true };
            var result1 = await controller.UpdateMessageFeedback(messageId, request1);
            var okResult1 = result1 as OkObjectResult;
            Assert.IsNotNull(okResult1);

            // Second update: set to false
            var updatedMessage2 = new Message { Id = messageId, IsHelpful = false, XCoordinate = 0f, YCoordinate = 0f };
            _messageServiceMock.Setup(m => m.UpdateMessageFeedbackAsync(userId, messageId, false))
                .ReturnsAsync(updatedMessage2);

            var request2 = new MessageController.UpdateFeedbackRequest { IsHelpful = false };
            var result2 = await controller.UpdateMessageFeedback(messageId, request2);
            var okResult2 = result2 as OkObjectResult;
            Assert.IsNotNull(okResult2);

            // Verify both calls were made
            _messageServiceMock.Verify(m => m.UpdateMessageFeedbackAsync(userId, messageId, true), Times.Once);
            _messageServiceMock.Verify(m => m.UpdateMessageFeedbackAsync(userId, messageId, false), Times.Once);
        }
    }
}