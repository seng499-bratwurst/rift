using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rift.Controllers;
using Rift.Services;
using Rift.Models;

namespace Rift.Tests
{
    [TestClass]
    public class MessageEdgeControllerTests
    {
        private Mock<IMessageEdgeService> _messageEdgeServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _messageEdgeServiceMock = new Mock<IMessageEdgeService>();
        }

        private MessageEdgeController CreateControllerWithUser(string userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));
            var controller = new MessageEdgeController(_messageEdgeServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            return controller;
        }

        private MessageEdgeController CreateControllerWithoutUser()
        {
            var controller = new MessageEdgeController(_messageEdgeServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            return controller;
        }

        [TestMethod]
        public async Task CreateEdge_ReturnsBadRequest_WhenServiceReturnsNull()
        {
            _messageEdgeServiceMock.Setup(s => s.CreateEdgeAsync(It.IsAny<MessageEdge>()))
                .ReturnsAsync((MessageEdge)null);

            var controller = CreateControllerWithUser("user1");
            var edge = new MessageEdge { SourceMessageId = 1, TargetMessageId = 2 };

            var result = await controller.CreateEdge(edge);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            var apiResponse = badRequest.Value as ApiResponse<MessageEdge>;
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("Failed to create edge.", apiResponse.Error);
        }

        [TestMethod]
        public async Task CreateEdge_ReturnsOk_WhenServiceReturnsEdge()
        {
            var edge = new MessageEdge { SourceMessageId = 1, TargetMessageId = 2 };
            _messageEdgeServiceMock.Setup(s => s.CreateEdgeAsync(It.IsAny<MessageEdge>()))
                .ReturnsAsync(edge);

            var controller = CreateControllerWithUser("user1");

            var result = await controller.CreateEdge(edge);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<MessageEdge>;
            Assert.IsTrue(apiResponse.Success);
            Assert.AreEqual(edge, apiResponse.Data);
        }

        [TestMethod]
        public async Task DeleteEdge_ReturnsNotFound_WhenServiceReturnsNull()
        {
            _messageEdgeServiceMock.Setup(s => s.DeleteEdgeAsync(1))
                .ReturnsAsync((int?)null);

            var controller = CreateControllerWithUser("user1");

            var result = await controller.DeleteEdge(1);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            var apiResponse = notFound.Value as ApiResponse<int?>;
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("Edge not found.", apiResponse.Error);
        }

        [TestMethod]
        public async Task DeleteEdge_ReturnsOk_WhenServiceReturnsId()
        {
            _messageEdgeServiceMock.Setup(s => s.DeleteEdgeAsync(1))
                .ReturnsAsync(1);

            var controller = CreateControllerWithUser("user1");

            var result = await controller.DeleteEdge(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<object>;
            Assert.IsTrue(apiResponse.Success);

            // Use reflection to get DeletedId from anonymous object
            Assert.IsNotNull(apiResponse.Data);
            var deletedIdProp = apiResponse.Data.GetType().GetProperty("DeletedId");
            Assert.IsNotNull(deletedIdProp, "DeletedId property not found on Data");
            var deletedIdValue = deletedIdProp.GetValue(apiResponse.Data);
            Assert.AreEqual(1, deletedIdValue);
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
        public async Task GetMessages_ReturnsOk_WithEdges()
        {
            var userId = "user1";
            var edges = new List<MessageEdge>
            {
                new MessageEdge { SourceMessageId = 1, TargetMessageId = 2 },
                new MessageEdge { SourceMessageId = 2, TargetMessageId = 3 }
            };
            _messageEdgeServiceMock.Setup(s => s.GetEdgesForConversationAsync(userId, 5))
                .ReturnsAsync(edges);

            var controller = CreateControllerWithUser(userId);

            var result = await controller.GetMessages(5);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<List<MessageEdge>>;
            Assert.IsTrue(apiResponse.Success);
            Assert.AreEqual(2, apiResponse.Data.Count);
        }
    }
}