namespace Rift.Services;

using Microsoft.VisualBasic;
using Rift.Models;

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

    public async Task<Conversation?> GetConversationsForSessionAsync(string sessionId)
    {
        return await _repository.GetConversationsBySessionIdAsync(sessionId);
    }

    public async Task<Conversation> CreateConversationByUserId(string userId)
    {
        return await _repository.CreateConversationByUserId(userId);
    }

    public async Task<Conversation> CreateConversationBySessionId(string sessionId)
    {
        return await _repository.CreateConversationBySessionId(sessionId);
    }

    public async Task<Conversation?> DeleteConversation(string userId, int conversationId)
    {
        return await _repository.DeleteConversation(userId, conversationId);
    }

    public async Task<Conversation?> GetConversationById(string userId, int conversationId)
    {
        return await _repository.GetConversationById(userId, conversationId);
    }

    public async Task<Conversation?> GetOrCreateConversationByUserId(string userId, int? conversationId)
    {
        Conversation? conversation;
        if (conversationId == null)
        {
            conversation = await CreateConversationByUserId(userId);
        }
        else
        {
            conversation = await GetConversationById(userId, conversationId.Value);
        }
        return conversation;
    }

    public async Task<Conversation?> UpdateLastInteractionTime(int conversationId)
    {
        return await _repository.UpdateLastInteractionTime(conversationId);
    }

    public async Task<Conversation?> UpdateConversationTitle(int conversationId, string title)
    {
        return await _repository.UpdateConversationTitle(conversationId, title);
    }

    public async Task<Conversation?> GetConversationByIdOnly(int conversationId)
    {
        return await _repository.GetConversationByIdOnly(conversationId);
    }
}