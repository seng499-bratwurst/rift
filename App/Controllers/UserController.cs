using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Rift.Models;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPatch("users")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdate userUpdate)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userService.UpdateUser(userId, userUpdate.Name, userUpdate.Email, userUpdate.ONCApiToken);

        if (user == null)
        {
            return NotFound(new ApiResponse<User>
            {
                Success = false,
                Error = "User not found",
                Data = null
            });
        }

        return Ok(new ApiResponse<User>
        {
            Success = true,
            Error = null,
            Data = user
        });
    }
    public class UserUpdate
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ONCApiToken { get; set; }
    }
}