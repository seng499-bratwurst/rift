using System.Threading.Tasks;
using System.Text.Json;

namespace Rift.LLM
{
    public interface ILlmProvider
    {
        Task<string> GenerateJSON(string prompt);
        Task<string> GenerateFinalResponse(string prompt, JsonElement onc_api_response);
        // string GatherONCAPIData(string userQuery);
    }
}