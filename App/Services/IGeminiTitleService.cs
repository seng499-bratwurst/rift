using System.Threading.Tasks;

namespace Rift.Services
{
    public interface IGeminiTitleService
    {
        Task<string> GenerateTitleAsync(string userPrompt, string assistantResponse);
    }
}
