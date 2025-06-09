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

    public async Task<Conversation> CreateConversation(string userId)
    {
        var conversation = new Conversation
        {
            UserId = userId,
            FirstInteraction = DateTime.UtcNow,
            LastInteraction = DateTime.UtcNow
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();

        return conversation;
    }
}