using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Rift.Models;
using Rift.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/sessional-keys")]
public class SessionalKeyController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetSessionalKeyForGuest()
    {
        var sessionId = Guid.NewGuid().ToString();

        return Ok(new
        {
            sessionId,
        });
    }
}