using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Rift.Models;
using Rift.Controllers;

namespace Rift.Tests
{
    [TestClass]
    public class ConversationControllerTests
    {
        private Mock<IConversationService> _conversationServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _conversationServiceMock = new Mock<IConversationService>();
        }

        private ConversationController CreateControllerWithUser(string userId)
        {
            var controller = new ConversationController(_conversationServiceMock.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = user }
            };
            return controller;
        }

        private ConversationController CreateControllerWithoutUser()
        {
            var controller = new ConversationController(_conversationServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext()
            };
            return controller;
        }

        [TestMethod]
        public async Task GetConversations_ReturnsUnauthorized_WhenUserIdMissing()
        {
            var controller = CreateControllerWithoutUser();

            var result = await controller.GetConversations();

            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task GetConversations_ReturnsOk_WithConversations()
        {
            var userId = "user-123";
            var conversations = new List<Conversation>
            {
                new Conversation { Id = 1, UserId = userId },
                new Conversation { Id = 2, UserId = userId }
            };
            _conversationServiceMock.Setup(s => s.GetConversationsForUserAsync(userId))
                .ReturnsAsync(conversations);

            var controller = CreateControllerWithUser(userId);

            var result = await controller.GetConversations();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<IEnumerable<Conversation>>;
            Assert.IsTrue(apiResponse.Success);
            Assert.AreEqual(2, ((List<Conversation>)apiResponse.Data).Count);
        }

        [TestMethod]
        public async Task DeleteConversation_ReturnsUnauthorized_WhenUserIdMissing()
        {
            var controller = CreateControllerWithoutUser();

            var result = await controller.DeleteConversation(1);

            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task DeleteConversation_ReturnsNotFound_WhenConversationNull()
        {
            var userId = "user-123";
            _conversationServiceMock.Setup(s => s.DeleteConversation(userId, 1))
                .ReturnsAsync((Conversation)null);

            var controller = CreateControllerWithUser(userId);

            var result = await controller.DeleteConversation(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeleteConversation_ReturnsOk_WhenConversationDeleted()
        {
            var userId = "user-123";
            var conversation = new Conversation { Id = 1, UserId = userId };
            _conversationServiceMock.Setup(s => s.DeleteConversation(userId, 1))
                .ReturnsAsync(conversation);

            var controller = CreateControllerWithUser(userId);

            var result = await controller.DeleteConversation(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<Conversation>;
            Assert.IsTrue(apiResponse.Success);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_ReturnsUnauthorized_WhenUserIdMissing()
        {
            var controller = CreateControllerWithoutUser();
            var request = new ConversationController.UpdateTitleRequest { Title = "New Title" };

            var result = await controller.UpdateConversationTitle(1, request);

            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task UpdateConversationTitle_ReturnsBadRequest_WhenTitleIsEmpty()
        {
            var userId = "user-123";
            var controller = CreateControllerWithUser(userId);
            var request = new ConversationController.UpdateTitleRequest { Title = "" };

            var result = await controller.UpdateConversationTitle(1, request);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var apiResponse = badRequestResult.Value as ApiResponse<Conversation>;
            Assert.IsNotNull(apiResponse);
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("Title cannot be empty", apiResponse.Error);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_ReturnsBadRequest_WhenTitleIsWhitespace()
        {
            var userId = "user-123";
            var controller = CreateControllerWithUser(userId);
            var request = new ConversationController.UpdateTitleRequest { Title = "   " };

            var result = await controller.UpdateConversationTitle(1, request);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var apiResponse = badRequestResult.Value as ApiResponse<Conversation>;
            Assert.IsNotNull(apiResponse);
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("Title cannot be empty", apiResponse.Error);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_ReturnsNotFound_WhenConversationNotOwnedByUser()
        {
            var userId = "user-123";
            var conversationId = 1;
            _conversationServiceMock.Setup(s => s.GetConversationById(userId, conversationId))
                .ReturnsAsync((Conversation?)null);

            var controller = CreateControllerWithUser(userId);
            var request = new ConversationController.UpdateTitleRequest { Title = "New Title" };

            var result = await controller.UpdateConversationTitle(conversationId, request);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            var apiResponse = notFoundResult.Value as ApiResponse<Conversation>;
            Assert.IsNotNull(apiResponse);
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("Conversation not found", apiResponse.Error);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_ReturnsBadRequest_WhenUpdateFails()
        {
            var userId = "user-123";
            var conversationId = 1;
            var existingConversation = new Conversation { Id = conversationId, UserId = userId, Title = "Old Title" };
            var newTitle = "New Title";

            _conversationServiceMock.Setup(s => s.GetConversationById(userId, conversationId))
                .ReturnsAsync(existingConversation);
            _conversationServiceMock.Setup(s => s.UpdateConversationTitle(conversationId, newTitle))
                .ReturnsAsync((Conversation?)null);

            var controller = CreateControllerWithUser(userId);
            var request = new ConversationController.UpdateTitleRequest { Title = newTitle };

            var result = await controller.UpdateConversationTitle(conversationId, request);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var apiResponse = badRequestResult.Value as ApiResponse<Conversation>;
            Assert.IsNotNull(apiResponse);
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("Failed to update conversation title", apiResponse.Error);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_ReturnsOk_WhenUpdateSucceeds()
        {
            var userId = "user-123";
            var conversationId = 1;
            var existingConversation = new Conversation { Id = conversationId, UserId = userId, Title = "Old Title" };
            var newTitle = "New Updated Title";
            var updatedConversation = new Conversation { Id = conversationId, UserId = userId, Title = newTitle };

            _conversationServiceMock.Setup(s => s.GetConversationById(userId, conversationId))
                .ReturnsAsync(existingConversation);
            _conversationServiceMock.Setup(s => s.UpdateConversationTitle(conversationId, newTitle))
                .ReturnsAsync(updatedConversation);

            var controller = CreateControllerWithUser(userId);
            var request = new ConversationController.UpdateTitleRequest { Title = newTitle };

            var result = await controller.UpdateConversationTitle(conversationId, request);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<Conversation>;
            Assert.IsNotNull(apiResponse);
            Assert.IsTrue(apiResponse.Success);
            Assert.IsNull(apiResponse.Error);
            Assert.IsNotNull(apiResponse.Data);
            Assert.AreEqual(conversationId, apiResponse.Data.Id);
            Assert.AreEqual(newTitle, apiResponse.Data.Title);
        }

        [TestMethod]
        public async Task UpdateConversationTitle_VerifiesServiceCalls_WithCorrectParameters()
        {
            var userId = "user-123";
            var conversationId = 42;
            var existingConversation = new Conversation { Id = conversationId, UserId = userId, Title = "Old Title" };
            var newTitle = "Verification Test Title";
            var updatedConversation = new Conversation { Id = conversationId, UserId = userId, Title = newTitle };

            _conversationServiceMock.Setup(s => s.GetConversationById(userId, conversationId))
                .ReturnsAsync(existingConversation);
            _conversationServiceMock.Setup(s => s.UpdateConversationTitle(conversationId, newTitle))
                .ReturnsAsync(updatedConversation);

            var controller = CreateControllerWithUser(userId);
            var request = new ConversationController.UpdateTitleRequest { Title = newTitle };

            await controller.UpdateConversationTitle(conversationId, request);

            // Verify the ownership check was performed
            _conversationServiceMock.Verify(s => s.GetConversationById(userId, conversationId), Times.Once);
            // Verify the update was performed with correct parameters
            _conversationServiceMock.Verify(s => s.UpdateConversationTitle(conversationId, newTitle), Times.Once);
        }
    }
}