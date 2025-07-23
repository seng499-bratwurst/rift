
using System.Text;
using Rift.App.Models;
using Rift.Models;

namespace Rift.LLM;

public class PromptBuilder
{
    private readonly string _systemPrompt;

    public PromptBuilder(string systemPrompt)
    {
        _systemPrompt = systemPrompt;
    }

    public Prompt BuildPrompt(string userQuery, List<Message> messageHistory, string oncApiData, List<RelevantDocument> relevantDocuments)
    {
        var messages = new List<PromptMessage>
        {
            new PromptMessage
            {
                role = "system",
                content = _systemPrompt
            }
        };

        var formattedMessageHistory = messageHistory.Select(m => new PromptMessage
        {
            role = m.Role,
            content = m.Content ?? string.Empty
        }).ToList();

        messages.AddRange(formattedMessageHistory);

        messages.Add(new PromptMessage
        {
            role = "user",
            content = userQuery
        });

        var contextContent = new StringBuilder();
        contextContent.Append("[API Data] \n\n" + oncApiData + "\n\n");
        
        contextContent.Append("[Relevant Document Chunks] \n\n");
        var documentChunks = relevantDocuments.Select(doc => {
            var docTitle = string.Empty;
            if (doc.Metadata != null && doc.Metadata.ContainsKey("name"))
            {
                docTitle = doc.Metadata["name"].ToString() ?? string.Empty;
            }
            return new DocumentChunk
            {
                Title = docTitle,
                Content = doc.Content
            };
        }).ToList();

        foreach (var doc in documentChunks)
        {

            contextContent.Append($"\t[Document {doc.Title}]\n {doc.Content}\n");
        }

        messages.Add(new PromptMessage
        {
            role = "system",
            content = contextContent.ToString()
        });

        // Current date and time in the format of yyyy-MM-ddTHH:mm:ss.fffZ
        var currentDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

        var prompt = new Prompt
        {
            SystemPrompt = _systemPrompt.ToString() + $"\n\nCurrent Date and Time: {currentDate}",
            UserQuery = userQuery,
            Messages = messages,
            OncAPIData = oncApiData,
            RelevantDocumentChunks = documentChunks
        };

        return prompt;
    }
}