using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using Rift.App.Models;


namespace Rift.LLM
{
    public class HuggingFace : ILlmProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string _modelSmall;
        private readonly string _modelBig;
        private readonly OncFunctionParser _parser;

        // Constructor reads values from appsettings.json
        public HuggingFace(IConfiguration config, OncFunctionParser parser)
        {
            _httpClient = new HttpClient();

            // Loading API Key, endpoint, and model from appsettings.json
            _apiKey = config["LLmSettings:HuggingFace:ApiKey"]!;
            _endpoint = config["LLmSettings:HuggingFace:Endpoint"]!;
            _modelSmall = config["LLmSettings:HuggingFace:ModelSmall"]!;
            _modelBig = config["LLmSettings:HuggingFace:ModelBig"]!;
            _parser = parser;
        }

        // Sends a chat completion request to Hugging Face endpoint
        public async Task<string> GatherOncAPIData(string prompt, string? oncApiToken)
        {
            // Equivalent cURL:
            // curl https://router.huggingface.co/together/v1/chat/completions \
            // -H 'Authorization: Bearer hf_xxx' \
            // -H 'Content-Type: application/json' \
            // -d '{ "messages": [{ "role": "user", "content": "..." }], "model": "...", "stream": false }'

            string systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "function_call_required_or_not.md"));

            var payload = new
            {
                model = _modelSmall,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
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
            // Console.WriteLine("response: "+response);
            // Console.WriteLine("response.IsSuccessStatusCode: "+response.IsSuccessStatusCode);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseContent);


            var message = doc.RootElement.GetProperty("choices")[0].GetProperty("message");

            string LLMContent = message.GetProperty("content").GetString() ?? string.Empty;
            // Console.WriteLine("LLMContent: "+LLMContent);

            object? generalResponse = null;
            var match = Regex.Match(LLMContent, @"\{(?:[^{}]|(?<open>\{)|(?<-open>\}))*\}(?(open)(?!))", RegexOptions.Singleline);
            if (!match.Success)
            {
                generalResponse = new
                {
                    details = message.GetProperty("content").GetString(),
                    message = "ONC API call not required. Answer based on the user prompt."
                };
            }else{
                var LLMContentFiltered = match.Value;
                // Console.WriteLine("LLMContentFiltered: "+ LLMContentFiltered);


                using JsonDocument innerDoc = JsonDocument.Parse(LLMContentFiltered);

                bool useFunction = innerDoc.RootElement.GetProperty("use_function").GetBoolean();

                if (!useFunction)
                {
                    return "{}";
                }
                else if (useFunction)
                {
                    // var (functionName, functionParams) = _parser.ExtractFunctionAndQueries(LLMContentFiltered);
                    
                    // return await _parser.OncAPICall(functionName, functionParams);
                }
            }

            return JsonSerializer.Serialize(generalResponse);
        }


        public async Task<string> GenerateFinalResponse(string prompt, JsonElement oncAPIResponse)
        {
            string jsonInput = JsonSerializer.Serialize(oncAPIResponse, new JsonSerializerOptions
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
            // Console.WriteLine("response: "+response);
            // Console.WriteLine("response.IsSuccessStatusCode: "+response.IsSuccessStatusCode);
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
        

        public async Task<string> GenerateFinalResponseRAG(Prompt prompt)
        {

            // I think eventually we should move this into the PromptBuilder class but 
            // this should be okay for now. We will need to figure out the best way to
            // feed all of this data into the LLM.
            // ----------------------------------------------
            // var fullUserPrompt = new StringBuilder();
            // fullUserPrompt.Append("This is the User Query:\n");
            // fullUserPrompt.Append(prompt.UserQuery);
            // fullUserPrompt.Append("\n\nHere is the ONC API response:\n");
            // fullUserPrompt.Append(prompt.OncAPIData);
            // fullUserPrompt.Append("\n\nHere are relevant documents :\n");
            // foreach (var relevantDoc in prompt.RelevantDocuments)
            // {
            //     fullUserPrompt.Append($"- {relevantDoc}\n");
            // }
            // fullUserPrompt.Append("\n\nHere is the message history:\n");
            // foreach (var message in prompt.MessageHistory)
            // {
            //     fullUserPrompt.Append($"- {message.Content}\n");
            // }
            // ----------------------------------------------

            var payload = new
            {
                model = _modelBig,
                messages = new[]
                {
                    new { role = "system", content = prompt.SystemPrompt },
                    new { role = "user", content = prompt.UserQuery },
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