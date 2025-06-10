
namespace Rift.LLM;

public class PromptBuilder
{
    private readonly string _systemPrompt;

    public PromptBuilder(string systemPrompt)
    {
        _systemPrompt = systemPrompt;
    }

    public string BuildPrompt(string userQuery, string relevantData)
    {
        // Here The prompt is built up for the larger LLM that will include everything we want.
        // In here we should also gather the user history and any metadata we want to include.
        throw new NotImplementedException("Prompt building logic is not implemented yet.");
    }
}