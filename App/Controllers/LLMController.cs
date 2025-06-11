using Microsoft.AspNetCore.Mvc;
using Rift.Models;         // For PromptRequest model
using Rift.LLM;            // For ILlmProvider interface
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Rift.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LLMController : ControllerBase
    {
        private readonly ILlmProvider _llmProvider;
        private readonly IConfiguration _config;
         private readonly HttpClient _httpClient;

        public LLMController(ILlmProvider llmProvider, IConfiguration config)
        {
            _llmProvider = llmProvider;
            _config = config;
            _httpClient = new HttpClient();
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] PromptRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt cannot be empty.");

            var response = await _llmProvider.GenerateONCAPICall(request.Prompt);
            
            
            using var doc = JsonDocument.Parse(response);

            // Clone it so we can return it after the doc is disposed
            JsonElement json = doc.RootElement.Clone();

            var final_res = await _llmProvider.GenerateFinalResponse(request.Prompt, json);

            return Ok(final_res);
            // return Ok(new { response });
        }


    }
}