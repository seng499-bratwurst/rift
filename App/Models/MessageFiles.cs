namespace Rift.Models;

public class MessageFiles
{
    public int Id { get; set; }
    public required int MessageId { get; set; }
    public required int FileId { get; set; }
    public DateTime CreatedAt { get; set; }
}