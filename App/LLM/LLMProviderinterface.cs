using System.Threading.Tasks;
using System.Text.Json;
using Rift.App.Models;

namespace Rift.LLM
{
    public interface ILlmProvider
    {
        Task<string> GatherOncAPIData(string prompt, string? oncApiToken);
        Task<string> GenerateFinalResponse(string prompt, JsonElement onc_api_response);
        Task<string> GenerateFinalResponseRAG(Prompt prompt);
        IAsyncEnumerable<string> StreamFinalResponseRAG(Prompt prompt);
        IAsyncEnumerable<string> StreamFinalResponse(string prompt, JsonElement onc_api_response);
    }
}