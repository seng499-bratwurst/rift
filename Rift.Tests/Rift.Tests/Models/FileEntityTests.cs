using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;

namespace Rift.Tests.Models
{
    [TestClass]
    public class FileEntityTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [TestMethod]
        public void FileEntity_ValidModel_PassesValidation()
        {
            var file = new FileEntity
            {
                Id = 1,
                FileName = "file.txt",
                Content = "file content",
                Size = 123,
                UploadedBy = "user1",
                CreatedAt = DateTime.UtcNow
            };

            var results = ValidateModel(file);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void FileEntity_MissingRequiredFields_FailsValidation()
        {
            // Set required properties to empty strings to satisfy the constructor,
            // but still fail validation due to [Required] attributes.
            var file = new FileEntity
            {
                FileName = "",
                Content = "",
                UploadedBy = ""
            };

            var results = ValidateModel(file);
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("FileName")));
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Content")));
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("UploadedBy")));
        }

        [TestMethod]
        public void FileEntity_FileNameTooLong_FailsValidation()
        {
            var file = new FileEntity
            {
                FileName = new string('a', 256),
                Content = "content",
                UploadedBy = "user"
            };

            var results = ValidateModel(file);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("FileName")));
        }

        [TestMethod]
        public void FileEntityDto_ValidModel_PassesValidation()
        {
            var dto = new FileEntityDto
            {
                Id = 1,
                FileName = "file.txt",
                UploadedBy = "user1",
                CreatedAt = DateTime.UtcNow,
                SourceType = "cambridge_bay_papers",
                SourceLink = "http://example.com"
            };

            var results = ValidateModel(dto);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void FileEntityDto_MissingRequiredFields_FailsValidation()
        {
            var dto = new FileEntityDto
            {
                FileName = "",
                UploadedBy = "",
                SourceType = "",
                SourceLink = ""
            };

            var results = ValidateModel(dto);
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("FileName")));
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("UploadedBy")));
        }

        [TestMethod]
        public void FileEntityDto_FileNameTooLong_FailsValidation()
        {
            var dto = new FileEntityDto
            {
                FileName = new string('b', 256),
                UploadedBy = "user",
                SourceType = "cambridge_bay_papers",
                SourceLink = "http://example.com"
            };

            var results = ValidateModel(dto);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("FileName")));
        }
    }
}