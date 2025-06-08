using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Rift.Models;
using Rift.Services;

namespace Rift.Controllers;

[ApiController]
[Route("api")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public class CreateMessageRequest
    {
        public int? ConversationId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? OncApiQuery { get; set; }
        public string? OncApiResponse { get; set; }
        public bool? IsHelpful { get; set; }
    }

    [HttpPost("messages")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateMessage([FromBody] CreateMessageRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(new ApiResponse<Message>
            {
                Success = false,
                Error = "Message content cannot be empty.",
                Data = null
            });
        }

        if (!string.IsNullOrEmpty(userId))
        {
            var message = await _messageService.CreateMessageAsync(
                request.ConversationId,
                request.Content,
                request.OncApiQuery,
                request.OncApiResponse,
                request.IsHelpful
            );

            if (message == null)
            {
                return BadRequest(new ApiResponse<Message>
                {
                    Success = false,
                    Error = "Failed to create message.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<Message>
            {
                Success = true,
                Error = null,
                Data = message
            });
        }
        else
        {
            var tempMessage = new Message
            {
                ConversationId = request.ConversationId,
                Content = request.Content,
                OncApiQuery = request.OncApiQuery,
                OncApiResponse = request.OncApiResponse,
                IsHelpful = request.IsHelpful,
                CreatedAt = DateTime.UtcNow
            };

            return Ok(new ApiResponse<Message>
            {
                Success = true,
                Error = null,
                Data = tempMessage
            });
        }
    }

    [HttpGet("conversations/{conversationId}/messages")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetMessages(int conversationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ApiResponse<List<Message>>
            {
                Success = false,
                Error = "Unauthorized",
                Data = null
            });
        }

        var messages = await _messageService.GetMessagesForConversationAsync(conversationId);

        return Ok(new ApiResponse<List<Message>>
        {
            Success = true,
            Error = null,
            Data = messages
        });
    }
}