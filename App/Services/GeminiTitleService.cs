using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DotNetEnv;

namespace Rift.Services;

public class GeminiTitleService : IGeminiTitleService
{
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _promptTemplate;

        public GeminiTitleService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;

            // Load environment variables
            Env.Load();
            _apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")
                ?? throw new InvalidOperationException("GOOGLE_API_KEY environment variable is required");

            // Load the conversation title prompt template
            try
            {
                var promptPath = Path.Combine(AppContext.BaseDirectory, "LLM", "SystemPrompts", "conversation_title_prompt.md");
                _promptTemplate = File.ReadAllText(promptPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not load conversation title prompt template: {ex.Message}");
                _promptTemplate = GetFallbackPrompt();
            }
        }

        public async Task<string> GenerateTitleAsync(string userPrompt, string assistantResponse)
        {
            try
            {
                var fullPrompt = $"{_promptTemplate}\n\n" +
                                $"**User Prompt:** {userPrompt}\n\n" +
                                $"**LLM Response:** {assistantResponse}\n\n" +
                                "Generate the conversation title now:";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = fullPrompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.3,
                        topK = 40,
                        topP = 0.8,
                        maxOutputTokens = 100,
                        stopSequences = new string[] { }
                    }
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key={_apiKey}";
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var title = ExtractTitleFromResponse(responseContent);
                    return title ?? "Untitled Conversation";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Gemini API error: {response.StatusCode} - {errorContent}");
                    return "Untitled Conversation";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating title with Gemini: {ex.Message}");
                return "Untitled Conversation";
            }
        }

        private string? ExtractTitleFromResponse(string responseJson)
        {
            try
            {
                var response = JsonSerializer.Deserialize<GeminiResponse>(responseJson);
                var rawText = response?.candidates?[0]?.content?.parts?[0]?.text?.Trim();
                
                if (string.IsNullOrEmpty(rawText))
                    return null;

                var title = rawText;
                
                // Look for the "**Conversation Title:**" format and extract just the title part
                var titlePrefix = "**Conversation Title:**";
                var titleIndex = title.IndexOf(titlePrefix, StringComparison.OrdinalIgnoreCase);
                if (titleIndex >= 0)
                {
                    // Extract everything after the prefix
                    title = title.Substring(titleIndex + titlePrefix.Length).Trim();
                }
                
                // Also handle cases where it might be in a code block or have markdown formatting
                title = title.Trim('`', '"', '\'', ' ', '\n', '\r', '*');
                
                // Remove any remaining markdown or formatting
                if (title.StartsWith("```") && title.EndsWith("```"))
                {
                    title = title.Substring(3, title.Length - 6).Trim();
                }
                
                // Ensure it's within reasonable length (3-9 words as per prompt)
                if (!string.IsNullOrEmpty(title) && title.Length > 100)
                {
                    var words = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length > 9)
                    {
                        title = string.Join(" ", words.Take(9));
                    }
                }
                
                return string.IsNullOrEmpty(title) ? null : title;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing Gemini response: {ex.Message}");
                return null;
            }
        }

        private string GetFallbackPrompt()
        {
            return @"You are a professional conversation title generator for an oceanographic research platform. 

Your task is to create concise, descriptive titles (3-9 words) that capture the essence of conversations between users and AI assistants about ocean science.

Guidelines:
- Use clear, scientific terminology when appropriate
- Be specific rather than generic
- Focus on the main topic/concept discussed
- Avoid redundant words like 'conversation', 'discussion', 'chat'
- Use title case formatting
- If the conversation involves multiple topics, focus on the primary one

Examples:
- User asks about ocean currents → 'Ocean Current Patterns and Formation'
- User asks about marine biology → 'Marine Species Biodiversity Analysis'
- User asks about climate change → 'Climate Change Ocean Impact'

Generate only the title, nothing else.";
        }

        public class GeminiResponse
        {
            public GeminiCandidate[]? candidates { get; set; }
        }

        public class GeminiCandidate
        {
            public GeminiContent? content { get; set; }
        }

        public class GeminiContent
        {
            public GeminiPart[]? parts { get; set; }
        }

        public class GeminiPart
        {
            public string? text { get; set; }
        }
    }
