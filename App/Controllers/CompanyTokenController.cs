using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rift.Models;
using Rift.Services;

namespace Rift.Controllers;

[ApiController]
[Route("api")]
public class CompanyTokenController : ControllerBase
{
    private readonly ICompanyTokenService _companyTokenService;

    public CompanyTokenController(ICompanyTokenService companyTokenService)
    {
        _companyTokenService = companyTokenService;
    }

    public class CreateCompanyTokenRequest
    {
        public string CompanyName { get; set; } = null!;
        public string? ONCApiToken { get; set; }
        public int Usage { get; set; } // could remove this since the middleware already handles this.
    }


    [HttpPost("company-token")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public async Task<IActionResult> SetCompanyToken([FromBody] CreateCompanyTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CompanyName))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Company name is required.",
                Data = null
            });
        }

        var token = await _companyTokenService.CreateTokenAsync(
            request.CompanyName,
            request.ONCApiToken,
            request.Usage
        );

        if (token == null)
        {
            return Conflict(new ApiResponse<object>
            {
                Success = false,
                Error = "Company token already exists.",
                Data = null
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Error = null,
            Data = new
            {
                token.CompanyName,
                token.ONCApiToken,
                token.Usage
            }
        });
    }


    [HttpDelete("company-token/{token}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public async Task<IActionResult> DeleteCompanyToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Token is required.",
                Data = null
            });
        }

        var result = await _companyTokenService.DeleteTokenAsync(token);

        if (result == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Error = "Token not found.",
                Data = null
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Error = null,
            Data = null
        });
    }
    
}