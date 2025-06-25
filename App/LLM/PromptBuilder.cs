
using System.Text;
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

    public Prompt BuildPrompt(string userQuery, List<Message> messageHistory, string oncApiData, List<string> relevantDocuments)
    {
        var fullSystemPrompt = new StringBuilder(_systemPrompt);
        fullSystemPrompt.AppendLine("\nOnly respond to the user query!");
        fullSystemPrompt.AppendLine("\nDo not include the ONC API data, relevant documents, or message history in your response. Instead, use them to inform your response to the user query.");
        fullSystemPrompt.AppendLine("\nThe user is not aware of the ONC API data, relevant documents, or message history, so do not mention them in your response.");
        var prompt = new Prompt
        {
            // TODO: Just a rough implementation for now, I will update this once we get the MVP working
            // Should probably update this once it is all working
            PromptId = _promptIdCounter++,
            SystemPrompt = fullSystemPrompt.ToString(),
            UserQuery = userQuery,
            MessageHistory = messageHistory,
            OncAPIData = oncApiData,
            RelevantDocuments = relevantDocuments
        };

        return prompt;
    }
}