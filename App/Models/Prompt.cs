using Rift.Models;

namespace Rift.App.Models;

public class Prompt
{
    public int PromptId { get; set; }
    public string SystemPrompt { get; set; } = string.Empty;
    public string UserQuery { get; set; } = string.Empty;
    public List<Message> MessageHistory { get; set; } = [];
    public string OncAPIData { get; set; } = string.Empty;
    public List<string> RelevantDocuments { get; set; } = [];
}