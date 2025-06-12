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
            .Select(m => new Message
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                PromptMessageId = m.PromptMessageId,
                Content = m.Content,
                OncApiQuery = m.OncApiQuery,
                OncApiResponse = m.OncApiResponse,
                IsHelpful = m.IsHelpful,
                Role = m.Role,
                CreatedAt = m.CreatedAt,
                XCoordinate = m.XCoordinate,
                YCoordinate = m.YCoordinate,
            })
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }
}