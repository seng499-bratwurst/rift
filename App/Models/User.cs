namespace Rift.Models;

// purely an example model class we can build on

public class User
{
    public int id { get; set; }
    public required string name { get; set; }
    public required string email { get; set; }
    public string? apiToken { get; set; }
}