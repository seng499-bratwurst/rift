using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rift.Models;
using Rift.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rift.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    /// <summary>
    /// Get a list of all users and their roles.
    /// </summary>
    [HttpGet("user-list")]
    public async Task<IActionResult> GetUsersWithRoles()
    {
        var usersWithRoles = await _adminService.GetUsersWithRolesAsync();
        var userList = new List<UserWithRolesDto>();

        foreach (var (user, roles) in usersWithRoles)
        {
            userList.Add(new UserWithRolesDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user?.Email ?? string.Empty,
                Roles = new List<string>(roles)
            });
        }

        return Ok(new ApiResponse<List<UserWithRolesDto>>
        {
            Success = true,
            Error = null,
            Data = userList
        });
    }

    /// <summary>
    /// Change a user's role from "User" to "Admin" or vice versa.
    /// </summary>
    [HttpPatch("users/{userId}/role")]
    public async Task<IActionResult> ChangeUserRole(string userId, [FromBody] ChangeRoleRequest request)
    {

        if (request == null || !ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Invalid request data.",
                Data = null
            });
        }

        var result = await _adminService.ChangeUserRoleAsync(userId, request.NewRole);

        switch (result)
        {
            case RoleChangeResult.UserNotFound:
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Error = "User not found.",
                    Data = null
                });
            case RoleChangeResult.RemoveRolesFailed:
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Error = "Failed to remove existing roles.",
                    Data = null
                });
            case RoleChangeResult.AddRoleFailed:
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Error = "Failed to add new role.",
                    Data = null
                });
            case RoleChangeResult.Success:
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Error = null,
                    Data = $"User role changed to {request.NewRole}."
                });
            default:
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Error = "Unknown error.",
                    Data = null
                });
        }
    }

    public class ChangeRoleRequest
    {
        public string NewRole { get; set; } = "User";
    }

    public class UserWithRolesDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
