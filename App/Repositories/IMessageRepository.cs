using Rift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rift.Repositories;

public interface IMessageRepository
{
    Task<Message?> GetByIdAsync(string userId, int messageId);
    Task<Message> UpdateAsync(Message message);
    Task<Message> CreateAsync(Message message);
    Task<Message?> DeleteAsync(string userId, int messageId);
    Task<List<Message>> GetUserConversationMessagesAsync(string userId, int conversationId);
    Task<List<Message>> GetGuestConversationMessagesAsync(string sessionId, int conversationId);
}