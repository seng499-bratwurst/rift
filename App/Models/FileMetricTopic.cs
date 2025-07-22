namespace Rift.Models;

public class FileMetricTopic
{
    public required string Topic { get; set; }
    public required int UpVotes { get; set; }
    public required int DownVotes { get; set; }
    public required int Usages { get; set; }
}