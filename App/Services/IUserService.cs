using Rift.Models;

public interface IUserService
{
    Task<User?> UpdateUser(
        string userId,
        string? name = null,
        string? email = null,
        string? oncApiToken = null
    );
}