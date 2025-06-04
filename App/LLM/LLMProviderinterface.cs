using System.Threading.Tasks;

namespace Rift.LLM
{
    public interface ILlmProvider
    {
        string GatherONCAPIData(string userQuery);
        Task<string> GenerateResponseAsync(string prompt);
    }
}
