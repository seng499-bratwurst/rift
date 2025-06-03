
namespace Rift.LLM;

public class PromptBuilder
{
    private readonly string _systemPrompt;

    public PromptBuilder(string systemPrompt)
    {
        _systemPrompt = systemPrompt;;
    }

    public string BuildPrompt(string userQuery, string relevantData)
    {
        // Here 
        throw new NotImplementedException("Prompt building logic is not implemented yet.");
    }
}