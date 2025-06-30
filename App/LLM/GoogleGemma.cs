using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Rift.LLM
{
    /// <summary>
    /// LLM provider for Google's Gemma 3n model, implementing ILlmProvider.
    /// </summary>
    public class GoogleGemma : ILlmProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string _modelName;
        private readonly OncFunctionParser _parser;

        /// <summary>
        /// Constructor reads values from appsettings.json
        /// </summary>
        public GoogleGemma(IConfiguration config, OncFunctionParser parser)
        {
            _httpClient = new HttpClient();

            // Example config section:
            // "LLMSettings": {
            //   "GoogleGemma": {
            //     "ApiKey": "",
            //     "Endpoint": "https://generativelanguage.googleapis.com/v1beta/openai/chat/completions",
            //     "ModelName": "gemma-3n"
            //   }
            // }
            _apiKey = config["LLMSettings:GoogleGemma:ApiKey"] ?? throw new ArgumentNullException("GoogleGemma ApiKey missing in config");
            _endpoint = config["LLMSettings:GoogleGemma:Endpoint"] ?? throw new ArgumentNullException("GoogleGemma Endpoint missing in config");
            _modelName = config["LLMSettings:GoogleGemma:ModelName"] ?? "gemma-3n-e4b-it";
            _parser = parser;
        }

        /// <summary>
        /// Sends a prompt to the Gemma 3n model to generate an ONC API call.
        /// </summary>
        public async Task<string> GenerateONCAPICall(string prompt)
        {
            // System prompt file should be present as in other providers
            // string systemPrompt = System.IO.File.ReadAllText(
            //     System.IO.Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "function_call_required_or_not.md")
            // );
            string systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "function_call_required_or_not.md"));

            var payload = new
            {
                model = _modelName,
                messages = new[]
                {
                    new { role = "user", content = systemPrompt },
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.SendAsync(request);

            
            // Console.WriteLine($"Response Status Code: {response.StatusCode}");
            // Console.WriteLine($"Response Headers: {response.Headers}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Response: {errorContent}");
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseContent);

            // OpenAI-compatible response: { "choices": [ { "message": { "content": "..." } } ] }
            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            // Console.WriteLine($"Content: {content}");

    
            var match = Regex.Match(content, @"\{(?:[^{}]|(?<open>\{)|(?<-open>\}))*\}(?(open)(?!))", RegexOptions.Singleline);

            object generalResponse = null;

            if (!match.Success)
            {
                generalResponse = new {
                    response = content,
                    message = "ONC API call not required. Answer based on the user prompt."
                };
            }else
            {
                String LLMContentFiltered = match.Value;
                // Console.WriteLine($"Filtered Content: {LLMContentFiltered}");

                using var innerDoc = JsonDocument.Parse(LLMContentFiltered);
                bool useFunction = innerDoc.RootElement.GetProperty("use_function").GetBoolean();

                if (!useFunction)
                {
                    generalResponse = new {
                        message = "ONC API call not required. Answer based on the user prompt."
                    };

                    return JsonSerializer.Serialize(generalResponse);
                }
                else if (useFunction)
                {
                    var (functionName, functionParams) = _parser.ExtractFunctionAndQueries(LLMContentFiltered);
                    return await _parser.OncAPICall(functionName, functionParams);
                }
            }
            
            return JsonSerializer.Serialize(generalResponse);
        }

        /// <summary>
        /// Sends a prompt and ONC API response to the Gemma 3n model to generate a final user-facing answer.
        /// </summary>
        public async Task<string> GenerateFinalResponse(string prompt, JsonElement onc_api_response)
        {
            string jsonInput = JsonSerializer.Serialize(onc_api_response, new JsonSerializerOptions { WriteIndented = true });
            var systemPrompt = "You are a helpful Ocean Networks Canada assistant that interprets the data given and answers the user prompt with accuracy.";

            string fullPrompt = $"{prompt}\n\nHere is the ONC API response:\n{jsonInput}";

            var payload = new
            {
                model = _modelName,
                messages = new[]
                {
                    new { role = "user", content = systemPrompt },
                    new { role = "user", content = fullPrompt }
                }
            };

            var json = JsonSerializer.Serialize(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseContent);
            var result = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return result ?? "No response from Gemma 3n model.";
        }
    }
}
