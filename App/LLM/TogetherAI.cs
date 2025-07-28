using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Rift.App.Models;
using System.Runtime.CompilerServices;

namespace Rift.LLM
{
    public class TogetherAI : ILlmProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string _modelSmall;
        private readonly string _modelBig;
        private readonly string _ONCToken;

        // Constructor reads values from appsettings.json
        public TogetherAI(IConfiguration config)
        {
            _httpClient = new HttpClient();

            // Loading API Key, endpoint, and model from appsettings.json
            _apiKey = config["LLmSettings:HuggingFace:ApiKey"]!;
            _endpoint = config["LLmSettings:HuggingFace:Endpoint"]!;
            _modelSmall = config["LLmSettings:HuggingFace:ModelSmall"]!;
            _modelBig = config["LLmSettings:HuggingFace:ModelBig"]!;
            _ONCToken = config["ONC_TOKEN"]!;
        }

        // public string GatherONCAPIData(string userQuery)
        // {
        //     throw new NotImplementedException();
        // }

        // Sends a chat completion request to Hugging Face endpoint
        public async Task<string> GatherOncAPIData(string prompt, string? oncApiToken)
        {
            // Equivalent cURL:
            // curl https://router.huggingface.co/together/v1/chat/completions \
            // -H 'Authorization: Bearer hf_xxx' \
            // -H 'Content-Type: application/json' \
            // -d '{ "messages": [{ "role": "user", "content": "..." }], "model": "...", "stream": false }'

            // string file_path = "App\LLM\sys_prompt_small_llm.md";
            string system_Prompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "sys_prompt_small_llm.md"));

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



            Console.WriteLine(result);
            Console.WriteLine($"[DEBUG] Hugging Face Endpoint: {_endpoint}");

            // return result ?? "No response from Hugging Face model.";


            return result ?? "{}";
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

        public async Task<string> GenerateFinalResponseRAG(Prompt prompt)
        {
            await Task.Delay(100); // Simulate async operation
            throw new NotImplementedException("RAG functionality is not implemented in TogetherAI.");
        }

        public async IAsyncEnumerable<string> StreamFinalResponseRAG(Prompt prompt)
        {
            string jsonInput = JsonSerializer.Serialize(prompt.RelevantDocumentChunks, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "sys_prompt_large_llm.md"));

            string fullPrompt = $"{prompt.UserQuery}\n\nHere is the relevant context:\n{jsonInput}";
            
            await foreach (var chunk in StreamResponse(_modelBig, systemPrompt, fullPrompt))
            {
                yield return chunk;
            }
        }

        public async IAsyncEnumerable<string> StreamFinalResponse(string prompt, JsonElement onc_api_response)
        {
            string jsonInput = JsonSerializer.Serialize(onc_api_response, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            var systemPrompt =
                    "You are a helpful oncean network canada assistant that interprets the data given and answers the user prompt with accuracy.";

            string fullPrompt = $"{prompt}\n\nHere is the ONC API response:\n{jsonInput}";
            
            await foreach (var chunk in StreamResponse(_modelBig, systemPrompt, fullPrompt))
            {
                yield return chunk;
            }
        }

        private async IAsyncEnumerable<string> StreamResponse(string model, string systemPrompt, string userPrompt, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                stream = true
            };

            var json = JsonSerializer.Serialize(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (cancellationToken.IsCancellationRequested)
                    yield break;

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("data: "))
                {
                    var data = line.Substring(6);
                    
                    if (data == "[DONE]")
                        yield break;

                    var contentText = ParseStreamingContent(data);
                    if (!string.IsNullOrEmpty(contentText))
                    {
                        yield return contentText;
                    }
                }
            }
        }

        private string? ParseStreamingContent(string data)
        {
            try
            {
                using var doc = JsonDocument.Parse(data);
                var choices = doc.RootElement.GetProperty("choices");
                
                if (choices.GetArrayLength() > 0)
                {
                    var choice = choices[0];
                    if (choice.TryGetProperty("delta", out var delta))
                    {
                        if (delta.TryGetProperty("content", out var content))
                        {
                            return content.GetString();
                        }
                    }
                }
            }
            catch (JsonException)
            {
                // Skip malformed JSON
            }
            return null;
        }

    }
}