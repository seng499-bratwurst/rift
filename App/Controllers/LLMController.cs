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

            var response = await _llmProvider.GenerateJSON(request.Prompt);
            using var doc = JsonDocument.Parse(response);

            // Clone it so we can return it after the doc is disposed
            var json = doc.RootElement.Clone();

            string service = json.GetProperty("service").GetString() ?? "locations";
            var token = _config["ONC_TOKEN"];

            if (string.IsNullOrWhiteSpace(token))
        return StatusCode(500, "ONC_TOKEN is not set in configuration or environment variables.");

    // Step 4: Build the query string (excluding "service", and injecting real token)
    var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

    foreach (var prop in json.EnumerateObject())
{
    if (prop.Name == "token")
    {
        query["token"] = token; // override placeholder token
    }
    else if (prop.Name == "method" || prop.Name == "locationName")
    {
        if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
        {
            query[prop.Name] = prop.Value.ToString();
        }
    }
}


            // Step 5: Construct the full ONC API URL
    var rawQuery = query.ToString()?.Replace("+", " ");
        var oncApiUrl = $"https://data.oceannetworks.ca/api/{service}?{rawQuery}";
        Console.WriteLine("[DEBUG] ONC API URL: " + oncApiUrl);

        // Step 6: Call the ONC API
        var oncResponse = await _httpClient.GetAsync(oncApiUrl);
        if (!oncResponse.IsSuccessStatusCode)
            return StatusCode((int)oncResponse.StatusCode, $"ONC API error: {oncResponse.StatusCode}");

        var oncContent = await oncResponse.Content.ReadAsStringAsync();
        var oncData = JsonDocument.Parse(oncContent).RootElement.Clone();

            var fina_res = await _llmProvider.GenerateFinalResponse(request.Prompt, oncData);


            return Ok(fina_res);
            // return Ok(new { response });
        }


    }
}
