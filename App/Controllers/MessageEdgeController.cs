using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rift.Models;
using Rift.Services;

namespace Rift.Controllers;

[ApiController]
[Route("api/edges")]
public class MessageEdgeController : ControllerBase
{
    private readonly IMessageEdgeService _messageEdgeService;

    public MessageEdgeController(IMessageEdgeService messageEdgeService)
    {
        _messageEdgeService = messageEdgeService;
    }

    [HttpPost("")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> CreateEdge([FromBody] MessageEdge edge)
    {
        var result = await _messageEdgeService.CreateEdgeAsync(edge);

        if (result == null)
        {
            return BadRequest(new ApiResponse<MessageEdge>
            {
                Success = false,
                Error = "Failed to create edge.",
                Data = null
            });
        }

        return Ok(new ApiResponse<MessageEdge>
        {
            Success = true,
            Error = null,
            Data = result
        });
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> DeleteEdge(int id)
    {
        var result = await _messageEdgeService.DeleteEdgeAsync(id);

        if (result == null)
        {
            return NotFound(new ApiResponse<int?>
            {
                Success = false,
                Error = "Edge not found.",
                Data = null
            });
        }
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Error = null,
            Data = new
            {
                DeletedId = result
            }
        });
    }
}