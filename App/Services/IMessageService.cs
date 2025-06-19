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
    Task<List<MessageWithEdges>> GetMessagesForConversationAsync(string userId, int conversationId);

    Task<Message?> UpdateMessageAsync(
        int messageId,
        string userId,
        float xCoordinate,
        float yCoordinate
    );
}