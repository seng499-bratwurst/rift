namespace Rift.Services;

using Microsoft.AspNetCore.Identity;
using Rift.Models;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;

    }

    public async Task<User?> UpdateUser(
        string userId,
        string? name = null,
        string? email = null,
        string? oncApiToken = null)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        if (email != null)
            user.Email = email;

        if (name != null)
            user.Name = name;

        if (oncApiToken != null && user is User appUser)
        {
            appUser.ONCApiToken = oncApiToken;
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return null;
        }

        return user;
    }
}