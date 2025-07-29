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

    public async Task<List<MessageEdge>> CreateMessageEdgesFromSourcesAsync(int targetMessageId, PartialMessageEdge[] sources)
    {
        var edges = new List<MessageEdge>();
        foreach (var src in sources)
        {
            // Skip invalid source message IDs to prevent foreign key constraint violations
            if (src.SourceMessageId <= 0)
                continue;

            var edge = new MessageEdge
            {
                SourceMessageId = src.SourceMessageId,
                TargetMessageId = targetMessageId,
                SourceHandle = src.SourceHandle,
                TargetHandle = src.TargetHandle,
            };
            edges.Add(edge);
        }

        // If no valid edges, return empty list without calling repository
        if (!edges.Any())
            return new List<MessageEdge>();

        return await _edgeRepository.AddEdgesAsync(edges.ToArray());
    }
    public async Task<List<MessageEdge>> GetEdgesForConversationAsync(string userId, int conversationId)
    {
        return await _edgeRepository.GetEdgesForConversationAsync(userId, conversationId);
    }
}
