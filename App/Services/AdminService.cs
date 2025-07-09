using Rift.Models;
using Rift.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rift.Controllers; // For RoleChangeResult

namespace Rift.Services;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;

    public AdminService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public async Task<List<(User user, IList<string> roles)>> GetUsersWithRolesAsync()
    {
        return await _adminRepository.GetUsersWithRolesAsync();
    }

    public async Task<AdminController.RoleChangeResult> ChangeUserRoleAsync(string userId, string newRole)
    {
        // You will need to update your repository to return a result enum as well,
        // or map its bool/error output to the correct RoleChangeResult here.
        var repoResult = await _adminRepository.ChangeUserRoleAsync(userId, newRole);

        // If your repository returns a bool, map it to the enum here:
        if (repoResult)
        {
            return AdminController.RoleChangeResult.Success;
        }
        else
        {
            return AdminController.RoleChangeResult.AddRoleFailed; // Or map to the appropriate failure
        }
    }
}