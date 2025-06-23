using Rift.Models;
using Microsoft.EntityFrameworkCore;

namespace Rift.Repositories;

public class MessageEdgeRepository : IMessageEdgeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MessageEdgeRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<MessageEdge> AddEdgeAsync(MessageEdge edge)
    {
        _dbContext.MessageEdges.Add(edge);
        await _dbContext.SaveChangesAsync();
        return edge;
    }

    public async Task<List<MessageEdge>> AddEdgesAsync(MessageEdge[] edges)
    {
        _dbContext.MessageEdges.AddRange(edges);
        await _dbContext.SaveChangesAsync();
        return edges.ToList();
    }


    public async Task<int?> RemoveEdgeAsync(int edgeId)
    {
        var edge = await _dbContext.MessageEdges.FindAsync(edgeId);

        if (edge != null)
        {
            _dbContext.MessageEdges.Remove(edge);
            await _dbContext.SaveChangesAsync();
        }
        return edge?.Id;
    }
    
    public async Task<List<MessageEdge>> GetEdgesForConversationAsync(string userId, int conversationId)
    {
        var messageIds = await _dbContext.Messages
            .Where(m => m.ConversationId == conversationId)
            .Select(m => m.Id)
            .ToListAsync();

        var edges = await _dbContext.MessageEdges
            .Where(e => messageIds.Contains(e.SourceMessageId) || messageIds.Contains(e.TargetMessageId))
            .ToListAsync();

        return edges;
    }
}