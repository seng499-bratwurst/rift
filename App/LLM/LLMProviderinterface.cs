using System.Threading.Tasks;
using System.Text.Json;
using Rift.App.Models;

namespace Rift.LLM
{
    public interface ILlmProvider
    {
        Task<string> GatherOncAPIData(string prompt);
        Task<string> GenerateFinalResponse(string prompt, JsonElement onc_api_response);
        IAsyncEnumerable<string> GenerateFinalResponseRAG(Prompt prompt);
    }
}