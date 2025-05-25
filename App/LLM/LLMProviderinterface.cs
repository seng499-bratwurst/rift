using System.Threading.Tasks;

namespace Rift.LLM
{
    public interface ILlmProvider
    {
        Task<string> GenerateResponseAsync(string prompt);
    }
}
