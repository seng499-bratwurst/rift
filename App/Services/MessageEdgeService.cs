using Rift.Models;
using Rift.Repositories;

namespace Rift.Services;
public class MessageEdgeService : IMessageEdgeService
{
    private readonly IMessageEdgeRepository _edgeRepository;

    public MessageEdgeService(IMessageEdgeRepository edgeRepository)
    {
        _edgeRepository = edgeRepository;
    }

    public async Task<MessageEdge> CreateEdgeAsync(MessageEdge edge)
    {
        return await _edgeRepository.AddEdgeAsync(edge);
    }

    public async Task<int?> DeleteEdgeAsync(int edgeId)
    {
        return await _edgeRepository.RemoveEdgeAsync(edgeId);
    }
}
