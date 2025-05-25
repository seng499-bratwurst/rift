using Microsoft.AspNetCore.Mvc;
using Rift.Models;         // For PromptRequest model
using Rift.LLM;            // For ILlmProvider interface

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

            var response = await _llmProvider.GenerateResponseAsync(request.Prompt);
            return Ok(new { response });
        }
    }
}
