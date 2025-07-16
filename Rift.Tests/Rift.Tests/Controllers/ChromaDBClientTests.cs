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

        #region Metadata Filename Normalization Tests

        [TestMethod]
        public async Task AddDocumentAsync_NormalizesFilenameInMetadata()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/add", HttpStatusCode.OK);
            
            var request = new AddRequest 
            { 
                Id = "1", 
                CollectionName = "test",
                Metadata = new Dictionary<string, object> { { "filename", "document.txt" } }
            };

            var result = await _client.AddDocumentAsync(request);

            Assert.IsTrue(result);
            Assert.AreEqual("document.md", request.Metadata["filename"]);
        }

        [TestMethod]
        public async Task AddDocumentAsync_NormalizesMultipleFilenameFields()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/add", HttpStatusCode.OK);
            
            var request = new AddRequest 
            { 
                Id = "1", 
                CollectionName = "test",
                Metadata = new Dictionary<string, object> 
                { 
                    { "filename", "document1.txt" },
                    { "file_name", "document2.txt" },
                    { "fileName", "document3.TXT" },
                    { "name", "document4.Txt" }
                }
            };

            var result = await _client.AddDocumentAsync(request);

            Assert.IsTrue(result);
            Assert.AreEqual("document1.md", request.Metadata["filename"]);
            Assert.AreEqual("document2.md", request.Metadata["file_name"]);
            Assert.AreEqual("document3.md", request.Metadata["fileName"]);
            Assert.AreEqual("document4.md", request.Metadata["name"]);
        }

        [TestMethod]
        public async Task AddDocumentAsync_HandlesNullMetadata()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/add", HttpStatusCode.OK);
            
            var request = new AddRequest 
            { 
                Id = "1", 
                CollectionName = "test",
                Metadata = null
            };

            var result = await _client.AddDocumentAsync(request);

            Assert.IsTrue(result); // Should not throw exception
        }

        [TestMethod]
        public async Task AddDocumentAsync_HandlesEmptyMetadata()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/add", HttpStatusCode.OK);
            
            var request = new AddRequest 
            { 
                Id = "1", 
                CollectionName = "test",
                Metadata = new Dictionary<string, object>()
            };

            var result = await _client.AddDocumentAsync(request);

            Assert.IsTrue(result); // Should not throw exception
        }

        [TestMethod]
        public async Task AddDocumentAsync_HandlesNullFilenameValues()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/add", HttpStatusCode.OK);
            
            var request = new AddRequest 
            { 
                Id = "1", 
                CollectionName = "test",
                Metadata = new Dictionary<string, object> 
                { 
                    { "filename", null! },
                    { "file_name", "" },
                    { "name", "   " }
                }
            };

            var result = await _client.AddDocumentAsync(request);

            Assert.IsTrue(result); // Should not throw exception
            // Null, empty, and whitespace values should remain unchanged
            Assert.IsNull(request.Metadata["filename"]);
            Assert.AreEqual("", request.Metadata["file_name"]);
            Assert.AreEqual("   ", request.Metadata["name"]);
        }

        [TestMethod]
        public async Task AddDocumentAsync_PreservesNonTxtFilenames()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/add", HttpStatusCode.OK);
            
            var request = new AddRequest 
            { 
                Id = "1", 
                CollectionName = "test",
                Metadata = new Dictionary<string, object> 
                { 
                    { "filename", "document.pdf" },
                    { "file_name", "image.jpg" },
                    { "name", "already.md" }
                }
            };

            var result = await _client.AddDocumentAsync(request);

            Assert.IsTrue(result);
            Assert.AreEqual("document.pdf", request.Metadata["filename"]);
            Assert.AreEqual("image.jpg", request.Metadata["file_name"]);
            Assert.AreEqual("already.md", request.Metadata["name"]);
        }

        [TestMethod]
        public async Task AddDocumentsBatchAsync_NormalizesFilenamesInBatch()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/batch", HttpStatusCode.OK);
            
            var request = new BatchDocumentsRequest 
            { 
                CollectionName = "test",
                Documents = new List<Document>
                {
                    new Document 
                    { 
                        Id = "1", 
                        Metadata = new Dictionary<string, object> { { "filename", "doc1.txt" } }
                    },
                    new Document 
                    { 
                        Id = "2", 
                        Metadata = new Dictionary<string, object> { { "file_name", "doc2.TXT" } }
                    },
                    new Document 
                    { 
                        Id = "3", 
                        Metadata = new Dictionary<string, object> { { "name", "doc3.pdf" } }
                    }
                }
            };

            var result = await _client.AddDocumentsBatchAsync(request);

            Assert.IsTrue(result);
            Assert.AreEqual("doc1.md", request.Documents[0].Metadata!["filename"]);
            Assert.AreEqual("doc2.md", request.Documents[1].Metadata!["file_name"]);
            Assert.AreEqual("doc3.pdf", request.Documents[2].Metadata!["name"]); // Should remain unchanged
        }

        [TestMethod]
        public async Task AddDocumentsBatchAsync_HandlesDocumentsWithNullMetadata()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/batch", HttpStatusCode.OK);
            
            var request = new BatchDocumentsRequest 
            { 
                CollectionName = "test",
                Documents = new List<Document>
                {
                    new Document { Id = "1", Metadata = null },
                    new Document 
                    { 
                        Id = "2", 
                        Metadata = new Dictionary<string, object> { { "filename", "doc2.txt" } }
                    }
                }
            };

            var result = await _client.AddDocumentsBatchAsync(request);

            Assert.IsTrue(result); // Should not throw exception
            Assert.IsNull(request.Documents[0].Metadata);
            Assert.AreEqual("doc2.md", request.Documents[1].Metadata!["filename"]);
        }

        [TestMethod]
        public async Task AddDocumentAsync_HandlesComplexFilenameEdgeCases()
        {
            SetupHttpResponse(HttpMethod.Post, "http://test/documents/add", HttpStatusCode.OK);
            
            var request = new AddRequest 
            { 
                Id = "1", 
                CollectionName = "test",
                Metadata = new Dictionary<string, object> 
                { 
                    { "filename", "file.backup.txt" }, // Multiple dots
                    { "file_name", ".txt" }, // Starting with dot
                    { "fileName", "файл.txt" }, // Unicode characters
                    { "name", "file-name_with@special#chars.TXT" } // Special characters
                }
            };

            var result = await _client.AddDocumentAsync(request);

            Assert.IsTrue(result);
            Assert.AreEqual("file.backup.md", request.Metadata["filename"]);
            Assert.AreEqual(".md", request.Metadata["file_name"]);
            Assert.AreEqual("файл.md", request.Metadata["fileName"]);
            Assert.AreEqual("file-name_with@special#chars.md", request.Metadata["name"]);
        }

        #endregion

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