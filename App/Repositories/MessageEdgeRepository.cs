using System.Threading.Tasks;
using Rift.Models;
using Rift.Repositories;

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
}