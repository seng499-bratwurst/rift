using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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

        // Constructor reads values from appsettings.json
        public HuggingFace(IConfiguration config)
        {
            _httpClient = new HttpClient();

            // Loading API Key, endpoint, and model from appsettings.json
            _apiKey = config["LLmSettings:HuggingFace:ApiKey"]!;
            _endpoint = config["LLmSettings:HuggingFace:Endpoint"]!;
            _modelSmall = config["LLmSettings:HuggingFace:ModelSmall"]!;
            _modelBig = config["LLmSettings:HuggingFace:ModelBig"]!;
            _ONCToken = config["ONC_TOKEN"]!;
        }

        // Sends a chat completion request to Hugging Face endpoint
        public async Task<string> GenerateJSON(string prompt)
        {
            // Equivalent cURL:
            // curl https://router.huggingface.co/together/v1/chat/completions \
            // -H 'Authorization: Bearer hf_xxx' \
            // -H 'Content-Type: application/json' \
            // -d '{ "messages": [{ "role": "user", "content": "..." }], "model": "...", "stream": false }'

            var systemPrompt =
                    "You are an API assistant for Ocean Networks Canada (ONC). Your task is to convert natural language requests into a clean JSON object that contains only the necessary fields to build an ONC Web Service API request.\n\n" +
                    "Respond ONLY with raw JSON. Do not include any explanation, markdown, or curl commands.\n\n" +
                    "The JSON object must include:\n" +
                    "- \"service\": either \"locations\" or \"data\" — this determines which ONC API endpoint should be used\n" +
                    "- \"method\": always \"get\"\n" +
                    "- \"token\": always \"token\" (as a placeholder string)\n" +
                    "- Add optional fields only if mentioned by the user:\n" +
                    "  - locationName (e.g., \"Cambridge Bay\") — keep original casing and spacing\n" +
                    "  - locationCode (e.g., \"CAMBAY\")\n" +
                    "  - deviceCategoryCode (e.g., \"CTD\")\n" +
                    "  - propertyCode (e.g., \"temperature\", \"salinity\")\n" +
                    "  - dataProductCode (e.g., \"CPID\")\n" +
                    "  - depth (number in meters)\n" +
                    "  - dateFrom and dateTo in ISO 8601 format: \"yyyy-MM-ddTHH:mm:ss.000Z\"\n" +
                    "  - includeChildren (true or false)\n" +
                    "  - aggregateDeployments (true or false)\n\n" +
                    "All field names must be in camelCase.\n\n" +
                    "Example input:\n" +
                    "\"Get salinity in Cambridge Bay from Jan 1 to Jan 5, 2020\"\n\n" +
                    "Expected output:\n" +
                    "{\n" +
                    "  \"service\": \"data\",\n" +
                    "  \"method\": \"get\",\n" +
                    "  \"token\": \"token\",\n" +
                    "  \"locationName\": \"Cambridge Bay\",\n" +
                    "  \"propertyCode\": \"salinity\",\n" +
                    "  \"dateFrom\": \"2020-01-01T00:00:00.000Z\",\n" +
                    "  \"dateTo\": \"2020-01-05T00:00:00.000Z\"\n" +
                    "}";

            var payload = new
            {
                model = _modelSmall,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
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

    }
}
