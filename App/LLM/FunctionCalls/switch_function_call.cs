using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json.Nodes;
using App.LLM;
using System.Text.RegularExpressions;

namespace Rift.LLM
{
    public class FunctionCallSwitch
    {
       
        private readonly Properties _oncPropertiesClient;
        private readonly Deployments _oncDeploymentClient;
        private readonly DeviceCategories _oncDeviceCategoriesClient;
        private readonly OncAPI _oncApiClient;

        public FunctionCallSwitch(
            Properties oncPropertiesClient,
            Deployments oncDeploymentClient,
            DeviceCategories oncDeviceCategoriesClient,
            OncAPI oncApiClient    
        )
        {
            _oncPropertiesClient = oncPropertiesClient!;
            _oncDeploymentClient = oncDeploymentClient!;
            _oncDeviceCategoriesClient = oncDeviceCategoriesClient!;
            _oncApiClient = oncApiClient;
        }


        public (string functionName, Dictionary<string, string?> args) ExtractFunctionAndArgsFromContent(string content_llm)
        {
            content_llm = Regex.Replace(content_llm, @",\s*}", "}");
            content_llm = Regex.Replace(content_llm, @",\s*]", "]");

            using JsonDocument innerDoc = JsonDocument.Parse(content_llm);
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

        public async Task<string> ONC_API_Call(string functionName, Dictionary<string, string?> args)
        {

            var response = await _oncApiClient.GetDataAsync(functionName, args);
            var str_respone = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            
            Console.WriteLine($"ONC API Response: {str_respone}");

            return str_respone;
            // switch (functionName)
            // {
            //     case "deviceCategories":
            //         Console.WriteLine("Device Cat switch worked");
            //         var result_device_cat = await _oncDeviceCategoriesClient.GetDeviceCategoriesAsync(
            //                     deviceCategoryCode: args.GetValueOrDefault("deviceCategoryCode"),
            //                     deviceCategoryName: args.GetValueOrDefault("deviceCategoryName"),
            //                     description: args.GetValueOrDefault("description"),
            //                     locationCode: args.GetValueOrDefault("locationCode"),
            //                     propertyCode: args.GetValueOrDefault("propertyCode")
            //                 );

            //         return JsonSerializer.Serialize(result_device_cat, new JsonSerializerOptions { WriteIndented = true });

            //     case "deployments":
            //         var result_deployments = await _oncDeploymentClient.GetDeploymentsAsync(
            //                     deviceCategoryCode: args.GetValueOrDefault("deviceCategoryCode"),
            //                     deviceCode: args.GetValueOrDefault("deviceCode"),
            //                     locationCode: args.GetValueOrDefault("locationCode"),
            //                     propertyCode: args.GetValueOrDefault("propertyCode"),
            //                     dateFrom: args.GetValueOrDefault("dateFrom"),
            //                     dateTo: args.GetValueOrDefault("dateTo")
            //                 );

            //         return JsonSerializer.Serialize(result_deployments, new JsonSerializerOptions { WriteIndented = true });

            //     case "properties":
            //         var result_properties = await _oncPropertiesClient.GetPropertiesAsync(
            //                     deviceCategoryCode: args.GetValueOrDefault("deviceCategoryCode"),
            //                     deviceCode: args.GetValueOrDefault("deviceCode"),
            //                     locationCode: args.GetValueOrDefault("locationCode"),
            //                     propertyCode: args.GetValueOrDefault("propertyCode"),
            //                     description: args.GetValueOrDefault("description"),
            //                     propertyName: args.GetValueOrDefault("propertyName")
            //                 );

            //         return JsonSerializer.Serialize(result_properties, new JsonSerializerOptions { WriteIndented = true });

            //     default:
            //         return "{}"; 
            // }
        }



    }
}


