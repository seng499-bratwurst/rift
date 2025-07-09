using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Rift.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Rift.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public AdminController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Get a list of all users and their roles.
    /// </summary>
    [HttpGet("user-list")]
    public async Task<IActionResult> GetUsersWithRoles()
    {
        var users = _userManager.Users.ToList();
        var userList = new List<UserWithRolesDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userList.Add(new UserWithRolesDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Roles = roles.ToList()
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
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Error = "User not found.",
                Data = null
            });
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        // Remove all roles first
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Failed to remove existing roles.",
                Data = null
            });
        }

        // Add the new role
        var addResult = await _userManager.AddToRoleAsync(user, request.NewRole);
        if (!addResult.Succeeded)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Failed to add new role.",
                Data = null
            });
        }

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Error = null,
            Data = $"User role changed to {request.NewRole}."
        });
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
