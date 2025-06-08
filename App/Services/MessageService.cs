using Rift.Models;
using Rift.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rift.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<Message?> CreateMessageAsync(
        int? conversationId,
        string content,
        string? oncApiQuery,
        string? oncApiResponse,
        bool? isHelpful
    )
    {
        if (conversationId == null)
            return null;

        var message = new Message
        {
            ConversationId = conversationId,
            Content = content,
            OncApiQuery = oncApiQuery,
            OncApiResponse = oncApiResponse,
            IsHelpful = isHelpful,
            CreatedAt = DateTime.UtcNow
        };
        return await _messageRepository.CreateAsync(message);
    }

    public async Task<List<Message>> GetMessagesForConversationAsync(int? conversationId)
    {
        if (conversationId == null) return new List<Message>();
        return await _messageRepository.GetMessagesByConversationIdAsync(conversationId.Value);
    }
}