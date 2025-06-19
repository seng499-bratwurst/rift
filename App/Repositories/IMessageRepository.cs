using Rift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rift.Repositories;

public interface IMessageRepository
{
    Task<Message?> GetByIdAsync(string userId, int messageId);
    Task<Message> UpdateAsync(Message message);
    Task<Message> CreateAsync(Message message);
    Task<List<MessageWithEdges>> GetMessagesByConversationIdAsync(string userId, int conversationId);
}