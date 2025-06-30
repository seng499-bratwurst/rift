using Rift.Models;

public interface IRAGService
{
    public Task<string> GenerateResponseAsync(string userQuery, List<Message>? messageHistory);
}