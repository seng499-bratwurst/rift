using System.Text.RegularExpressions;

public class ResponseProcessor
{
    public (string cleanedResponse, string? conversationTitle) ProcessResponse(string LLMResponse)
    {
        // Extract conversation title using regex pattern - only support "Conversation Title" format as per sys_prompt_large_llm.md
        var conversationTitlePattern = @"\*\*Conversation Title:\*\*\s*(.+)$";
        
        string? conversationTitle = null;
        string cleanedResponse = LLMResponse;
        
        // Match the "Conversation Title" format
        var conversationMatch = Regex.Match(LLMResponse, conversationTitlePattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
        if (conversationMatch.Success)
        {
            conversationTitle = conversationMatch.Groups[1].Value.Trim();
            // Remove the conversation title line from the response
            cleanedResponse = Regex.Replace(cleanedResponse, conversationTitlePattern, "", RegexOptions.Multiline | RegexOptions.IgnoreCase).Trim();
        }
        
        return (cleanedResponse, conversationTitle);
    }
}