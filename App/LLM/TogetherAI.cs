using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Rift.LLM
{
    public class TogetherAI : ILlmProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string _model;
        private readonly string _modelBig;

        // passing this in Program.cs to create an instance of TogetherAI
        public TogetherAI(IConfiguration config)
        {
            _httpClient = new HttpClient();

            // Loading API Key, endpoint, and model form appsetting.json
            _apiKey = config["LLmSettings:TogetherAI:ApiKey"]!;
            _endpoint = config["LLmSettings:TogetherAI:Endpoint"]!;
            _model = config["LLmSettings:TogetherAI:Model"]!;
            _modelBig = config["LLmSettings:TogetherAI:Model"]!;
        }

        // Main function which creates the cURL request and sends it to the LLM
        // Return: response from the llm
        public async Task<string> GenerateJSON(string prompt)
        {

            // cURL sample request:
            // curl -X POST "https://api.together.xyz/v1/chat/completions" \
            //      -H "Authorization: Bearer $TOGETHER_API_KEY" \
            //      -H "Content-Type: application/json" \
            //      -d '{
            //      	"model": "meta-llama/Meta-Llama-3.1-8B-Instruct-Turbo",
            //      	"messages": [
            //           {"role": "user", "content": "What are the top 3 things to do in New York?"}
            //      	]
            // }'


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

            // creating a payload which will be used as the '-d' in the cURL request
            var payload = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = prompt }
                }
            };

            // converting the payload into the required json format
            var json = JsonSerializer.Serialize(payload);

            // creating the HTTP request object using the endpoint loaded from appsettings.json
            // its the -X POST "endpoint" of the cURL request
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                // setting the content type and encoding json (payload) in the required format
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            // creating the Authorization: Bearer $TOGETHER_API_KEY" part in the cURL request
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            // Send the cURL POST request nd waiting for a response
            var response = await _httpClient.SendAsync(request);
            // throwing an exeception if not 200
            response.EnsureSuccessStatusCode();

            // storing the response as string
            var responseContent = await response.Content.ReadAsStringAsync();

            // converting the response into C# JSON doc
            using var doc = JsonDocument.Parse(responseContent);
            // reading and storing the message of the reponse
            var result = doc.RootElement
                            .GetProperty("choices")[0]
                            .GetProperty("message")
                            .GetProperty("content")
                            .GetString();

            Console.WriteLine($"[DEBUG] TogetherAI Endpoint: {_endpoint}");
            Console.WriteLine("[DEBUG] LLaMA JSON Output:\n" + result);
            // returning the message received form TogetherAI, if no reponse return the default message
            // return result ?? "No response from model.";
            // var parsedJson = JsonSerializer.Deserialize<JsonElement>(result);

            // Return the raw object directly, not wrapped in another string
            // return Ok(parsedJson);
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
