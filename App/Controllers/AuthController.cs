using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Rift.Models;
using Rift.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = new JwtTokenService(configuration);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            return BadRequest(new { error = "A user with this email already exists." });
        }

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            ONCApiToken = model.ONCApiToken,
            UserName = Guid.NewGuid().ToString()
        };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            var roles = await _userManager.GetRolesAsync(user);
            var jwt = _jwtTokenService.GenerateJwtToken(user, roles);
            return Ok(new { jwt });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (result.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var jwt = _jwtTokenService.GenerateJwtToken(user, roles);
            return Ok(new { jwt });
        }

        return Unauthorized("Invalid username or password.");
    }

    public class RegisterModel
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? ONCApiToken { get; set; }
    }

    public class LoginModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}