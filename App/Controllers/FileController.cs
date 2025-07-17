using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rift.Models;
using Rift.Services;

namespace Rift.Controllers;

[ApiController]
[Route("api/files")]
[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("")]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId) || !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        if (request == null || request.File == null || request.File.Length == 0 ||
            string.IsNullOrWhiteSpace(request.SourceLink) ||
            string.IsNullOrWhiteSpace(request.SourceType))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Invalid file upload request.",
                Data = null
            });
        }

        var file = request.File;

        var fileContents = await _fileService.ExtractTextAsync(file);

        var fileEntity = new FileEntity
        {
            Name = file.FileName,
            Content = fileContents,
            Size = file.Length,
            CreatedAt = DateTime.UtcNow,
            UploadedBy = userId,
            SourceLink = request.SourceLink,
            SourceType = request.SourceType
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

    public class UploadFileRequest
    {
        public required IFormFile File { get; set; }
        public required string SourceLink { get; set; }
        public required string SourceType { get; set; }
    }
}