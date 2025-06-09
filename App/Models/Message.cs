namespace Rift.Models;

public class Message
{
    public int Id { get; set; }
    public int? ConversationId { get; set; }
    public int? PromptMessageId { get; set; }
    public string? Content { get; set; }
    public string? OncApiQuery { get; set; }
    public string? OncApiResponse { get; set; }
    public bool? IsHelpful { get; set; }
    public string Role { get; set; } = "user";
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Conversation? Conversation { get; set; }
    public Message? PromptMessage { get; set; }
}