using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;
using Rift.Services;

namespace Rift.Tests.Services
{
    [TestClass]
    public class JwtTokenServiceTests
    {
        private JwtTokenService _service = null!;
        private IConfiguration _config = null!;

        [TestInitialize]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "super_secret_test_key_1234567890123456"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _service = new JwtTokenService(_config);
        }

        [TestMethod]
        public void GenerateJwtToken_ReturnsValidToken_WithCorrectClaims()
        {
            var user = new User
            {
                Id = "42", // Id is string
                Name = "Test User", // Required property
                UserName = "testuser",
                Email = "test@email.com"
            };
            var roles = new List<string> { "Admin", "User" };

            var token = _service.GenerateJwtToken(user, roles);

            Assert.IsFalse(string.IsNullOrEmpty(token));

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.AreEqual("TestIssuer", jwtToken.Issuer);
            Assert.AreEqual("TestAudience", jwtToken.Audiences.FirstOrDefault());

            var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            Assert.IsNotNull(subClaim);
            Assert.AreEqual("42", subClaim.Value);

            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);
            Assert.IsNotNull(nameClaim);
            Assert.AreEqual("testuser", nameClaim.Value);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            Assert.IsNotNull(emailClaim);
            Assert.AreEqual("test@email.com", emailClaim.Value);

            var roleClaims = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            CollectionAssert.AreEquivalent(roles, roleClaims);
        }

        [TestMethod]
        public void GetUserIdFromBearerToken_ReturnsUserId_WhenTokenIsValid()
        {
            var user = new User
            {
                Id = "99", // Id is string
                Name = "Another User", // Required property
                UserName = "anotheruser",
                Email = "another@email.com"
            };
            var roles = new List<string> { "User" };
            var token = _service.GenerateJwtToken(user, roles);
            var bearer = "Bearer " + token;

            var userId = _service.GetUserIdFromBearerToken(bearer);

            Assert.AreEqual("99", userId);
        }

        [TestMethod]
        public void GetUserIdFromBearerToken_ReturnsEmpty_WhenHeaderIsNullOrEmpty()
        {
            Assert.AreEqual("", _service.GetUserIdFromBearerToken(null));
            Assert.AreEqual("", _service.GetUserIdFromBearerToken(""));
        }

        [TestMethod]
        public void GetUserIdFromBearerToken_ReturnsEmpty_WhenHeaderIsNotBearer()
        {
            Assert.AreEqual("", _service.GetUserIdFromBearerToken("Basic xyz"));
        }

        [TestMethod]
        public void GetUserIdFromBearerToken_ReturnsEmpty_WhenTokenIsInvalid()
        {
            Assert.AreEqual("", _service.GetUserIdFromBearerToken("Bearer not_a_real_token"));
        }
    }
}