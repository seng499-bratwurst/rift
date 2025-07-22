namespace Rift.Models;

public class FileMetricTopic
{
    public required string Topic { get; set; }
    public required int FileUpVotes { get; set; }
    public required int FileDownVotes { get; set; }
    public required int FilesReferenced { get; set; }
    public required int QueryCount { get; set; }
}