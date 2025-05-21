using Microsoft.AspNetCore.Mvc;
using Rift.Models;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User data)
    {
        var registeredUser = await _userService.RegisterUserAsync(data);
        return Created($"/register/{registeredUser.id}", registeredUser);
    }
}