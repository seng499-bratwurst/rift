using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Rift.Services;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Rift.Tests.Services
{
    [TestClass]
    public class GeminiTitleServiceTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock = null!;
        private Mock<IConfiguration> _configurationMock = null!;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock = null!;
        private HttpClient _httpClient = null!;

        [TestInitialize]
        public void Setup()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _configurationMock = new Mock<IConfiguration>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(_httpClient);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _httpClient?.Dispose();
        }

        [TestMethod]
        public async Task GenerateTitleAsync_ValidResponse_ExtractsTitleCorrectly()
        {
            // Arrange
            var userPrompt = "What is the temperature?";
            var assistantResponse = "The current temperature is 15°C.";
            var expectedTitle = "Current Temperature Inquiry";

            var geminiResponse = new
            {
                candidates = new[]
                {
                    new
                    {
                        content = new
                        {
                            parts = new[]
                            {
                                new { text = $"Title: {expectedTitle}" }
                            }
                        }
                    }
                }
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(geminiResponse), Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var service = new GeminiTitleService(_httpClientFactoryMock.Object, _configurationMock.Object, "test-api-key");

            // Act
            var result = await service.GenerateTitleAsync(userPrompt, assistantResponse);

            // Assert
            Assert.AreEqual(expectedTitle, result);
        }

        [TestMethod]
        public async Task GenerateTitleAsync_TitleWithFormatting_RemovesFormatting()
        {
            // Arrange

            var userPrompt = "How to cook pasta?";
            var assistantResponse = "To cook pasta, boil water and add the pasta...";
            var formattedTitle = "**Cooking Pasta Instructions**";
            var expectedTitle = "Cooking Pasta Instructions";

            var geminiResponse = new
            {
                candidates = new[]
                {
                    new
                    {
                        content = new
                        {
                            parts = new[]
                            {
                                new { text = $"Title: {formattedTitle}" }
                            }
                        }
                    }
                }
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(geminiResponse), Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var service = new GeminiTitleService(_httpClientFactoryMock.Object, _configurationMock.Object, "test-api-key");

            // Act
            var result = await service.GenerateTitleAsync(userPrompt, assistantResponse);

            // Assert
            Assert.AreEqual(expectedTitle, result);
        }

        [TestMethod]
        public async Task GenerateTitleAsync_TitleTooLong_TruncatesToNineWords()
        {
            // Arrange

            var userPrompt = "Complex question?";
            var assistantResponse = "Complex answer...";
            var longTitle = "This is a very long title that has more than nine words in it";
            var expectedTitle = "This is a very long title that has more";

            var geminiResponse = new
            {
                candidates = new[]
                {
                    new
                    {
                        content = new
                        {
                            parts = new[]
                            {
                                new { text = $"Title: {longTitle}" }
                            }
                        }
                    }
                }
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(geminiResponse), Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var service = new GeminiTitleService(_httpClientFactoryMock.Object, _configurationMock.Object, "test-api-key");

            // Act
            var result = await service.GenerateTitleAsync(userPrompt, assistantResponse);

            // Assert
            Assert.AreEqual(expectedTitle, result);
        }

        [TestMethod]
        public async Task GenerateTitleAsync_NoTitlePrefix_ReturnsTitleAsIs()
        {
            // Arrange

            var userPrompt = "Simple question?";
            var assistantResponse = "Simple answer";
            var titleWithoutPrefix = "Just a Title";

            var geminiResponse = new
            {
                candidates = new[]
                {
                    new
                    {
                        content = new
                        {
                            parts = new[]
                            {
                                new { text = titleWithoutPrefix }
                            }
                        }
                    }
                }
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(geminiResponse), Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var service = new GeminiTitleService(_httpClientFactoryMock.Object, _configurationMock.Object, "test-api-key");

            // Act
            var result = await service.GenerateTitleAsync(userPrompt, assistantResponse);

            // Assert
            Assert.AreEqual(titleWithoutPrefix, result);
        }

        [TestMethod]
        public async Task GenerateTitleAsync_ApiError_ReturnsFallbackTitle()
        {
            // Arrange

            var userPrompt = "Error test?";
            var assistantResponse = "Error response";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var service = new GeminiTitleService(_httpClientFactoryMock.Object, _configurationMock.Object, "test-api-key");

            // Act
            var result = await service.GenerateTitleAsync(userPrompt, assistantResponse);

            // Assert
            Assert.AreEqual("New Conversation", result);
        }

        [TestMethod]
        public async Task GenerateTitleAsync_EmptyResponse_ReturnsFallbackTitle()
        {
            // Arrange

            var userPrompt = "Empty test?";
            var assistantResponse = "Empty response";

            var emptyResponse = new
            {
                candidates = new object[0]
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(emptyResponse), Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var service = new GeminiTitleService(_httpClientFactoryMock.Object, _configurationMock.Object, "test-api-key");

            // Act
            var result = await service.GenerateTitleAsync(userPrompt, assistantResponse);

            // Assert
            Assert.AreEqual("New Conversation", result);
        }

        [TestMethod]
        public async Task GenerateTitleAsync_HttpException_ReturnsFallbackTitle()
        {
            // Arrange

            var userPrompt = "Http exception test?";
            var assistantResponse = "Http exception response";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            var service = new GeminiTitleService(_httpClientFactoryMock.Object, _configurationMock.Object, "test-api-key");

            // Act
            var result = await service.GenerateTitleAsync(userPrompt, assistantResponse);

            // Assert
            Assert.AreEqual("New Conversation", result);
        }

        [TestMethod]
        public async Task GenerateTitleAsync_InvalidJson_ReturnsFallbackTitle()
        {
            // Arrange

            var userPrompt = "Invalid JSON test?";
            var assistantResponse = "Invalid JSON response";

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{ invalid json }", Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var service = new GeminiTitleService(_httpClientFactoryMock.Object, _configurationMock.Object, "test-api-key");

            // Act
            var result = await service.GenerateTitleAsync(userPrompt, assistantResponse);

            // Assert
            Assert.AreEqual("New Conversation", result);
        }

        [TestMethod]
        public void Constructor_MissingApiKey_ThrowsException()
        {
            // Arrange - Clear environment variable to simulate missing API key scenario
            Environment.SetEnvironmentVariable("GOOGLE_API_KEY", null);
            Environment.SetEnvironmentVariable("GOOGLE_API_KEY", "");

            // Act & Assert - Use the main constructor without API key parameter
            Assert.ThrowsException<InvalidOperationException>(() =>
                new GeminiTitleService(_httpClientFactoryMock.Object, _configurationMock.Object));
        }

        [TestMethod]
        public async Task GenerateTitleAsync_CaseInsensitiveTitlePrefix_ExtractsTitleCorrectly()
        {
            // Arrange

            var userPrompt = "Case test?";
            var assistantResponse = "Case test response";
            var expectedTitle = "Case Insensitive Title";

            var geminiResponse = new
            {
                candidates = new[]
                {
                    new
                    {
                        content = new
                        {
                            parts = new[]
                            {
                                new { text = $"title: {expectedTitle}" } // lowercase prefix
                            }
                        }
                    }
                }
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(geminiResponse), Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var service = new GeminiTitleService(_httpClientFactoryMock.Object, _configurationMock.Object, "test-api-key");

            // Act
            var result = await service.GenerateTitleAsync(userPrompt, assistantResponse);

            // Assert
            Assert.AreEqual(expectedTitle, result);
        }
    }
}
