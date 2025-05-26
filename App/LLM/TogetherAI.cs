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

        // passing this in Program.cs to create an instance of TogetherAI
        public TogetherAI(IConfiguration config)
        {
            _httpClient = new HttpClient();

            // Loading API Key, endpoint, and model form appsetting.json
            _apiKey = config["LLmSettings:TogetherAI:ApiKey"]!;
            _endpoint = config["LLmSettings:TogetherAI:Endpoint"]!;
            _model = config["LLmSettings:TogetherAI:Model"]!;
        }

        // Main function which creates the cURL request and sends it to the LLM
        // Return: response from the llm
        public async Task<string> GenerateResponseAsync(string prompt)
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


            // creating a payload which will be used as the '-d' in the cURL request
            var payload = new
            {
                model = _model,
                messages = new[]
                {
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
            // returning the message received form TogetherAI, if no reponse return the default message
            return result ?? "No response from model.";
        }
    }
}
