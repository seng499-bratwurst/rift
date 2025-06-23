using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;
using Rift.Repositories;

namespace Rift.Tests.Repositories
{
    [TestClass]
    public class MessageEdgeRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions = null!;
        private ApplicationDbContext _dbContext = null!;
        private MessageEdgeRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ApplicationDbContext(_dbOptions);
            _repository = new MessageEdgeRepository(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task AddEdgeAsync_AddsAndReturnsEdge()
        {
            var edge = new MessageEdge { SourceMessageId = 1, TargetMessageId = 2 };
            var result = await _repository.AddEdgeAsync(edge);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.SourceMessageId);
            Assert.AreEqual(2, result.TargetMessageId);

            var dbEdge = await _dbContext.MessageEdges.FindAsync(result.Id);
            Assert.IsNotNull(dbEdge);
        }

        [TestMethod]
        public async Task AddEdgesAsync_AddsAndReturnsEdges()
        {
            var edges = new[]
            {
                new MessageEdge { SourceMessageId = 1, TargetMessageId = 2 },
                new MessageEdge { SourceMessageId = 3, TargetMessageId = 4 }
            };

            var result = await _repository.AddEdgesAsync(edges);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(e => e.SourceMessageId == 1 && e.TargetMessageId == 2));
            Assert.IsTrue(result.Any(e => e.SourceMessageId == 3 && e.TargetMessageId == 4));

            var dbEdges = _dbContext.MessageEdges.ToList();
            Assert.AreEqual(2, dbEdges.Count);
        }

        [TestMethod]
        public async Task RemoveEdgeAsync_RemovesEdgeAndReturnsId_WhenExists()
        {
            var edge = new MessageEdge { SourceMessageId = 1, TargetMessageId = 2 };
            _dbContext.MessageEdges.Add(edge);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.RemoveEdgeAsync(edge.Id);

            Assert.AreEqual(edge.Id, result);
            Assert.IsNull(await _dbContext.MessageEdges.FindAsync(edge.Id));
        }

        [TestMethod]
        public async Task RemoveEdgeAsync_ReturnsNull_WhenNotExists()
        {
            var result = await _repository.RemoveEdgeAsync(9999);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEdgesForConversationAsync_ReturnsEdgesForConversation()
        {
            var userId = "user1";
            var conversationId = 10;

            // Add messages for the conversation
            var msg1 = new Message { Id = 1, ConversationId = conversationId, Content = "A", Role = "user", XCoordinate = 0, YCoordinate = 0, CreatedAt = DateTime.UtcNow };
            var msg2 = new Message { Id = 2, ConversationId = conversationId, Content = "B", Role = "assistant", XCoordinate = 1, YCoordinate = 1, CreatedAt = DateTime.UtcNow };
            var msgOther = new Message { Id = 3, ConversationId = 99, Content = "C", Role = "user", XCoordinate = 2, YCoordinate = 2, CreatedAt = DateTime.UtcNow };
            _dbContext.Messages.AddRange(msg1, msg2, msgOther);

            // Add edges
            var edge1 = new MessageEdge { SourceMessageId = 1, TargetMessageId = 2 };
            var edge2 = new MessageEdge { SourceMessageId = 2, TargetMessageId = 1 };
            var edgeOther = new MessageEdge { SourceMessageId = 3, TargetMessageId = 1 };
            _dbContext.MessageEdges.AddRange(edge1, edge2, edgeOther);

            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetEdgesForConversationAsync(userId, conversationId);

            // Should return edges where either source or target is a message in the conversation
            Assert.IsTrue(result.Any(e => e.SourceMessageId == 1 && e.TargetMessageId == 2));
            Assert.IsTrue(result.Any(e => e.SourceMessageId == 2 && e.TargetMessageId == 1));
            Assert.IsTrue(result.Any(e => e.SourceMessageId == 3 && e.TargetMessageId == 1)); // target is in conversation
            Assert.AreEqual(3, result.Count);
        }
    }
}