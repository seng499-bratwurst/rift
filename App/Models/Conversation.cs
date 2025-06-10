namespace Rift.Models;

public class Conversation
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? SessionId { get; set; }
    public string? Title { get; set; }
    public DateTime FirstInteraction { get; set; }
    public DateTime LastInteraction { get; set; }

    public User? User { get; set; }
}