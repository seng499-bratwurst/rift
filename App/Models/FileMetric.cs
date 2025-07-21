namespace Rift.Models;

public class FileMetric
{
    public required int FileId { get; set; }
    public required string FileName { get; set; }
    public required int UpVotes { get; set; }
    public required int DownVotes { get; set; }
    public required int Usages { get; set; }
}