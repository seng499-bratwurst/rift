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
    }
}