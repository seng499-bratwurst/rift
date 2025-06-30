using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Rift.Controllers;
using Rift.LLM;
using Rift.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rift.Tests
{
    [TestClass]
    public class LLMControllerTests
    {
        [TestMethod]
        public async Task Ask_ReturnsBadRequest_WhenPromptIsEmpty()
        {
            var llmProviderMock = new Mock<ILlmProvider>();
            var controller = new LLMController(llmProviderMock.Object);

            var result = await controller.Ask(new PromptRequest { Prompt = "" });

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Ask_ReturnsOk_WithFinalResponse()
        {
            var llmProviderMock = new Mock<ILlmProvider>();
            var fakePrompt = "What is the ocean?";
            var fakeApiResponse = "{\"answer\":\"The ocean is vast.\"}";
            var fakeFinalResponse = "The ocean is vast.";

            llmProviderMock.Setup(x => x.GatherOncAPIData(fakePrompt))
                .ReturnsAsync(fakeApiResponse);

            llmProviderMock.Setup(x => x.GenerateFinalResponse(fakePrompt, It.IsAny<JsonElement>()))
                .ReturnsAsync(fakeFinalResponse);

            var controller = new LLMController(llmProviderMock.Object);

            var result = await controller.Ask(new PromptRequest { Prompt = fakePrompt });

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(fakeFinalResponse, okResult.Value);
        }
    }
}