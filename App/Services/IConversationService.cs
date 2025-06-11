using Rift.Models;

public interface IConversationService
{
    Task<List<Conversation>> GetConversationsForUserAsync(string userId);

    Task<Conversation?> GetConversationsForSessionAsync(string sessionId);

    Task<Conversation> CreateConversationByUserId(string userId);

    Task<Conversation> CreateConversationBySessionId(string sessionId);

    Task<Conversation?> DeleteConversation(string userId, int conversationId);

}