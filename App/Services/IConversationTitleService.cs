namespace Rift.Services
{
    public interface IConversationTitleService
    {
        Task<string> GenerateTitleAsync(string userPrompt, string assistantResponse);
        Task<bool> UpdateConversationTitleAsync(int conversationId, string title);
    }
}
