using System.Text.Json;
using System.Text.RegularExpressions;

namespace Rift.LLM
{
    public class OncFunctionParser
    {
        private readonly OncAPI _oncApiClient;

        public OncFunctionParser(OncAPI oncApiClient)
        {
            _oncApiClient = oncApiClient;
        }

        public (string functionName, Dictionary<string, string?> functionParams) ExtractFunctionAndQueries(string LLMContent)
        {
            LLMContent = Regex.Replace(LLMContent, @",\s*}", "}");
            LLMContent = Regex.Replace(LLMContent, @",\s*]", "]");

            using JsonDocument innerDoc = JsonDocument.Parse(LLMContent);
            var root = innerDoc.RootElement;

            string functionName = root.GetProperty("function").GetString() ?? string.Empty;

            var argsElement = root.GetProperty("args");

            var args = new Dictionary<string, string?>();
            foreach (var prop in argsElement.EnumerateObject())
            {
                args[prop.Name] = prop.Value.ValueKind == JsonValueKind.Null ? null : prop.Value.ToString();
            }
            return (functionName, args);
        }

        public async Task<string> OncAPICall(string functionName, Dictionary<string, string?> functionParams)
        {

            var response = await _oncApiClient.GetDataAsync(functionName, functionParams);
            var serializedResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            
            Console.WriteLine($"ONC API Response: {serializedResponse}");

            return serializedResponse;
        }
    }
}


