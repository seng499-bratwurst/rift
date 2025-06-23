using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;

namespace Rift.Tests.Models
{
    [TestClass]
    public class UserTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [TestMethod]
        public void User_ValidModel_PassesValidation()
        {
            var user = new User
            {
                Id = "user1",
                Name = "Test User",
                UserName = "testuser",
                Email = "test@email.com",
                ONCApiToken = "token"
            };

            var results = ValidateModel(user);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void User_ONCApiToken_IsOptional()
        {
            var user = new User
            {
                Id = "user3",
                Name = "No Token User",
                UserName = "notokenuser",
                Email = "notoken@email.com"
                // ONCApiToken is not set
            };

            var results = ValidateModel(user);
            Assert.AreEqual(0, results.Count);
            Assert.IsNull(user.ONCApiToken);
        }
    }
}