using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Rift.Models;
using Rift.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/password")]
public class PasswordController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public PasswordController(
        UserManager<User> userManager
    )
    {
        _userManager = userManager;
    }

    [HttpPatch("")]
    [Authorize]
    public async Task<IActionResult> PasswordReset([FromBody] PasswordResetModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (model.newPassword != model.NewPasswordConfirmed)
        {
            return BadRequest(new { error = "New password and confirmation do not match." });
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { error = "User not found." });
        }

        var result = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);

        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description).ToArray() });
        }

        return Ok(new { message = "Password updated successfully." });
    }

    public class PasswordResetModel
    {
        public required string oldPassword { get; set; }
        public required string newPassword { get; set; }
        public required string NewPasswordConfirmed { get; set; }
    }
}