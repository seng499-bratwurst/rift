using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rift.Models;

public class FileEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string FileName { get; set; }

    [Required]
    public required byte[] Content { get; set; }

    public long Size { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public required string UploadedBy { get; set; }
}

public class FileEntityDto{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string FileName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public required string UploadedBy { get; set; }
}