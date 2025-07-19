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
    public required float XCoordinate { get; set; }
    public required float YCoordinate { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Conversation? Conversation { get; set; }
    public Message? PromptMessage { get; set; }
    public ICollection<MessageEdge> OutgoingEdges { get; set; } = new List<MessageEdge>();
    public ICollection<MessageEdge> IncomingEdges { get; set; } = new List<MessageEdge>();
    public ICollection<FileEntityDto> Documents { get; set; } = new List<FileEntityDto>();

}

// public class MessageWithEdges : Message
// {
//     public ICollection<MessageEdge?> OutgoingEdges { get; set; } = new List<MessageEdge?>();
// }