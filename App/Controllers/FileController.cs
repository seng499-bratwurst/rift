using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rift.Models;
using Rift.Services;

namespace Rift.Controllers;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId) || !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var fileContents = await _fileService.ReadFileContentAsync(file);

        var fileEntity = new FileEntity
        {
            FileName = file.FileName,
            Content = fileContents,
            Size = file.Length,
            CreatedAt = DateTime.UtcNow,
            UploadedBy = userId
        };

        var result = await _fileService.UploadFileAsync(fileEntity);
        return Ok(new ApiResponse<int>
        {
            Success = true,
            Error = null,
            Data = result.Id
        });
    }

    [HttpGet("")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<IEnumerable<FileEntityDto>>> GetAllFiles()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId) || !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var files = await _fileService.GetAllFilesAsync();

        return Ok(new ApiResponse<IEnumerable<FileEntityDto>>
        {
            Success = true,
            Error = null,
            Data = files
        });
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> DeleteFile(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId) || !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var deleted = await _fileService.DeleteFileByIdAsync(id);

        if (deleted == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Error = "File not found",
                Data = new
                {
                    DeletedId = deleted
                }
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Error = null,
            Data = new
            {
                DeletedId = deleted
            }
        });
    }
}