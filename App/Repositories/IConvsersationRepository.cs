using Rift.Models;

public interface IConversationRepository
{
    Task<List<Conversation>> GetConversationsByUserIdAsync(string userId);
    Task<Conversation> CreateConversationByUserId(string userId);

    Task<Conversation> CreateConversationBySessionId(string sessionId);
    Task<Conversation?> DeleteConversation(string userId, int conversationId);

}