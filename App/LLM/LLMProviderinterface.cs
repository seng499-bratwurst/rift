using System.Threading.Tasks;
using System.Text.Json;

namespace Rift.LLM
{
    public interface ILlmProvider
    {
        Task<string> GenerateONCAPICall(string prompt);
        Task<string> GenerateFinalResponse(string prompt, JsonElement onc_api_response);
    }
}