using Rift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rift.Repositories;

public interface IMessageRepository
{
    Task<Message> CreateAsync(Message message);
    Task<List<Message>> GetMessagesByConversationIdAsync(string userId, int conversationId);
}