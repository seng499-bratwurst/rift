using Rift.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rift.Repositories;

public interface IAdminRepository
{
    Task<List<(User user, IList<string> roles)>> GetUsersWithRolesAsync();
    Task<RoleChangeResult> ChangeUserRoleAsync(string userId, string newRole);
}