using Microsoft.AspNetCore.Mvc;
using Rift.Models;         // For PromptRequest model
using Rift.LLM;            // For ILlmProvider interface
using System.Text.Json;

namespace Rift.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LLMController : ControllerBase
    {
        private readonly ILlmProvider _llmProvider;

        public LLMController(ILlmProvider llmProvider)
        {
            _llmProvider = llmProvider;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] PromptRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt cannot be empty.");

            var response = await _llmProvider.GatherOncAPIData(request.Prompt);
                       
            using var doc = JsonDocument.Parse(response);

            // Clone it so we can return it after the doc is disposed
            JsonElement json = doc.RootElement.Clone();

            var finalResponse = await _llmProvider.GenerateFinalResponse(request.Prompt, json);

            return Ok(finalResponse);
        }
    }
}