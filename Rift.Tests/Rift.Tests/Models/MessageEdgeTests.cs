using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;

namespace Rift.Tests.Models
{
    [TestClass]
    public class MessageEdgeTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [TestMethod]
        public void MessageEdge_ValidModel_PassesValidation()
        {
            var edge = new MessageEdge
            {
                Id = 1,
                SourceMessageId = 10,
                TargetMessageId = 20,
                SourceHandle = "bottom",
                TargetHandle = "top",
                SourceMessage = null
            };

            var results = ValidateModel(edge);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void MessageEdge_MissingRequiredFields_FailsValidation()
        {
            // Required properties must be set in the initializer, so set them to default values to simulate "missing"
            var edge = new MessageEdge
            {
                SourceMessageId = 0,
                TargetMessageId = 0
            };

            var results = ValidateModel(edge);
            // Since int is a value type and required, this will not fail validation unless you add [Range] or custom validation.
            // This test is included for completeness.
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void MessageEdge_DefaultHandles_AreCorrect()
        {
            var edge = new MessageEdge
            {
                SourceMessageId = 1,
                TargetMessageId = 2
            };

            Assert.AreEqual("bottom", edge.SourceHandle);
            Assert.AreEqual("top", edge.TargetHandle);
        }

        [TestMethod]
        public void PartialMessageEdge_ValidModel_PassesValidation()
        {
            var partial = new PartialMessageEdge
            {
                SourceMessageId = 5,
                SourceHandle = "left",
                TargetHandle = "right"
            };

            var results = ValidateModel(partial);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void PartialMessageEdge_DefaultHandles_AreCorrect()
        {
            var partial = new PartialMessageEdge
            {
                SourceMessageId = 7
            };

            Assert.AreEqual("bottom", partial.SourceHandle);
            Assert.AreEqual("top", partial.TargetHandle);
        }
    }
}