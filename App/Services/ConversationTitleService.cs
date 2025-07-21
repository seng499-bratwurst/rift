using DotnetGeminiSDK;
using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model.Request;
using Rift.Repositories;

namespace Rift.Services
{
    public class ConversationTitleService : IConversationTitleService
    {
        private readonly string _apiKey;
        private readonly string _systemPrompt;
        private readonly IConversationRepository _conversationRepository;

        public ConversationTitleService(IConfiguration config, IConversationRepository conversationRepository)
        {
            // Use the same configuration pattern as GoogleGemma.cs
            _apiKey = config["LLMSettings:GoogleGemma:ApiKey"] ?? throw new ArgumentNullException("GoogleGemma ApiKey missing in config");
            
            _conversationRepository = conversationRepository;

            // Load system prompt from file
            var promptPath = Path.Combine(AppContext.BaseDirectory, "LLM", "SystemPrompts", "conversation_title_prompt.md");
            _systemPrompt = File.Exists(promptPath) ? File.ReadAllText(promptPath) : "Generate a 3-9 word title about oceanography.";
        }

        public async Task<string> GenerateTitleAsync(string userPrompt, string assistantResponse)
        {
            try
            {
                // Construct the full prompt for Gemini using both user prompt and assistant response
                string fullPrompt = $"{_systemPrompt}\n\n" +
                                    $"User Question: \"{userPrompt}\"\n" +
                                    $"Assistant Response: \"{assistantResponse}\"\n\n" +
                                    "Generate a conversation title:";

                // Initialize the Gemini client with API key
                var gemini = new GoogleGeminiClient(
                    new GoogleGeminiConfig
                    {
                        ApiKey = _apiKey
                    }
                );

                // Create the request for the Gemini 2.5 Flash model
                var request = new GeminiRequest
                {
                    Contents = new List<Content>
                    {
                        new Content
                        {
                            Parts = new List<Part>
                            {
                                new Part
                                {
                                    Text = fullPrompt
                                }
                            }
                        }
                    }
                };

                // Make the API call to generate the content
                var result = await gemini.GenerateContentAsync("gemini-2.5-flash", request);

                if (result?.Candidates != null && result.Candidates.Count > 0)
                {
                    var title = result.Candidates[0].Content.Parts[0].Text.Trim();
                    
                    if (string.IsNullOrEmpty(title))
                    {
                        throw new InvalidOperationException("LLM returned empty title");
                    }

                    // Validate the title
                    ValidateTitle(title);
                    return title;
                }
                else
                {
                    throw new InvalidOperationException("Could not generate a title - no candidates returned");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to generate conversation title: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateConversationTitleAsync(int conversationId, string title)
        {
            return await _conversationRepository.UpdateConversationTitle(conversationId, title);
        }

        private void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty or whitespace");
            }

            var words = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 3 || words.Length > 9)
            {
                throw new ArgumentException($"Title must be 3-9 words, got {words.Length} words: '{title}'");
            }

            // Check for forbidden words
            var forbiddenWords = new[] { "chat", "conversation", "question", "help", "information", "data request" };
            var lowerTitle = title.ToLower();
            
            foreach (var forbidden in forbiddenWords)
            {
                if (lowerTitle.Contains(forbidden))
                {
                    throw new ArgumentException($"Title contains forbidden word '{forbidden}': '{title}'");
                }
            }
        }
    }
}
