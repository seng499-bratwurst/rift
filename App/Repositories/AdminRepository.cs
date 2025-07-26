using Microsoft.AspNetCore.Identity;
using Rift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Rift.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly UserManager<User> _userManager;

    public AdminRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<(User user, IList<string> roles)>> GetUsersWithRolesAsync()
    {
        var users = _userManager.Users.ToList(); // Materialize the users list
        var result = new List<(User, IList<string>)>();

        foreach (var user in users) // Iterate sequentially
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add((user, roles));
        }

        return result;
    }

    public async Task<RoleChangeResult> ChangeUserRoleAsync(string userId, string newRole)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return RoleChangeResult.UserNotFound;

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
            return RoleChangeResult.RemoveRolesFailed;

        var addResult = await _userManager.AddToRoleAsync(user, newRole);
        if (!addResult.Succeeded)
            return RoleChangeResult.AddRoleFailed;

        return RoleChangeResult.Success;
    }
}