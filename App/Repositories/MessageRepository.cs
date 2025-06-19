using Microsoft.EntityFrameworkCore;
using Rift.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rift.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
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

    public async Task<List<Message>> GetMessagesByConversationIdAsync(string userId, int conversationId)
    {
        return await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .Where(m => m.Conversation != null && m.Conversation.UserId == userId)
            .Include(m => m.OutgoingEdges)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }
}