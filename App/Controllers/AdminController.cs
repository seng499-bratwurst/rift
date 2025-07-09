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
        var userList = new List<object>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userList.Add(new
            {
                user.Id,
                user.Name,
                user.Email,
                Roles = roles
            });
        }

        return Ok(new
        {
            Success = true,
            Users = userList
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
            return NotFound(new { Success = false, Error = "User not found." });
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        // Remove all roles first
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
        {
            return BadRequest(new { Success = false, Error = "Failed to remove existing roles." });
        }

        // Add the new role
        var addResult = await _userManager.AddToRoleAsync(user, request.NewRole);
        if (!addResult.Succeeded)
        {
            return BadRequest(new { Success = false, Error = "Failed to add new role." });
        }

        return Ok(new { Success = true, Message = $"User role changed to {request.NewRole}." });
    }

    public class ChangeRoleRequest
    {
        public string NewRole { get; set; } = "User";
    }
}
