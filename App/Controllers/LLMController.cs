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

            // Check if there is a token
            // Skip if there is no token
                // Decode token and get the user information
                // If there is a user, store the message and the response the the database
                // If there is a conversation_id, use that otherwise create a new conversation

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt cannot be empty.");

            var response = await _llmProvider.GenerateResponseAsync(request.Prompt);
            return Ok(new { response });
        }
    }
}
