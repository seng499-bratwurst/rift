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

    public async Task<Conversation?> GetConversationsBySessionIdAsync(string sessionId)
    {
        return await _context.Conversations
            .Where(c => c.SessionId == sessionId)
            .SingleOrDefaultAsync();
    }

    public async Task<Conversation> CreateConversationByUserId(string userId)
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

    public async Task<Conversation> CreateConversationBySessionId(string sessionId)
    {
        var conversation = new Conversation
        {
            SessionId = sessionId,
            UserId = null,
            FirstInteraction = DateTime.UtcNow,
            LastInteraction = DateTime.UtcNow
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();

        return conversation;
    }

    public async Task<Conversation?> DeleteConversation(string userId, int conversationId)
    {
        var conversation = await GetConversationById(userId, conversationId);

        if (conversation == null)
        {
            return null;
        }

        _context.Messages.RemoveRange(
            _context.Messages.Where(m => m.ConversationId == conversationId));
        _context.Conversations.Remove(conversation);
        await _context.SaveChangesAsync();

        return conversation;
    }

    public async Task<Conversation?> GetConversationById(string userId, int conversationId)
    {
        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c => c.Id == conversationId && c.UserId == userId);

        if (conversation == null)
        {
            return null;
        }

        return conversation;
    }

    public async Task<Conversation?> UpdateLastInteractionTime(int conversationId)
    {
        var conversation = await _context.Conversations
          .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null)
        {
            return null;
        }

        conversation.LastInteraction = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return conversation;
    }

    public async Task<Conversation?> UpdateConversationTitle(int conversationId, string title)
    {
        var conversation = await _context.Conversations
          .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null)
        {
            return null;
        }

        conversation.Title = title;
        await _context.SaveChangesAsync();

        return conversation;
    }
}