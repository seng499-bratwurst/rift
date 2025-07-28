using Rift.Models;

public interface IRAGService
{
    // public Task<string> GenerateResponseAsync(string userQuery, List<Message>? messageHistory);
    public Task<(string cleanedResponse, List<string> relevantDocTitles)> GenerateResponseAsync(string userQuery, List<Message>? messageHistory, string? oncApiToken);
    public IAsyncEnumerable<(string contentChunk, List<string> relevantDocTitles)> StreamResponseAsync(string userQuery, List<Message>? messageHistory, string? oncApiToken);
}