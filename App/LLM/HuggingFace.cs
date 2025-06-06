using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json.Nodes;
using App.LLM;


namespace Rift.LLM
{
    public class HuggingFace : ILlmProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string _modelSmall;
        private readonly string _modelBig;
        private readonly string _ONCToken;
        private readonly OncAPI _oncApiClient;

        // Constructor reads values from appsettings.json
        public HuggingFace(IConfiguration config, OncAPI oncApiClient)
        {
            _httpClient = new HttpClient();

            // Loading API Key, endpoint, and model from appsettings.json
            _apiKey = config["LLmSettings:HuggingFace:ApiKey"]!;
            _endpoint = config["LLmSettings:HuggingFace:Endpoint"]!;
            _modelSmall = config["LLmSettings:HuggingFace:ModelSmall"]!;
            _modelBig = config["LLmSettings:HuggingFace:ModelBig"]!;
            _ONCToken = config["ONC_TOKEN"]!;
            _oncApiClient = oncApiClient;
        }

    

        // Sends a chat completion request to Hugging Face endpoint
        public async Task<string> GenerateONCAPICall(string prompt)
        {
            // Equivalent cURL:
            // curl https://router.huggingface.co/together/v1/chat/completions \
            // -H 'Authorization: Bearer hf_xxx' \
            // -H 'Content-Type: application/json' \
            // -d '{ "messages": [{ "role": "user", "content": "..." }], "model": "...", "stream": false }'

            // string file_path = "App\LLM\sys_prompt_small_llm.md"; 
            string system_Prompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "function_call_required_or_not.md"));
            Console.WriteLine("[DEBUG] function called generateonc api call");
            var func_call = new JsonArray
            {
                FunctionSchemas.deviceCategories
            };

            // Console.WriteLine("=== System Prompt Start ===");
            // Console.WriteLine(system_Prompt);
            // Console.WriteLine("=== System Prompt End ===");

            var payload = new
            {
                model = _modelSmall,
                messages = new[]
                {
                    new { role = "system", content = system_Prompt },
                    new { role = "user", content = prompt }
                },
                stream = false,
                // tools = func_call
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

            // var result = doc.RootElement
            //                 .GetProperty("choices")[0]
            //                 .GetProperty("message")
            //                 .GetProperty("content")
            //                 .GetString();

            var message = doc.RootElement.GetProperty("choices")[0].GetProperty("message");
            Console.WriteLine("[DEBUG] message"+message.ToString());

            // Handle function_call
           if (message.TryGetProperty("tool_calls", out var toolCalls) && toolCalls.ValueKind == JsonValueKind.Array)
            {
                foreach (var call in toolCalls.EnumerateArray())
                {
                    var function = call.GetProperty("function");
                    var name = function.GetProperty("name").GetString();
                    
                    // arguments is a JSON string → parse it
                    var rawArgs = function.GetProperty("arguments").GetString();
                    var args = JsonDocument.Parse(rawArgs!).RootElement;

                    Console.WriteLine("[DEBUG] Tool function: " + name);
                    Console.WriteLine("[DEBUG] Arguments: " + rawArgs);

                    if (name == "deviceCategories")
                    {
                        // Helper to extract optional args
                        string? GetArg(JsonElement e, string key) =>
                            e.TryGetProperty(key, out var val) ? val.GetString() : null;

                        var result = await _oncApiClient.GetDeviceCategoriesAsync(
                            deviceCategoryCode: GetArg(args, "deviceCategoryCode"),
                            deviceCategoryName: GetArg(args, "deviceCategoryName"),
                            description: GetArg(args, "description"),
                            locationCode: GetArg(args, "locationCode"),
                            propertyCode: GetArg(args, "propertyCode")
                        );

                        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
                    }
                }
            }


            // Fallback to content
            var resultContent = message.GetProperty("content").GetString();


            Console.WriteLine(resultContent);
            Console.WriteLine($"[DEBUG] Hugging Face Endpoint: {_endpoint}");

            // return result ?? "No response from Hugging Face model.";


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


            Console.WriteLine($"[DEBUG] Hugging Face Endpoint: {_endpoint}");

            return result ?? "No response from Hugging Face model.";
            // return result ?? "{}";
        }
    }
}