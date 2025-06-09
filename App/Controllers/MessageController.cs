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
            conversation = await _conversationService.CreateConversation(userId);
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

    // [HttpPatch("messages/{messageId}")]
    // [Authorize(AuthenticationSchemes = "Bearer")]
    // public async Task<IActionResult> UpdateHelpfulness(int messageId)
    // {
    //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    //     if (string.IsNullOrEmpty(userId))
    //     {
    //         return Unauthorized(new ApiResponse<List<Message>>
    //         {
    //             Success = false,
    //             Error = "Unauthorized",
    //             Data = null
    //         });
    //     }

    //     var messages = await _messageService.GetMessagesForConversationAsync(conversationId);

    //     return Ok(new ApiResponse<List<Message>>
    //     {
    //         Success = true,
    //         Error = null,
    //         Data = messages
    //     });
    // }
}