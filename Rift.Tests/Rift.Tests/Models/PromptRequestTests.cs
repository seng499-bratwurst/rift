using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;

namespace Rift.Tests.Models
{
    [TestClass]
    public class PromptRequestTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [TestMethod]
        public void PromptRequest_Default_PassesValidation()
        {
            var req = new PromptRequest();
            var results = ValidateModel(req);
            Assert.AreEqual(0, results.Count);
            Assert.AreEqual(string.Empty, req.Prompt);
        }

        [TestMethod]
        public void PromptRequest_CanSetPrompt()
        {
            var req = new PromptRequest { Prompt = "What is the ocean?" };
            var results = ValidateModel(req);
            Assert.AreEqual(0, results.Count);
            Assert.AreEqual("What is the ocean?", req.Prompt);
        }
    }
}