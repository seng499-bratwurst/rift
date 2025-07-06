using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Rift.Models;
using Rift.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtTokenService _jwtTokenService;
    private readonly IWebHostEnvironment _env;
    private bool _isProduction => _env.IsProduction();


    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration,
        IWebHostEnvironment env
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = new JwtTokenService(configuration);
        _env = env;
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

            SetJwtCookie(jwt);

            return Ok(new
            {
                jwt,
                roles,
                user = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.ONCApiToken
                }
            });
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

            SetJwtCookie(jwt);

            return Ok(new
            {
                jwt,
                roles,
                user = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.ONCApiToken
                }
            });
        }

        return Unauthorized("Invalid username or password.");
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            roles,
            user = new
            {
                user.Id,
                user.Name,
                user.Email,
                user.ONCApiToken
            }
        });
    }

    private void SetJwtCookie(string jwt)
    {
        Response.Cookies.Append("jwt", jwt, new CookieOptions
        {
            HttpOnly = true,
            Secure = _isProduction,
            SameSite = _isProduction ? SameSiteMode.Strict : SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddHours(1),
        });
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