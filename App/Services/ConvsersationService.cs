namespace Rift.Services;

using Rift.Models;
using Rift.Repositories;

public class ConversationService : IConversationService
{
    private readonly IConversationRepository _repository;

    public ConversationService(IConversationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Conversation>> GetConversationsForUserAsync(string userId)
    {
        return await _repository.GetConversationsByUserIdAsync(userId);
    }

    public async Task<Conversation> CreateConversation(string userId)
    {
        return await _repository.CreateConversation(userId);
    }

    public async Task<Conversation?> DeleteConversation(string userId, int conversationId)
    {
        return await _repository.DeleteConversation(userId, conversationId);
    }
}