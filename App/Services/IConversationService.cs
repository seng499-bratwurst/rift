using Rift.Models;

public interface IConversationService
{
    Task<List<Conversation>> GetConversationsForUserAsync(string userId);

    Task<Conversation> CreateConversation(string userId);

    Task<Conversation?> DeleteConversation(string userId, int conversationId);

}