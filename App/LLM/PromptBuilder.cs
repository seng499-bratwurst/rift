
using Rift.App.Models;
using Rift.Models;

namespace Rift.LLM;

public class PromptBuilder
{
    private static int _promptIdCounter = 0;
    private readonly string _systemPrompt;

    public PromptBuilder(string systemPrompt)
    {
        _systemPrompt = systemPrompt;
    }

    public Prompt BuildPrompt(string userQuery, List<Message> messageHistory, string oncApiData, List<RelevantDocument> relevantDocuments)
    {
        var prompt = new Prompt
        {
            // Should probably update this once it is all working
            PromptId = _promptIdCounter++,
            SystemPrompt = _systemPrompt,
            UserQuery = userQuery,
            MessageHistory = messageHistory,
            OncAPIData = oncApiData,
            RelevantDocuments = relevantDocuments
        };

        return prompt;
    }
}