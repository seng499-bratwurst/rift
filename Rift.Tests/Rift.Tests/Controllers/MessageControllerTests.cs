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
        private Mock<IGroup2CompanyService> _group2CompanyServiceMock = null!;
        private Mock<IGeminiTitleService> _geminiTitleServiceMock = null!;
        [TestInitialize]
        public void Setup()
        {
            _messageServiceMock = new Mock<IMessageService>();
            _messageEdgeServiceMock = new Mock<IMessageEdgeService>();
            _conversationServiceMock = new Mock<IConversationService>();
            _ragServiceMock = new Mock<IRAGService>();
            _fileServiceMock = new Mock<IFileService>();
            _messageFilesServiceMock = new Mock<IMessageFilesService>();
            _group2CompanyServiceMock = new Mock<IGroup2CompanyService>();
            _geminiTitleServiceMock = new Mock<IGeminiTitleService>();
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
                _messageFilesServiceMock.Object,
                _group2CompanyServiceMock.Object,
                _geminiTitleServiceMock.Object
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
                _messageFilesServiceMock.Object,
                _group2CompanyServiceMock.Object,
                _geminiTitleServiceMock.Object
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

            _ragServiceMock.Setup(l => l.GenerateResponseAsync("Hello", null, It.IsAny<string>()))
                .ReturnsAsync(("Hi!", new List<string>()));

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

        [TestMethod]
        public async Task CreateMessage_WithGeminiService_CallsTitleGeneration()
        {
            // Arrange
            var userId = "test-user";
            var conversationId = 1;
            var userContent = "What is the temperature?";
            var llmResponse = "The temperature is 15°C.";
            var generatedTitle = "Temperature Inquiry";

            var conversation = new Conversation { Id = conversationId, UserId = userId };
            var promptMessage = new Message { Id = 1, Content = userContent, Role = "user", XCoordinate = 0f, YCoordinate = 0f, CreatedAt = DateTime.UtcNow };
            var responseMessage = new Message { Id = 2, Content = llmResponse, Role = "assistant", XCoordinate = 100f, YCoordinate = 100f, CreatedAt = DateTime.UtcNow };
            var edge = new MessageEdge { Id = 1, SourceMessageId = 1, TargetMessageId = 2 };

            _conversationServiceMock.Setup(c => c.GetOrCreateConversationByUserId(userId, conversationId))
                .ReturnsAsync(conversation);
            _ragServiceMock.Setup(r => r.GenerateResponseAsync(userContent, It.IsAny<List<Message>>(), It.IsAny<string>()))
                .ReturnsAsync((llmResponse, new List<string>()));
            _messageServiceMock.Setup(m => m.CreateMessageAsync(conversationId, null, userContent, "user", It.IsAny<float>(), It.IsAny<float>()))
                .ReturnsAsync(promptMessage);
            _messageServiceMock.Setup(m => m.CreateMessageAsync(conversationId, 1, llmResponse, "assistant", It.IsAny<float>(), It.IsAny<float>()))
                .ReturnsAsync(responseMessage);
            _messageEdgeServiceMock.Setup(e => e.CreateEdgeAsync(It.IsAny<MessageEdge>()))
                .ReturnsAsync(edge);
            _fileServiceMock.Setup(f => f.GetFilesByTitlesAsync(It.IsAny<string[]>()))
                .ReturnsAsync(new List<FileEntityDto>());
            _conversationServiceMock.Setup(c => c.GetConversationByIdOnly(conversationId))
                .ReturnsAsync(conversation);
            _geminiTitleServiceMock.Setup(g => g.GenerateTitleAsync(userContent, llmResponse))
                .ReturnsAsync(generatedTitle);

            var controller = CreateControllerWithUser(userId);
            var request = new MessageController.CreateMessageRequest
            {
                ConversationId = conversationId,
                Content = userContent,
                XCoordinate = 0f,
                YCoordinate = 0f,
                ResponseXCoordinate = 100f,
                ResponseYCoordinate = 100f
            };

            // Act
            var result = await controller.CreateMessage(request);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result);
            _geminiTitleServiceMock.Verify(g => g.GenerateTitleAsync(userContent, llmResponse), Times.Once);
            _conversationServiceMock.Verify(c => c.UpdateConversationTitle(conversationId, generatedTitle), Times.Once);
        }

        [TestMethod]
        public async Task CreateMessage_WithExistingTitle_DoesNotOverrideTitle()
        {
            // Arrange
            var userId = "test-user";
            var conversationId = 1;
            var userContent = "What is the temperature?";
            var llmResponse = "The temperature is 15°C.";

            var conversation = new Conversation { Id = conversationId, UserId = userId, Title = "Existing Title" };
            var promptMessage = new Message { Id = 1, Content = userContent, Role = "user", XCoordinate = 0f, YCoordinate = 0f, CreatedAt = DateTime.UtcNow };
            var responseMessage = new Message { Id = 2, Content = llmResponse, Role = "assistant", XCoordinate = 100f, YCoordinate = 100f, CreatedAt = DateTime.UtcNow };
            var edge = new MessageEdge { Id = 1, SourceMessageId = 1, TargetMessageId = 2 };

            _conversationServiceMock.Setup(c => c.GetOrCreateConversationByUserId(userId, conversationId))
                .ReturnsAsync(conversation);
            _ragServiceMock.Setup(r => r.GenerateResponseAsync(userContent, It.IsAny<List<Message>>(), It.IsAny<string>()))
                .ReturnsAsync((llmResponse, new List<string>()));
            _messageServiceMock.Setup(m => m.CreateMessageAsync(conversationId, null, userContent, "user", It.IsAny<float>(), It.IsAny<float>()))
                .ReturnsAsync(promptMessage);
            _messageServiceMock.Setup(m => m.CreateMessageAsync(conversationId, 1, llmResponse, "assistant", It.IsAny<float>(), It.IsAny<float>()))
                .ReturnsAsync(responseMessage);
            _messageEdgeServiceMock.Setup(e => e.CreateEdgeAsync(It.IsAny<MessageEdge>()))
                .ReturnsAsync(edge);
            _fileServiceMock.Setup(f => f.GetFilesByTitlesAsync(It.IsAny<string[]>()))
                .ReturnsAsync(new List<FileEntityDto>());
            _conversationServiceMock.Setup(c => c.GetConversationByIdOnly(conversationId))
                .ReturnsAsync(conversation);

            var controller = CreateControllerWithUser(userId);
            var request = new MessageController.CreateMessageRequest
            {
                ConversationId = conversationId,
                Content = userContent,
                XCoordinate = 0f,
                YCoordinate = 0f,
                ResponseXCoordinate = 100f,
                ResponseYCoordinate = 100f
            };

            // Act
            var result = await controller.CreateMessage(request);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result);
            _geminiTitleServiceMock.Verify(g => g.GenerateTitleAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _conversationServiceMock.Verify(c => c.UpdateConversationTitle(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task CreateMessage_WithoutGeminiService_UsesDefaultTitle()
        {
            // Arrange
            var userId = "test-user";
            var conversationId = 1;
            var userContent = "What is the temperature?";
            var llmResponse = "The temperature is 15°C.";

            var conversation = new Conversation { Id = conversationId, UserId = userId };
            var promptMessage = new Message { Id = 1, Content = userContent, Role = "user", XCoordinate = 0f, YCoordinate = 0f, CreatedAt = DateTime.UtcNow };
            var responseMessage = new Message { Id = 2, Content = llmResponse, Role = "assistant", XCoordinate = 100f, YCoordinate = 100f, CreatedAt = DateTime.UtcNow };
            var edge = new MessageEdge { Id = 1, SourceMessageId = 1, TargetMessageId = 2 };

            // Create controller without Gemini service
            var controller = new MessageController(
                _messageServiceMock.Object,
                _conversationServiceMock.Object,
                _ragServiceMock.Object,
                _messageEdgeServiceMock.Object,
                _fileServiceMock.Object,
                _messageFilesServiceMock.Object,
                _group2CompanyServiceMock.Object,
                null // No Gemini service
            );

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _conversationServiceMock.Setup(c => c.GetOrCreateConversationByUserId(userId, conversationId))
                .ReturnsAsync(conversation);
            _ragServiceMock.Setup(r => r.GenerateResponseAsync(userContent, It.IsAny<List<Message>>(), It.IsAny<string>()))
                .ReturnsAsync((llmResponse, new List<string>()));
            _messageServiceMock.Setup(m => m.CreateMessageAsync(conversationId, null, userContent, "user", It.IsAny<float>(), It.IsAny<float>()))
                .ReturnsAsync(promptMessage);
            _messageServiceMock.Setup(m => m.CreateMessageAsync(conversationId, 1, llmResponse, "assistant", It.IsAny<float>(), It.IsAny<float>()))
                .ReturnsAsync(responseMessage);
            _messageEdgeServiceMock.Setup(e => e.CreateEdgeAsync(It.IsAny<MessageEdge>()))
                .ReturnsAsync(edge);
            _fileServiceMock.Setup(f => f.GetFilesByTitlesAsync(It.IsAny<string[]>()))
                .ReturnsAsync(new List<FileEntityDto>());
            _conversationServiceMock.Setup(c => c.GetConversationByIdOnly(conversationId))
                .ReturnsAsync(conversation);

            var request = new MessageController.CreateMessageRequest
            {
                ConversationId = conversationId,
                Content = userContent,
                XCoordinate = 0f,
                YCoordinate = 0f,
                ResponseXCoordinate = 100f,
                ResponseYCoordinate = 100f
            };

            // Act
            var result = await controller.CreateMessage(request);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result);
            _conversationServiceMock.Verify(c => c.UpdateConversationTitle(conversationId, "New Conversation"), Times.Once);
        }

        [TestMethod]
        public async Task CreateMessage_GeminiServiceFails_UsesDefaultTitle()
        {
            // Arrange
            var userId = "test-user";
            var conversationId = 1;
            var userContent = "What is the temperature?";
            var llmResponse = "The temperature is 15°C.";

            var conversation = new Conversation { Id = conversationId, UserId = userId };
            var promptMessage = new Message { Id = 1, Content = userContent, Role = "user", XCoordinate = 0f, YCoordinate = 0f, CreatedAt = DateTime.UtcNow };
            var responseMessage = new Message { Id = 2, Content = llmResponse, Role = "assistant", XCoordinate = 100f, YCoordinate = 100f, CreatedAt = DateTime.UtcNow };
            var edge = new MessageEdge { Id = 1, SourceMessageId = 1, TargetMessageId = 2 };

            _conversationServiceMock.Setup(c => c.GetOrCreateConversationByUserId(userId, conversationId))
                .ReturnsAsync(conversation);
            _ragServiceMock.Setup(r => r.GenerateResponseAsync(userContent, It.IsAny<List<Message>>(), It.IsAny<string>()))
                .ReturnsAsync((llmResponse, new List<string>()));
            _messageServiceMock.Setup(m => m.CreateMessageAsync(conversationId, null, userContent, "user", It.IsAny<float>(), It.IsAny<float>()))
                .ReturnsAsync(promptMessage);
            _messageServiceMock.Setup(m => m.CreateMessageAsync(conversationId, 1, llmResponse, "assistant", It.IsAny<float>(), It.IsAny<float>()))
                .ReturnsAsync(responseMessage);
            _messageEdgeServiceMock.Setup(e => e.CreateEdgeAsync(It.IsAny<MessageEdge>()))
                .ReturnsAsync(edge);
            _fileServiceMock.Setup(f => f.GetFilesByTitlesAsync(It.IsAny<string[]>()))
                .ReturnsAsync(new List<FileEntityDto>());
            _conversationServiceMock.Setup(c => c.GetConversationByIdOnly(conversationId))
                .ReturnsAsync(conversation);
            _geminiTitleServiceMock.Setup(g => g.GenerateTitleAsync(userContent, llmResponse))
                .ReturnsAsync("New Conversation"); // Simulate failure

            var controller = CreateControllerWithUser(userId);
            var request = new MessageController.CreateMessageRequest
            {
                ConversationId = conversationId,
                Content = userContent,
                XCoordinate = 0f,
                YCoordinate = 0f,
                ResponseXCoordinate = 100f,
                ResponseYCoordinate = 100f
            };

            // Act
            var result = await controller.CreateMessage(request);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result);
            _geminiTitleServiceMock.Verify(g => g.GenerateTitleAsync(userContent, llmResponse), Times.Once);
            // Should not update title when Gemini fails
            _conversationServiceMock.Verify(c => c.UpdateConversationTitle(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task CreateGuestMessage_WithGeminiService_CallsTitleGeneration()
        {
            // Arrange
            var sessionId = "test-session";
            var userContent = "What is the temperature?";
            var llmResponse = "The temperature is 15°C.";
            var generatedTitle = "Temperature Inquiry";

            var conversation = new Conversation { Id = 1, SessionId = sessionId };
            var promptMessage = new Message { Id = 1, Content = userContent, Role = "user", XCoordinate = 0f, YCoordinate = 0f, CreatedAt = DateTime.UtcNow };
            var responseMessage = new Message { Id = 2, Content = llmResponse, Role = "assistant", XCoordinate = 100f, YCoordinate = 100f, CreatedAt = DateTime.UtcNow };
            var edge = new MessageEdge { Id = 1, SourceMessageId = 1, TargetMessageId = 2 };

            _conversationServiceMock.Setup(c => c.GetConversationsForSessionAsync(sessionId))
                .ReturnsAsync(conversation);
            _ragServiceMock.Setup(r => r.GenerateResponseAsync(userContent, It.IsAny<List<Message>>(), It.IsAny<string>()))
                .ReturnsAsync((llmResponse, new List<string>()));
            _messageServiceMock.Setup(m => m.CreateMessageAsync(1, null, userContent, "user", It.IsAny<float>(), It.IsAny<float>()))
                .ReturnsAsync(promptMessage);
            _messageServiceMock.Setup(m => m.CreateMessageAsync(1, 1, llmResponse, "assistant", It.IsAny<float>(), It.IsAny<float>()))
                .ReturnsAsync(responseMessage);
            _messageEdgeServiceMock.Setup(e => e.CreateEdgeAsync(It.IsAny<MessageEdge>()))
                .ReturnsAsync(edge);
            _fileServiceMock.Setup(f => f.GetFilesByTitlesAsync(It.IsAny<string[]>()))
                .ReturnsAsync(new List<FileEntityDto>());
            _conversationServiceMock.Setup(c => c.GetConversationByIdOnly(1))
                .ReturnsAsync(conversation);
            _geminiTitleServiceMock.Setup(g => g.GenerateTitleAsync(userContent, llmResponse))
                .ReturnsAsync(generatedTitle);

            var controller = CreateControllerWithoutUser();
            var request = new MessageController.CreateMessageRequest
            {
                SessionId = sessionId,
                Content = userContent,
                XCoordinate = 0f,
                YCoordinate = 0f,
                ResponseXCoordinate = 100f,
                ResponseYCoordinate = 100f
            };

            // Act
            var result = await controller.CreateGuestMessage(request);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(result);
            _geminiTitleServiceMock.Verify(g => g.GenerateTitleAsync(userContent, llmResponse), Times.Once);
            _conversationServiceMock.Verify(c => c.UpdateConversationTitle(1, generatedTitle), Times.Once);
        }
    }
}
