using Rift.Models;

public interface IRAGService
{
    // public Task<string> GenerateResponseAsync(string userQuery, List<Message>? messageHistory);
    public Task<(string cleanedResponse, List<string> relevantDocTitles)> GenerateResponseAsync(string userQuery, List<Message>? messageHistory);
}