using Rift.Models;

namespace Rift.Services;

/// <summary>
/// Interface for Group2Company API service integration.
/// </summary>
public interface IGroup2CompanyService
{
    /// <summary>
    /// Gets a response from Group2Company's RAG API.
    /// </summary>
    /// <param name="query">The user's query string</param>
    /// <param name="messageHistory">Optional conversation history</param>
    /// <param name="oncApiToken">ONC API token for authentication</param>
    /// <returns>A tuple containing the response text and response type (0: bad answer, 1: clarification question, 2: good answer)</returns>
    Task<(string response, int? responseType)> GetResponseAsync(string query, List<Message>? messageHistory, string oncApiToken);
}