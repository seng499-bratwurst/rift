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

        public (string functionName, Dictionary<string, string?> functionParams) ExtractFunctionAndQueries(string functionCallName, string functionCallParams)
        {
            functionCallParams = Regex.Replace(functionCallParams, @",\s*}", "}");
            // Console.WriteLine($"Function Call Params: {functionCallParams}");
            string functionName = "";

            if (functionCallName == "scalardata_location"){
                functionName = "scalardata/location";
                // Console.WriteLine($"Function Name: {functionName}");
            }else if (functionCallName == "locations_tree"){
                functionName = "locations/tree";
                // Console.WriteLine($"Function Name: {functionName}");
            }else{
                functionName = functionCallName;
                // Console.WriteLine($"Function Name: {functionName}");
            }

            using JsonDocument innerDoc = JsonDocument.Parse(functionCallParams);
            var root = innerDoc.RootElement;

            var args = new Dictionary<string, string?>();
            foreach (var prop in root.EnumerateObject())
            {
                args[prop.Name] = prop.Value.ValueKind == JsonValueKind.Null ? null : prop.Value.ToString();
            }
            return (functionName, args);
        }

        public async Task<(string, string)> OncAPICall(string functionName, Dictionary<string, string?> functionParams, string oncApiToken)
        {
            // Console.WriteLine($"Function Name ONC API Call function: {functionName}");
            // Console.WriteLine($"Function Params ONC API Call function: {functionParams}");
            var (userURL, response) = await _oncApiClient.GetDataAsync(functionName, oncApiToken,functionParams );
            // Console.WriteLine($"ONC API Response: {response}");
            var serializedResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });

            return (userURL, serializedResponse);
        }
    }
}


