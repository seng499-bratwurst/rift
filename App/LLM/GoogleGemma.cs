using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using Rift.App.Models;
using Rift.LLM.FunctionCalls;

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
        private readonly string _oncModelName;
        private readonly string _finalModelName;
        private readonly OncFunctionParser _parser;
        // private readonly OncAPI _oncApiClient;

        /// <summary>
        /// Constructor reads values from appsettings.json
        /// </summary>
        public GoogleGemma(IConfiguration config, OncFunctionParser parser)
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(5); // Add timeout for streaming requests
            _apiKey = config["LLMSettings:GoogleGemma:ApiKey"] ?? throw new ArgumentNullException("GoogleGemma ApiKey missing in config");
            _endpoint = config["LLMSettings:GoogleGemma:Endpoint"] ?? throw new ArgumentNullException("GoogleGemma Endpoint missing in config");
            _oncModelName = config["LLMSettings:GoogleGemma:ONCModelName"] ?? "gemini-2.5-flash";
            _finalModelName = config["LLMSettings:GoogleGemma:FinalModelName"] ?? "gemini-2.5-pro";
            _parser = parser;
        }

        /// <summary>
        /// Sends a the user prompt to the google 2.5 flash to generate an ONC API call if needed.
        /// </summary>
        public async Task<string> GatherOncAPIData(string prompt, string? oncApiToken)
        {
            // read the system prompt from the file
            string systemPrompt;
            systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "onc_sys_prompt.md"));
            
            // Current date and time in the format of yyyy-MM-ddTHH:mm:ss.fffZ
            string currentDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            // Console.WriteLine("$[DEBUG] Current Date and Time: " + currentDate);

            // create the messages list for function call looping and adding the current date and time to the system prompt
            var messages = new List<object>{
                new { role = "system", content = systemPrompt+$"\n\nCurrent Date and Time: {currentDate}" },
                new { role = "user", content = prompt }
            };

            // infinite loop through the messages list and call the ONC API if needed, only break out of the loop when the LLM thinks the user prompt is answered
            // tools: scalardata_location, locations_tree, deployments, devices, properties
            while (true){
                var payload = new
                {
                    // LLM Model: google 2.5 flash
                    model = _oncModelName,
                    // messages: the user prompt and the assistant's response (list)
                    messages = messages,
                    // tools are defined in the Tools.cs file under the FunctionCalls
                    tools = Tools.GetTools(),
                    // auto is the default choice for the LLM to decide when and which tool to call or not call at any tool
                    tool_choice = "auto",
                    // using the temperature as 0.1 to make the LLM more reliable for tool calls
                    temperature = 0.1
                };

                // creating the curl request and sending a post request 
                var json = JsonSerializer.Serialize(payload);
                var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                var response = await _httpClient.SendAsync(request);
                // Console.WriteLine($"Response: {response}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException("Failed to Generate ONC data using small LLM:\n" +
                    $"Status Code:{response.StatusCode}\nError: {errorContent}\n");
                }
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                // Console.WriteLine($"Response Content: {responseContent}");

                using var doc = JsonDocument.Parse(responseContent);
                var message = doc.RootElement.GetProperty("choices")[0].GetProperty("message");
              
                // if the response has tool_calls, then we need to call the ONC API
                if (message.TryGetProperty("tool_calls", out var toolCalls) && toolCalls.GetArrayLength() > 0){

                    string? functionCallName = toolCalls[0].GetProperty("function").GetProperty("name").GetString() ?? throw new Exception("Function Call Name is null");
                    string? functionCallParams = toolCalls[0].GetProperty("function").GetProperty("arguments").GetString() ?? throw new Exception("Function Call Params is null");
                    
                    // adding the tool call to the messages list (as the assistant)
                    messages.Add(new {
                        role = "assistant",
                        tool_calls = new[]
                        {
                            new {
                            function = new {
                                arguments = functionCallParams,
                                name = functionCallName,
                            },
                            id = functionCallName,
                            type = "function"
                            }
                        }
                    });

                    // extracting the function name and parameters from the tool call
                    var (functionName, functionParams) = _parser.ExtractFunctionAndQueries(functionCallName, functionCallParams);
                    // Console.WriteLine($"Function Name: {functionName}");
                    // Console.WriteLine($"Function Params: {functionParams}");

                    // calling the ONC API
                    var (userURL, result) = await _parser.OncAPICall(functionName, functionParams, oncApiToken?? string.Empty);
                    result = result + $"\n\nHere is the user URL: {userURL}";
                    // Console.WriteLine("userURL from google.cs file: " + userURL);

                    // adding the result to the messages list (as the tool)
                    messages.Add(new {
                        tool_call_id = functionCallName,
                        role = "tool",
                        name = functionCallName,
                        content = result
                    });

                    // continue the loop
                    continue;
                    
                }
                // if the response has content, it means the LLM has answered the user prompt and no more tool calls are needed
                else if (message.TryGetProperty("content", out var contentElement))
                {
                    string? content = contentElement.GetString() ?? throw new Exception("Content is null");
                    // Console.WriteLine($"Content: {content}");

                    // creating the general response object 
                    var generalResponse = new {
                        response = content,
                        message = "Response from the ONC API Assistant."
                    } ?? throw new Exception("General Response is null");

                    Console.WriteLine("General Response: " + generalResponse.response.ToString());

                    // returning the general response in json format
                    Console.WriteLine("General Response: " + JsonSerializer.Serialize(generalResponse));
                    return JsonSerializer.Serialize(generalResponse);
                }
            };
        }

        // / <summary>
        // / Sends a prompt and ONC API response to the Gemma 3n model to generate a final user-facing answer.
        // / </summary>
        // / !!! DEPRECATED  !!!
        // / The message pipeline from the frontend uses the GenerateFinalResponseRAG method instead.
        // / Just keeping this so Ishan is happy :)
        public async Task<string> GenerateFinalResponse(string prompt, JsonElement onc_api_response)
        {
            string jsonInput = JsonSerializer.Serialize(onc_api_response, new JsonSerializerOptions { WriteIndented = true });
            var systemPrompt = "You are a helpful Ocean Networks Canada assistant that interprets the data given and answers the user prompt with accuracy.";

            string fullPrompt = $"{prompt}\n\nHere is the ONC API response:\n{jsonInput}\n\nReturn the most revelant user URL based on the user prompt (if there are any) otherwise dont mention it.";

            var payload = new
            {
                model = _finalModelName,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
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

        /// <summary>
        /// Sends a prompt, chat history, relevant documents, and ONC API response to the Gemma 3n model to generate a final user-facing answer.
        /// </summary>
        public async Task<string> GenerateFinalResponseRAG(Prompt prompt)
        {

            var payload = new
            {
                model = _finalModelName,
                messages = prompt.Messages,
                stream = false,
                temperature = 0.5 // Answers seem a bit more reliable with lower temperature
            };

            var json = JsonSerializer.Serialize(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Response: {errorContent}");
                throw new HttpRequestException("Failed to Generate response from Large LLM:\n" +
                $"Status Code:{response.StatusCode}\nError: {errorContent}\n");
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseContent);
            var result = doc.RootElement
                            .GetProperty("choices")[0]
                            .GetProperty("message")
                            .GetProperty("content")
                            .GetString();

            Console.WriteLine("Final Response: " + result);

            return result ?? "No response from Gemma model.";
        }

        public async IAsyncEnumerable<string> StreamFinalResponseRAG(Prompt prompt)
        {
            var payload = new
            {
                model = _finalModelName,
                messages = prompt.Messages,
                stream = true,
                temperature = 0.5 // Same temperature as non-streaming version
            };

            var json = JsonSerializer.Serialize(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Streaming failed: {response.StatusCode} - {errorContent}");
            }

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
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

        public async IAsyncEnumerable<string> StreamFinalResponse(string prompt, JsonElement onc_api_response)
        {
            string jsonInput = JsonSerializer.Serialize(onc_api_response, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            
            // read the system prompt from the file
            string systemPrompt;
            systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "onc_sys_prompt.md"));
            
            // Current date and time in the format of yyyy-MM-ddTHH:mm:ss.fffZ
            string currentDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            // Console.WriteLine("$[DEBUG] Current Date and Time: " + currentDate);

            string fullPrompt = $"{prompt}\n\nHere is the ONC API response:\n{jsonInput}\n\nReturn the most revelant user URL based on the user prompt (if there are any) otherwise dont mention it.";
            
            await foreach (var chunk in StreamResponse(_finalModelName, systemPrompt + $"\n\nCurrent Date and Time: {currentDate}", fullPrompt))
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
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Streaming failed: {response.StatusCode} - {errorContent}");
            }

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