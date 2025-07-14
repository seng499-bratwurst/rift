using Rift.Models;

namespace Rift.App.Models;

public class PromptMessage
{
    public string role { get; set; } = string.Empty;
    public string content { get; set; } = string.Empty;

}

public class DocumentChunk
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class Prompt
{
    public string SystemPrompt { get; set; } = string.Empty;
    public string UserQuery { get; set; } = string.Empty;
    public List<PromptMessage> Messages { get; set; } = [];
    public string OncAPIData { get; set; } = string.Empty;
    public List<DocumentChunk> RelevantDocumentChunks { get; set; } = [];
}