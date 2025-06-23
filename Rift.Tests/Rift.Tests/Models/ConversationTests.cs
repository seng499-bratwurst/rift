using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;

namespace Rift.Tests.Models
{
    [TestClass]
    public class ConversationTests
    {
        [TestMethod]
        public void Conversation_Defaults_AreCorrect()
        {
            var conversation = new Conversation();

            Assert.AreEqual(0, conversation.Id);
            Assert.IsNull(conversation.UserId);
            Assert.IsNull(conversation.SessionId);
            Assert.IsNull(conversation.Title);
            Assert.AreEqual(default(DateTime), conversation.FirstInteraction);
            Assert.AreEqual(default(DateTime), conversation.LastInteraction);
            Assert.IsNull(conversation.User);
        }

        [TestMethod]
        public void Conversation_CanSetAllProperties()
        {
            var now = DateTime.UtcNow;
            var user = new User { Id = "user1", Name = "Test User", Email = "test@email.com" };

            var conversation = new Conversation
            {
                Id = 123,
                UserId = "user1",
                SessionId = "session-abc",
                Title = "Test Conversation",
                FirstInteraction = now,
                LastInteraction = now.AddMinutes(5),
                User = user
            };

            Assert.AreEqual(123, conversation.Id);
            Assert.AreEqual("user1", conversation.UserId);
            Assert.AreEqual("session-abc", conversation.SessionId);
            Assert.AreEqual("Test Conversation", conversation.Title);
            Assert.AreEqual(now, conversation.FirstInteraction);
            Assert.AreEqual(now.AddMinutes(5), conversation.LastInteraction);
            Assert.AreEqual(user, conversation.User);
        }
    }
}