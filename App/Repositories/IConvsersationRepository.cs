using Rift.Models;

public interface IConversationRepository
{
    Task<List<Conversation>> GetConversationsByUserIdAsync(string userId);
    Task<Conversation> CreateConversation(string userId);
    Task<Conversation?> DeleteConversation(string userId, int conversationId);

}