using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rift.Models;

public class FileEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public string Content { get; set; } = string.Empty;

    public long Size { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public required string UploadedBy { get; set; }
    public string SourceLink { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty;
}

public class FileEntityDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Required]
    public required string UploadedBy { get; set; }
    public string SourceLink { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty;
}