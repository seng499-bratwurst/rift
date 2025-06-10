using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json.Nodes;
using App.LLM;
using System.Text.RegularExpressions;


namespace Rift.LLM
{
    public class HuggingFace : ILlmProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string _modelSmall;
        private readonly string _modelBig;
        private readonly FunctionCallSwitch _switch;

        // Constructor reads values from appsettings.json
        public HuggingFace(IConfiguration config, FunctionCallSwitch Switch)
        {
            _httpClient = new HttpClient();

            // Loading API Key, endpoint, and model from appsettings.json
            _apiKey = config["LLmSettings:HuggingFace:ApiKey"]!;
            _endpoint = config["LLmSettings:HuggingFace:Endpoint"]!;
            _modelSmall = config["LLmSettings:HuggingFace:ModelSmall"]!;
            _modelBig = config["LLmSettings:HuggingFace:ModelBig"]!;
            _switch = Switch;
        }



        // Sends a chat completion request to Hugging Face endpoint
        public async Task<string> GenerateONCAPICall(string prompt)
        {
            // Equivalent cURL:
            // curl https://router.huggingface.co/together/v1/chat/completions \
            // -H 'Authorization: Bearer hf_xxx' \
            // -H 'Content-Type: application/json' \
            // -d '{ "messages": [{ "role": "user", "content": "..." }], "model": "...", "stream": false }'

            string system_Prompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "function_call_required_or_not.md"));

            var payload = new
            {
                model = _modelSmall,
                messages = new[]
                {
                    new { role = "system", content = system_Prompt },
                    new { role = "user", content = prompt }
                },
                stream = false,

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


            var message = doc.RootElement.GetProperty("choices")[0].GetProperty("message");

            string content_llm = message.GetProperty("content").GetString() ?? string.Empty;
           

            using JsonDocument innerDoc = JsonDocument.Parse(content_llm);

            bool useFunction = innerDoc.RootElement.GetProperty("use_function").GetBoolean();

            if (!useFunction)
            {
                return "{}";
            }
            else if (useFunction)
            {
                var (functionName, args) = _switch.ExtractFunctionAndArgsFromContent(content_llm);
                
                return await _switch.ONC_API_Call(functionName, args);
            }

            var resultContent = message.GetProperty("content").GetString();

            return resultContent ?? "{}";
        }


        public async Task<string> GenerateFinalResponse(string prompt, JsonElement onc_api_response)
        {
            string jsonInput = JsonSerializer.Serialize(onc_api_response, new JsonSerializerOptions
            {
                WriteIndented = true
            }); 
            var systemPrompt =
                    "You are a helpful oncean network canada assistant that interprets the data given and answers the user prompt with accuracy.";

            string fullPrompt = $"{prompt}\n\nHere is the ONC API response:\n{jsonInput}";
            var payload = new
            {
                model = _modelBig,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = fullPrompt }
                },
                stream = false
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

            return result ?? "No response from Hugging Face model.";
        }

    }
}