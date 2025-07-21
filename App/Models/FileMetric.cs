using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rift.Models;

public class FileMetric
{
    public required int FileId { get; set; }
    public required string FileName { get; set; }
    public required int UpVotes { get; set; }
    public required int DownVotes { get; set; }
    public required int Usages { get; set; }
}