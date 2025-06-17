namespace Rift.Models;

public class MessageEdge
{
    public int Id { get; set; }
    public required int SourceMessageId { get; set; }
    public required int TargetMessageId { get; set; }
    public string SourceHandle { get; set; } = "bottom";
    public string TargetHandle { get; set; } = "top";
}