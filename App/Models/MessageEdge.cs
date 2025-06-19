using System.Text.Json.Serialization;

namespace Rift.Models;

public class MessageEdge
{
    public int Id { get; set; }
    public required int SourceMessageId { get; set; }
    public required int TargetMessageId { get; set; }
    public string SourceHandle { get; set; } = "bottom";
    public string TargetHandle { get; set; } = "top";
    // JsonIgnore to prevent circular reference issues during serialization between Message and MessageEdge
    [JsonIgnore]
    public Message? SourceMessage { get; set; }
}

public class PartialMessageEdge
{
    public required int SourceMessageId { get; set; }
    public string SourceHandle { get; set; } = "bottom";
    public string TargetHandle { get; set; } = "top";
}