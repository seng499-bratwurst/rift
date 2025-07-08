using Rift.Models;

public interface IRAGService
{
    public IAsyncEnumerable<string> GenerateResponseAsync(string userQuery, List<Message>? messageHistory);
}