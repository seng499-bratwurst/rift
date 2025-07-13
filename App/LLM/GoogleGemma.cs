using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using Rift.App.Models;

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
            _oncModelName = config["LLMSettings:GoogleGemma:ONCModelName"] ?? "gemma-3n-e4b-it";
            _finalModelName = config["LLMSettings:GoogleGemma:FinalModelName"] ?? "gemini-2.5-flash";
            _parser = parser;
        }

        /// <summary>
        /// Sends a prompt to the Gemma 3n model to generate an ONC API call.
        /// </summary>
        public async Task<string> GatherOncAPIData(string prompt)
        {
             string systemPrompt = "";

             // All property codes from the Cambridge Bay observatory
            string[] propertyCodes = {
                "absolutebarometricpressure",
                "absolutehumidity", 
                "airdensity",
                "airtemperature",
                "dewpoint",
                "magneticheading",
                "mixingratio",
                "relativebarometricpressure",
                "relativehumidity",
                "solarradiation",
                "specificenthalpy",
                "wetbulbtemperature",
                "windchilltemperature",
                "winddirection",
                "windspeed",
                "conductivity",
                "density",
                "oxygen",
                "pressure",
                "salinity",
                "seawatertemperature",
                "soundspeed",
                "turbidityntu",
                "chlorophyll",
                "icedraft",
                "parphotonbased",
                "ph",
                "sigmatheta"
            };

            string pattern = @"\b(" + string.Join("|", propertyCodes) + @")\b";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            bool hasPropertyCode = regex.IsMatch(prompt);

            if (hasPropertyCode){
                // Console.WriteLine("using filter4.md");
                systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "filter4.md"));
            }else{
                // Console.WriteLine("using function_call_required_or_not.md");
                systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "function_call_required_or_not.md"));
            }
            var payload = new
            {
                model = _oncModelName,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
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
    
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException("Failed to Generate ONC data using small LLM:\n" +
                $"Status Code:{response.StatusCode}\nError: {errorContent}\n");
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

            Console.WriteLine($"Content: {content}");

    
            var match = Regex.Match(content!, @"\{(?:[^{}]|(?<open>\{)|(?<-open>\}))*\}(?(open)(?!))", RegexOptions.Singleline);

            object? generalResponse = null;

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
        /// !!! DEPRECATED !!!
        /// The message pipeline from the frontend uses the GenerateFinalResponseRAG method instead.
        /// Just keeping this so Ishan is happy :)
        public async Task<string> GenerateFinalResponse(string prompt, JsonElement onc_api_response)
        {
            string jsonInput = JsonSerializer.Serialize(onc_api_response, new JsonSerializerOptions { WriteIndented = true });
            var systemPrompt = "You are a helpful Ocean Networks Canada assistant that interprets the data given and answers the user prompt with accuracy.";

            string fullPrompt = $"{prompt}\n\nHere is the ONC API response:\n{jsonInput}";

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

            return result ?? "No response from Gemma model.";
        }

    }
}
