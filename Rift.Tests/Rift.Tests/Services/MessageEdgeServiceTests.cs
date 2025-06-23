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
    public class MessageEdgeServiceTests
    {
        private Mock<IMessageEdgeRepository> _edgeRepositoryMock = null!;
        private MessageEdgeService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _edgeRepositoryMock = new Mock<IMessageEdgeRepository>();
            _service = new MessageEdgeService(_edgeRepositoryMock.Object);
        }

        [TestMethod]
        public async Task CreateEdgeAsync_ReturnsCreatedEdge()
        {
            var edge = new MessageEdge { SourceMessageId = 1, TargetMessageId = 2 };
            _edgeRepositoryMock.Setup(r => r.AddEdgeAsync(edge)).ReturnsAsync(edge);

            var result = await _service.CreateEdgeAsync(edge);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.SourceMessageId);
            Assert.AreEqual(2, result.TargetMessageId);
        }

        [TestMethod]
        public async Task DeleteEdgeAsync_ReturnsId_WhenDeleted()
        {
            _edgeRepositoryMock.Setup(r => r.RemoveEdgeAsync(1)).ReturnsAsync(1);

            var result = await _service.DeleteEdgeAsync(1);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task DeleteEdgeAsync_ReturnsNull_WhenNotFound()
        {
            _edgeRepositoryMock.Setup(r => r.RemoveEdgeAsync(2)).ReturnsAsync((int?)null);

            var result = await _service.DeleteEdgeAsync(2);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CreateMessageEdgesFromSourcesAsync_ReturnsCreatedEdges()
        {
            int targetMessageId = 10;
            var sources = new[]
            {
                new PartialMessageEdge { SourceMessageId = 1, SourceHandle = "A", TargetHandle = "B" },
                new PartialMessageEdge { SourceMessageId = 2, SourceHandle = "C", TargetHandle = "D" }
            };
            var expectedEdges = new List<MessageEdge>
            {
                new MessageEdge { SourceMessageId = 1, TargetMessageId = 10, SourceHandle = "A", TargetHandle = "B" },
                new MessageEdge { SourceMessageId = 2, TargetMessageId = 10, SourceHandle = "C", TargetHandle = "D" }
            };
            _edgeRepositoryMock.Setup(r => r.AddEdgesAsync(It.IsAny<MessageEdge[]>()))
                .ReturnsAsync(expectedEdges);

            var result = await _service.CreateMessageEdgesFromSourcesAsync(targetMessageId, sources);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].SourceMessageId);
            Assert.AreEqual(10, result[0].TargetMessageId);
            Assert.AreEqual("A", result[0].SourceHandle);
            Assert.AreEqual("B", result[0].TargetHandle);
        }

        [TestMethod]
        public async Task GetEdgesForConversationAsync_ReturnsEdges()
        {
            string userId = "user1";
            int conversationId = 5;
            var edges = new List<MessageEdge>
            {
                new MessageEdge { SourceMessageId = 1, TargetMessageId = 2 },
                new MessageEdge { SourceMessageId = 3, TargetMessageId = 4 }
            };
            _edgeRepositoryMock.Setup(r => r.GetEdgesForConversationAsync(userId, conversationId))
                .ReturnsAsync(edges);

            var result = await _service.GetEdgesForConversationAsync(userId, conversationId);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].SourceMessageId);
            Assert.AreEqual(2, result[0].TargetMessageId);
        }
    }
}