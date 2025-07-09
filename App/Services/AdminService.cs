using Rift.Models;
using Rift.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public async Task<bool> ChangeUserRoleAsync(string userId, string newRole)
    {
        return await _adminRepository.ChangeUserRoleAsync(userId, newRole);
    }
}