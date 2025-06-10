using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Rift.Models;
using Rift.Services;
using Rift.LLM;

namespace Rift.Controllers;

[ApiController]
[Route("api")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IConversationService _conversationService;
    private readonly ILlmProvider _llmProvider;

    public MessageController(IMessageService messageService, IConversationService conversationService, ILlmProvider llmProvider)
    {
        _messageService = messageService;
        _llmProvider = llmProvider;
        _conversationService = conversationService;
    }

    public class CreateMessageRequest
    {
        public int? ConversationId { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    // Helper to get or create a temporary session UUID (24-hour)
    private string GetOrCreateSessionId()
    {
        if (Request.Headers.TryGetValue("Temporary-Session-Id", out var sessionId) && Guid.TryParse(sessionId, out _))
        {
            return sessionId!;
        }
        var newSessionId = Guid.NewGuid().ToString();
        Response.Headers["Temporary-Session-Id"] = newSessionId;

        return newSessionId;
    }

    /// <summary>
    /// Endpoint for authenticated and anonymous users (existing behavior).
    /// </summary>
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

        var response = await _llmProvider.GenerateResponseAsync(request.Content);

        // If userId is null, send the response back without storing it
        if (string.IsNullOrEmpty(userId))
        {
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Error = null,
                Data = response
            });
        }

        // If there is no conversationId, create a new conversation
        Conversation? conversation = null;
        if (request.ConversationId == null)
        {
            conversation = await _conversationService.CreateConversationByUserId(userId);
        }

        var conversationId = request.ConversationId ?? conversation?.Id;

        // Create the message with the users prompt
        var promptMessage = await _messageService.CreateMessageAsync(
            conversationId,
            null,
            request.Content,
            "user"
        );

        // Create the message with the LLM response
        await _messageService.CreateMessageAsync(
            conversationId,
            promptMessage?.Id,
            response,
            "assistant"
        );

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Error = null,
            Data = response
        });
    }

    /// <summary>
    /// Endpoint for unauthenticated users using a temporary UUID session.
    /// </summary>
    [HttpPost("messages/guest")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateGuestMessage([FromBody] CreateMessageRequest request)
    {
        var sessionId = GetOrCreateSessionId();

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(new ApiResponse<Message>
            {
                Success = false,
                Error = "Message content cannot be empty.",
                Data = null
            });
        }

        var response = await _llmProvider.GenerateResponseAsync(request.Content);
        
        // If there is no conversationId, create a new conversation for the session
        Conversation? conversation = await _conversationService.GetConversationsForSessionAsync(sessionId);

        if (conversation == null)
        {
            conversation = await _conversationService.CreateConversationBySessionId(sessionId);
        }

        var conversationId = request.ConversationId ?? conversation?.Id;

        // Store the user's message
        var promptMessage = await _messageService.CreateMessageAsync(
            conversationId,
            null,
            request.Content,
            "user"
        );

        // Store the LLM's response
        var assistantMessage = await _messageService.CreateMessageAsync(
            conversationId,
            promptMessage?.Id,
            response,
            "assistant"
        );

        // Return the LLM response and the session UUID (for client to persist)
        Response.Headers["Temporary-Session-Id"] = sessionId;
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Error = null,
            Data = new
            {
                Response = response,
                SessionId = sessionId
            }
        });
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

        var messages = await _messageService.GetMessagesForConversationAsync(userId, conversationId);

        return Ok(new ApiResponse<List<Message>>
        {
            Success = true,
            Error = null,
            Data = messages
        });
    }

}