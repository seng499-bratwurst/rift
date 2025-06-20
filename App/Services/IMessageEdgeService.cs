using Rift.Models;

namespace Rift.Services;

public interface IMessageEdgeService
{
    Task<MessageEdge> CreateEdgeAsync(MessageEdge edge);
    Task<int?> DeleteEdgeAsync(int edgeId);
    Task<List<MessageEdge>> CreateMessageEdgesFromSourcesAsync(int targetMessageId, PartialMessageEdge[] sources);
    Task<List<MessageEdge>> GetEdgesForConversationAsync(string userId, int conversationId);
}
