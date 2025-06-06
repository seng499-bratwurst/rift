namespace Rift.Models;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public required string Name { get; set; }
    public string? ONCApiToken { get; set; } = null;
}