using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rift.App.Clients;
using Rift.App.Models;
using System.Text.Json;
using System.Collections.Generic;

namespace Rift.Tests
{
    [TestClass]
    public class ChromaDBClientTests
    {
        private Mock<HttpMessageHandler> _httpHandlerMock;
        private HttpClient _httpClient;
        private Mock<ILogger<ChromaDBClient>> _loggerMock;
        private ChromaDBClient _client;

        [TestInitialize]
        public void Setup()
        {
            _httpHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpHandlerMock.Object);
            _loggerMock = new Mock<ILogger<ChromaDBClient>>();
            _client = new ChromaDBClient(_httpClient, _loggerMock.Object, "http://test");
        }

        [TestMethod]
        public async Task AddDocumentAsync_ReturnsTrue_OnSuccess()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/add", HttpStatusCode.OK);

            var result = await _client.AddDocumentAsync(new AddRequest { Id = "1", CollectionName = "test" });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task AddDocumentAsync_ReturnsFalse_OnFailure()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/add", HttpStatusCode.BadRequest);

            var result = await _client.AddDocumentAsync(new AddRequest { Id = "1", CollectionName = "test" });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetDocumentAsync_ReturnsDocument_OnSuccess()
        {
            var doc = new DocumentResponse { Id = "doc1" };
            var json = JsonSerializer.Serialize(doc, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            SetupHttpResponse(HttpMethod.Get, "http://test/documents/doc1?collection_name=oceanographic_data", HttpStatusCode.OK, json);

            var result = await _client.GetDocumentAsync("doc1");

            Assert.IsNotNull(result);
            Assert.AreEqual("doc1", result.Id);
        }

        [TestMethod]
        public async Task GetDocumentAsync_ReturnsNull_OnNotFound()
        {
            SetupHttpResponse(HttpMethod.Get, "http://test/documents/doc1?collection_name=oceanographic_data", HttpStatusCode.NotFound);

            var result = await _client.GetDocumentAsync("doc1");

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CreateCollectionAsync_ReturnsTrue_OnSuccess()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/collections", HttpStatusCode.OK);

            var result = await _client.CreateCollectionAsync(new CollectionInfo { Name = "test" });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteCollectionAsync_ReturnsFalse_OnFailure()
        {
            SetupHttpResponse(HttpMethod.Delete, "http://test/collections/test", HttpStatusCode.BadRequest);

            var result = await _client.DeleteCollectionAsync("test");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task QueryAsync_ReturnsQueryResponse_OnSuccess()
        {
            var resp = new QueryResponse { Count = 1 };
            var json = JsonSerializer.Serialize(resp, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            SetupHttpResponse(HttpMethod.Post, "http://test/query", HttpStatusCode.OK, json);

            var result = await _client.QueryAsync(new QueryRequest { Text = "test" });

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task AddAsync_CallsAddDocumentAsync()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/add", HttpStatusCode.OK);

            var result = await _client.AddAsync(new AddRequest { Id = "1", CollectionName = "test" });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task QueryLegacyAsync_ReturnsSerializedString_OnSuccess()
        {
            var resp = new QueryResponse { Count = 1 };
            var json = JsonSerializer.Serialize(resp, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            SetupHttpResponse(HttpMethod.Post, "http://test/query", HttpStatusCode.OK, json);

            var result = await _client.QueryLegacyAsync(new QueryRequest { Text = "test" });

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("\"count\":1"));
        }

        // Helper for setting up HttpClient responses
        private void SetupHttpResponse(HttpMethod method, string url, HttpStatusCode statusCode, string content = "")
        {
            _httpHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == method && req.RequestUri.ToString() == url),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                });
        }
    }
}