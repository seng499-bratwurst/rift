using Rift.Models;

public interface IConversationService
{
    Task<List<Conversation>> GetConversationsForUserAsync(string userId);
}