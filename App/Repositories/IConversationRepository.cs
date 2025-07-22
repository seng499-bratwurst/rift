using Rift.Models;

public interface IConversationRepository
{
    Task<List<Conversation>> GetConversationsByUserIdAsync(string userId);
    Task<Conversation?> GetConversationsBySessionIdAsync(string sessionId);

    Task<Conversation> CreateConversationByUserId(string userId);
    Task<Conversation> CreateConversationBySessionId(string sessionId);

    Task<Conversation?> DeleteConversation(string userId, int conversationId);

    Task<Conversation?> GetConversationById(string userId, int conversationId);
    Task<Conversation?> UpdateLastInteractionTime(int conversationId);
    Task<Conversation?> UpdateConversationTitle(int conversationId, string title);
    Task<Conversation?> GetConversationByIdOnly(int conversationId);
}