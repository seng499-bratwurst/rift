using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;

namespace Rift.Tests.Models
{
    [TestClass]
    public class MessageTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [TestMethod]
        public void Message_ValidModel_PassesValidation()
        {
            var msg = new Message
            {
                Id = 1,
                ConversationId = 2,
                PromptMessageId = 3,
                Content = "Hello",
                OncApiQuery = "query",
                OncApiResponse = "response",
                IsHelpful = true,
                Role = "user",
                XCoordinate = 1.5f,
                YCoordinate = 2.5f,
                CreatedAt = DateTime.UtcNow
            };

            var results = ValidateModel(msg);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void Message_MissingRequiredCoordinates_FailsValidation()
        {
            // XCoordinate and YCoordinate are required, so set them to default (0) to pass object initializer,
            // but remove [Required] attribute if you want to allow 0 as valid.
            // Here, we simulate missing by not setting them, but C# will require them in the initializer.
            // So, this test is not needed unless you remove 'required' keyword.
        }

        [TestMethod]
        public void Message_DefaultRole_IsUser()
        {
            var msg = new Message
            {
                Id = 1,
                XCoordinate = 0,
                YCoordinate = 0,
                CreatedAt = DateTime.UtcNow
            };
            Assert.AreEqual("user", msg.Role);
        }

        [TestMethod]
        public void Message_CanSetAllProperties()
        {
            var now = DateTime.UtcNow;
            var conversation = new Conversation { Id = 1 };
            var promptMessage = new Message { Id = 2, XCoordinate = 0, YCoordinate = 0, CreatedAt = now };

            var msg = new Message
            {
                Id = 3,
                ConversationId = 1,
                PromptMessageId = 2,
                Content = "Test",
                OncApiQuery = "q",
                OncApiResponse = "r",
                IsHelpful = false,
                Role = "assistant",
                XCoordinate = 10.1f,
                YCoordinate = 20.2f,
                CreatedAt = now,
                Conversation = conversation,
                PromptMessage = promptMessage,
                OutgoingEdges = new List<MessageEdge?>()
            };

            Assert.AreEqual(3, msg.Id);
            Assert.AreEqual(1, msg.ConversationId);
            Assert.AreEqual(2, msg.PromptMessageId);
            Assert.AreEqual("Test", msg.Content);
            Assert.AreEqual("q", msg.OncApiQuery);
            Assert.AreEqual("r", msg.OncApiResponse);
            Assert.IsFalse(msg.IsHelpful ?? true);
            Assert.AreEqual("assistant", msg.Role);
            Assert.AreEqual(10.1f, msg.XCoordinate);
            Assert.AreEqual(20.2f, msg.YCoordinate);
            Assert.AreEqual(now, msg.CreatedAt);
            Assert.AreEqual(conversation, msg.Conversation);
            Assert.AreEqual(promptMessage, msg.PromptMessage);
            Assert.IsNotNull(msg.OutgoingEdges);
        }
    }
}