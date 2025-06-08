namespace Rift.Repositories;

using Microsoft.EntityFrameworkCore;
using Rift.Models;

public class ConversationRepository : IConversationRepository
{
    private readonly ApplicationDbContext _context;

    public ConversationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Conversation>> GetConversationsByUserIdAsync(string userId)
    {
        return await _context.Conversations
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }
}