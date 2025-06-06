namespace Rift.Models;

public class Message
{
    public int Id { get; set; }
    public int? ConversationId { get; set; }
    public string? Content { get; set; }
    public string? OncApiQuery { get; set; }
    public string? OncApiResponse { get; set; }
    public bool? IsHelpful { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Conversation? Conversation { get; set; }
}