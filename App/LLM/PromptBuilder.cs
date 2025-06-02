
namespace Rift.LLM;

public class PromptBuilder
{
    private readonly string _systemPrompt;
    private readonly string _userPrompt;

    public PromptBuilder(string systemPrompt, string userPrompt)
    {
        _systemPrompt = systemPrompt;
        _userPrompt = userPrompt;
    }

  public string BuildPrompt(string userQuery, string relevantData)
  {
      throw new NotImplementedException("Prompt building logic is not implemented yet.");
    }
}