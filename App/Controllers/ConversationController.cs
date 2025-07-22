using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Rift.Models;

[ApiController]
[Route("api")]
public class ConversationController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpGet("conversations")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetConversations()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ApiResponse<IEnumerable<Conversation>>
            {
                Success = false,
                Error = "Unauthorized",
                Data = null
            });
        }

        var conversations = await _conversationService.GetConversationsForUserAsync(userId);

        return Ok(new ApiResponse<IEnumerable<Conversation>>
        {
            Success = true,
            Error = null,
            Data = conversations
        });
    }

    [HttpDelete("conversations/{conversationId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> DeleteConversation(int conversationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ApiResponse<IEnumerable<Conversation>>
            {
                Success = false,
                Error = "Unauthorized",
                Data = null
            });
        }

        var conversation = await _conversationService.DeleteConversation(userId, conversationId);

        if (conversation == null)
        {
            return NotFound(new ApiResponse<Conversation>
            {
                Success = false,
                Error = "Conversation not found",
                Data = null
            });
        }

        return Ok(new ApiResponse<Conversation>
        {
            Success = true,
            Error = null,
            Data = null
        });
    }

    [HttpPatch("conversations/{conversationId}/title")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> UpdateConversationTitle(int conversationId, [FromBody] UpdateTitleRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ApiResponse<Conversation>
            {
                Success = false,
                Error = "Unauthorized",
                Data = null
            });
        }

        // Validate the title is not empty
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new ApiResponse<Conversation>
            {
                Success = false,
                Error = "Title cannot be empty",
                Data = null
            });
        }

        // First verify the user owns this conversation
        var existingConversation = await _conversationService.GetConversationById(userId, conversationId);
        if (existingConversation == null)
        {
            return NotFound(new ApiResponse<Conversation>
            {
                Success = false,
                Error = "Conversation not found",
                Data = null
            });
        }

        // Update the title
        var updatedConversation = await _conversationService.UpdateConversationTitle(conversationId, request.Title);

        if (updatedConversation == null)
        {
            return BadRequest(new ApiResponse<Conversation>
            {
                Success = false,
                Error = "Failed to update conversation title",
                Data = null
            });
        }

        return Ok(new ApiResponse<Conversation>
        {
            Success = true,
            Error = null,
            Data = updatedConversation
        });
    }

    /// <summary>
    /// Request model for updating conversation title.
    /// </summary>
    public class UpdateTitleRequest
    {
        /// <summary>
        /// The new title for the conversation.
        /// </summary>
        public string Title { get; set; } = string.Empty;
    }
}