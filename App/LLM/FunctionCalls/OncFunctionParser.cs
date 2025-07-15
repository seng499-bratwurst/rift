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
            Console.WriteLine($"Function Call Params: {functionCallParams}");
            // functionCallParams = Regex.Replace(functionCallParams, @",\s*]", "]");
            string functionName = "";

            if (functionCallName == "scalardata_location"){
                functionName = "scalardata/location";
                Console.WriteLine($"Function Name: {functionName}");
            }else if (functionCallName == "locations_tree"){
                functionName = "locations/tree";
                Console.WriteLine($"Function Name: {functionName}");
            }else{
                functionName = functionCallName;
                // Console.WriteLine($"Function Name: {functionName}");
            }

            using JsonDocument innerDoc = JsonDocument.Parse(functionCallParams);
            // Console.WriteLine($"Inner Doc: {innerDoc.RootElement}");
            var root = innerDoc.RootElement;

            // string functionName = root.GetProperty("function").GetString() ?? string.Empty;

            // var argsElement = root.GetProperty("args");
            // string argsElement = functionCallParams;

            var args = new Dictionary<string, string?>();
            foreach (var prop in root.EnumerateObject())
            {
                args[prop.Name] = prop.Value.ValueKind == JsonValueKind.Null ? null : prop.Value.ToString();
            }
            return (functionName, args);
        }

        public async Task<string> OncAPICall(string functionName, Dictionary<string, string?> functionParams)
        {
            // Console.WriteLine($"Function Name ONC API Call function: {functionName}");
            // Console.WriteLine($"Function Params ONC API Call function: {functionParams}");
            var response = await _oncApiClient.GetDataAsync(functionName, functionParams);
            // Console.WriteLine($"ONC API Response: {response}");
            var serializedResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });

            if (functionName == "rawdata/device"){
                return ProcessSerializedResponse(serializedResponse);
            }
            
            return serializedResponse;
        }
        private string ProcessSerializedResponse(string serializedResponse)
        {
            using var doc = JsonDocument.Parse(serializedResponse);
            var root = doc.RootElement;

            var rootDict = new Dictionary<string, object>();

            // Copying everything except 'data'
            foreach (var prop in root.EnumerateObject())
            {
                if (prop.Name != "data")
                {
                    rootDict[prop.Name] = prop.Value.Clone();
                }
            }

            // getting rid of the lineTypes property
            if (root.TryGetProperty("data", out var dataElement))
            {
                var dataDict = new Dictionary<string, object>();
                foreach (var dataProp in dataElement.EnumerateObject())
                {
                    if (dataProp.Name != "lineTypes")
                    {
                        dataDict[dataProp.Name] = dataProp.Value.Clone();
                    }
                }
                rootDict["data"] = dataDict;
            }

            // converting back to JSON
            var options = new JsonSerializerOptions { WriteIndented = true };
            string result = JsonSerializer.Serialize(rootDict, options);
            // Console.WriteLine("result without LineTypes: "+result);
            return result;
        }
    }
}


