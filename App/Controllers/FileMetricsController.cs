using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rift.Services;
using Rift.Models;

namespace Rift.Controllers;

[ApiController]
[Route("api")]
[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
public class FileMetricsController : ControllerBase
{
    private readonly IFileMetricsService _fileMetricsService;

    public FileMetricsController(IFileMetricsService fileMetricsService)
    {
        _fileMetricsService = fileMetricsService;
    }

    /// <summary>
    /// Get file metrics.
    /// </summary>
    [HttpGet("metrics")]
    public async Task<IActionResult> GetFileMetrics()
    {
        var metrics = await _fileMetricsService.GetFileMetricsAsync();
        return Ok(new ApiResponse<IEnumerable<FileMetric>>
        {
            Success = true,
            Error = null,
            Data = metrics
        });
    }
}

