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
        string role,
        float xCoordinate,
        float yCoordinate
    )
    {
        var message = new Message
        {
            ConversationId = conversationId,
            PromptMessageId = promptMessageId,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            Role = role,
            XCoordinate = xCoordinate,
            YCoordinate = yCoordinate,
        };
        return await _messageRepository.CreateAsync(message);
    }

    public async Task<List<Message>> GetMessagesForConversationAsync(string userId, int conversationId)
    {
        return await _messageRepository.GetMessagesByConversationIdAsync(userId, conversationId);
    }

    public async Task<Message?> UpdateMessageAsync(
        int messageId,
        string userId,
        float xCoordinate,
        float yCoordinate
    )
    {
        var message = await _messageRepository.GetByIdAsync(userId, messageId);
        if (message == null)
        {
            return null; // Message not found
        }

        message.XCoordinate = xCoordinate;
        message.YCoordinate = yCoordinate;

        return await _messageRepository.UpdateAsync(message);
    }
}