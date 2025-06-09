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
        int? promptMessageId,
        string content,
        string role
    )
    {
        var message = new Message
        {
            ConversationId = conversationId,
            PromptMessageId = promptMessageId,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            Role = role,
        };
        return await _messageRepository.CreateAsync(message);
    }

    public async Task<List<Message>> GetMessagesForConversationAsync(int? conversationId)
    {
        if (conversationId == null) return new List<Message>();
        return await _messageRepository.GetMessagesByConversationIdAsync(conversationId.Value);
    }
}