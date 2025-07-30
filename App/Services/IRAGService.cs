using Rift.App.Models;
using Rift.Models;

public interface IRAGService
{
    // public Task<string> GenerateResponseAsync(string userQuery, List<Message>? messageHistory);
    public Task<(string cleanedResponse, List<DocumentChunk> relevantDocs)> GenerateResponseAsync(string userQuery, List<Message>? messageHistory, string? oncApiToken);
    public IAsyncEnumerable<(string contentChunk, List<DocumentChunk> relevantDocs)> StreamResponseAsync(string userQuery, List<Message>? messageHistory, string? oncApiToken);
}