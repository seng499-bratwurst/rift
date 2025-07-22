using Microsoft.EntityFrameworkCore;
using Rift.Models;

namespace Rift.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _context;
    private readonly FileDbContext _filesDbContext;

    public MessageRepository(ApplicationDbContext context, FileDbContext filesDbContext)
    {
        _context = context;
        _filesDbContext = filesDbContext;
    }

    public async Task<Message?> GetByIdAsync(string userId, int messageId)
    {
        var message = await _context.Messages
            .Where(m => m.Id == messageId &&
                        _context.Conversations.Any(c => c.Id == m.ConversationId && c.UserId == userId))
            .FirstOrDefaultAsync();
        if (message == null)
        {
            return null; // Message not found
        }
        return message;
    }

    public async Task<Message> UpdateAsync(Message message)
    {
        _context.Messages.Update(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<Message> CreateAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<Message>> GetUserConversationMessagesAsync(string userId, int conversationId)
    {
        return await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .Where(m => m.Conversation != null && m.Conversation.UserId == userId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Message>> GetGuestConversationMessagesAsync(string sessionId, int conversationId)
    {
        return await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .Where(m => m.Conversation != null && m.Conversation.SessionId == sessionId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<Message?> DeleteAsync(string userId, int messageId)
    {
        var message = await GetByIdAsync(userId, messageId);
        if (message == null)
        {
            return null; // Message not found
        }

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<int>> GetMessageIdsContainingTextAsync(string text)
    {
        var loweredText = text.ToLower();
        return await _context.Messages
            .Where(m => m.PromptMessageId != null && m.Content != null && EF.Functions.Like(m.Content.ToLower(), $"%{loweredText}%"))
            .Select(m => m.Id)
            .ToListAsync();
    }
}