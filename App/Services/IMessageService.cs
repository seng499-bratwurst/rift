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
        string role
    );
    Task<List<Message>> GetMessagesForConversationAsync(int? conversationId);
}