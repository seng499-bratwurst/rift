using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rift.Models;
using System.Runtime.Serialization;
using System.Reflection;

namespace Rift.Controllers;

[ApiController]
[Route("api/source-types")]
public class SourceTypeController : ControllerBase
{

    [HttpGet("")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public ActionResult<IEnumerable<SourceTypeDto>> GetSourceTypes()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId) || !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var sourceTypeOptions = Enum.GetValues<SourceTypes>()
         .Cast<SourceTypes>()
         .Select(e => new SourceTypeDto
         {
             DisplayName = GetDisplayName(e),
             Value = GetEnumMemberValue(e)
         })
         .ToArray();

        return Ok(new ApiResponse<IEnumerable<SourceTypeDto>>
        {
            Success = true,
            Error = null,
            Data = sourceTypeOptions
        });
    }

    private static string GetEnumMemberValue(SourceTypes value)
    {
        var memberInfo = typeof(SourceTypes).GetMember(value.ToString()).FirstOrDefault();
        var enumMemberAttribute = memberInfo?.GetCustomAttribute<EnumMemberAttribute>(false);
        return enumMemberAttribute?.Value ?? value.ToString() ?? string.Empty;
    }

    private static string GetDisplayName(SourceTypes value)
    {
        var strValue = value.ToString();
        return System.Text.RegularExpressions.Regex.Replace(
            strValue, "(\\B[A-Z])", " $1");
    }

    public class UploadFileRequest
    {
        public required IFormFile File { get; set; }
        public required string SourceLink { get; set; }
        public required string SourceType { get; set; }
    }
}