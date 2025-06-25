using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rift.Models;

[Table("AspCompanyAPITokens")]
public class CompanyAPITokens
{
    [Key]
    public string CompanyName { get; set; } = string.Empty;
    public string? ONCApiToken { get; set; } = null;

    public int? Usage { get; set; } = null;
}