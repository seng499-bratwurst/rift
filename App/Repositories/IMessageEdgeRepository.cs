using Rift.Models;

namespace Rift.Repositories;

public interface IMessageEdgeRepository
{
    Task<MessageEdge> AddEdgeAsync(MessageEdge edge);
    Task<List<MessageEdge>> AddEdgesAsync(MessageEdge[] edge);

    Task<int?> RemoveEdgeAsync(int edgeId);
}
