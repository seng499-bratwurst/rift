using System.Text;
using System.Text.Json;
using Rift.Models;

namespace Rift.Services;

/// <summary>
/// Service for integrating with Group2Company's RAG API.
/// Handles communication with the oceannetworksapi.nathanielroberts.ca endpoint
/// and formats message history according to their API requirements.
/// </summary>
public class Group2CompanyService : IGroup2CompanyService
{
    private readonly HttpClient _httpClient;
    private readonly string _systemOncToken;
    private const string BaseUrl = "https://oceannetworksapi.nathanielroberts.ca";
    private const string SearchEndpoint = "/search";

    public Group2CompanyService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _systemOncToken = configuration["ONC_TOKEN"] ?? throw new InvalidOperationException("ONC_TOKEN is not configured.");
    }

    public async Task<(string response, int? responseType)> GetResponseAsync(string query, List<Message>? messageHistory, string oncApiToken)
    {
        try
        {
            // Use the user's token if provided and not empty, otherwise use the system token
            string tokenToUse = !string.IsNullOrEmpty(oncApiToken) ? oncApiToken : _systemOncToken;

            var requestBody = new
            {
                query = query,
                collection_name = "new_collection",
                token = tokenToUse,
                message_history = FormatMessageHistoryAsArray(messageHistory) // Return as array, not string
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var fullUrl = $"{BaseUrl}{SearchEndpoint}";
            var response = await _httpClient.PostAsync(fullUrl, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Group2Company API request failed with status code: {response.StatusCode}. URL: {fullUrl}. Error: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            
            var apiResponse = JsonSerializer.Deserialize<Group2CompanyApiResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return (apiResponse?.Answer ?? "No response received", apiResponse?.Type);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error calling Group2Company API: {ex.Message}", ex);
        }
    }

    private string FormatMessageHistory(List<Message>? messageHistory)
    {
        if (messageHistory == null || !messageHistory.Any())
        {
            return "[]";
        }

        var formattedHistory = messageHistory.Select(m => new
        {
            actor = m.Role == "user" ? "user" : "system",
            message = m.Content ?? ""
        }).ToList();

        return JsonSerializer.Serialize(formattedHistory);
    }

    private object FormatMessageHistoryAsArray(List<Message>? messageHistory)
    {
        if (messageHistory == null || !messageHistory.Any())
        {
            return new object[0]; // Return empty array
        }

        return messageHistory.Select(m => new
        {
            actor = m.Role == "user" ? "user" : "system",
            message = m.Content ?? ""
        }).ToArray();
    }

    private class Group2CompanyApiResponse
    {
        public string? Answer { get; set; }
        public int? Type { get; set; }
    }
}