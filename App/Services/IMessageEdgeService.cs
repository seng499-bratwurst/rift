using Rift.Models;

namespace Rift.Services;

public interface IMessageEdgeService
{
    Task<MessageEdge> CreateEdgeAsync(MessageEdge edge);
    Task<int?> DeleteEdgeAsync(int edgeId);
}
