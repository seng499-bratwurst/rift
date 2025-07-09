using Rift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rift.Controllers; // For RoleChangeResult

namespace Rift.Services;

public interface IAdminService
{
    Task<List<(User user, IList<string> roles)>> GetUsersWithRolesAsync();
    Task<AdminController.RoleChangeResult> ChangeUserRoleAsync(string userId, string newRole);
}