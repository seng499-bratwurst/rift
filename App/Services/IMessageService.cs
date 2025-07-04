using Rift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rift.Services;

public interface IMessageService
{
    Task<Message?> CreateMessageAsync(
        int? conversationId,
        int? promptMessageId,
        string content,
        string role,
        float xCoordinate,
        float yCoordinate
    );
    Task<List<Message>> GetMessagesForConversationAsync(string userId, int conversationId);
    Task<List<Message>> GetGuestMessagesForConversationAsync(string sessionId, int conversationId);

    Task<Message?> UpdateMessageAsync(
        int messageId,
        string userId,
        float xCoordinate,
        float yCoordinate
    );

    Task<Message?> DeleteMessageAsync(string userId, int messageId);
}