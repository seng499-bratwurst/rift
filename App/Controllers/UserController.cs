using Microsoft.AspNetCore.Mvc;
using Rift.Models;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public UserController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User data)
    {
        _dbContext.Users.Add(data);
        await _dbContext.SaveChangesAsync();
        return Created($"/register/{data.id}", data);
    }
}