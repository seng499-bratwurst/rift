using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Rift.Models;
using Rift.Services;
using Microsoft.AspNetCore.RateLimiting;

namespace Rift.Controllers;

[ApiController]
[Route("api")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IMessageEdgeService _messageEdgeService;
    private readonly IConversationService _conversationService;
    private readonly IRAGService _ragService;
    private readonly IFileService _fileService;
    private readonly IMessageFilesService _messageFileService;

    public MessageController(
        IMessageService messageService,
        IConversationService conversationService,
        IRAGService ragService,
        IMessageEdgeService messageEdgeService,
        IFileService fileService,
        IMessageFilesService messageFileService
        )
    {
        _messageService = messageService;
        _messageEdgeService = messageEdgeService;
        _ragService = ragService;
        _messageFileService = messageFileService;
        _conversationService = conversationService;
        _fileService = fileService;
    }

    /// <summary>
    /// Endpoint for authenticated and anonymous users (existing behavior).
    /// </summary>
    [HttpPost("messages")]
    [AllowAnonymous]
    [EnableRateLimiting("PerOncApiToken")]
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

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "User ID is required for authenticated requests",
                Data = null
            });
        }

        Conversation? conversation = await _conversationService.GetOrCreateConversationByUserId(userId, request.ConversationId);

        if (conversation == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Error = "Conversation not found.",
                Data = null
            });
        }

        // If userId is null, send the response back without storing it
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ApiResponse<string>
            {
                Success = false,
                Error = "Unauthorized",
            });
        }

        var conversationId = request.ConversationId ?? conversation!.Id;

        var messageHistory = await _messageService.GetMessagesForConversationAsync(userId, conversationId);

        var oncApiToken = User.FindFirst("ONCApiToken")?.Value;

        var (llmResponse, relevantDocTitles) = await _ragService.GenerateResponseAsync(request.Content, messageHistory, oncApiToken);

        var documents = await _fileService.GetFilesByTitlesAsync(relevantDocTitles);

        // Create the message with the users prompt
        var promptMessage = await _messageService.CreateMessageAsync(
            conversationId,
            null,
            request.Content,
            "user",
            request.XCoordinate,
            request.YCoordinate
        );

        // Create the message with the LLM response
        var responseMessage = await _messageService.CreateMessageAsync(
            conversationId,
            promptMessage?.Id,
            llmResponse,
            "assistant",
            request.ResponseXCoordinate,
            request.ResponseYCoordinate
        );

        if (responseMessage == null || promptMessage == null)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Failed to create messages.",
                Data = null
            });
        }

        // Save the files that were used as context for the LLM response message
        await _messageFileService.InsertMessageFilesAsync(documents, responseMessage.Id);
        await _conversationService.UpdateLastInteractionTime(conversationId);

        MessageEdge promptToResponseEdge = await _messageEdgeService.CreateEdgeAsync(new MessageEdge
        {
            SourceMessageId = promptMessage.Id,
            TargetMessageId = responseMessage.Id,
            SourceHandle = request.SourceHandle,
            TargetHandle = request.TargetHandle
        });

        MessageEdge[] edges = Array.Empty<MessageEdge>();

        if (promptMessage?.Id != null && request.Sources != null && request.Sources.Length > 0)
        {
            edges = (await _messageEdgeService.CreateMessageEdgesFromSourcesAsync(
                promptMessage.Id,
                request.Sources
            )).ToArray();
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Error = null,
            Data = new
            {
                ConversationId = conversationId,
                Documents = documents,
                Response = llmResponse,
                PromptMessageId = promptMessage?.Id,
                ResponseMessageId = responseMessage?.Id,
                CreatedEdges = (new[] { promptToResponseEdge }).Concat(edges).ToArray(),
            }
        });
    }

    /// <summary>
    /// Endpoint for unauthenticated users using a temporary UUID session.
    /// </summary>
    [HttpPost("messages/guest")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateGuestMessage([FromBody] CreateMessageRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(new ApiResponse<Message>
            {
                Success = false,
                Error = "Message content cannot be empty.",
                Data = null
            });
        }


        if (string.IsNullOrEmpty(request?.SessionId))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Session ID is required for guest requests",
                Data = null
            });
        }

        var sessionId = request.SessionId;

        // If there is no conversationId, create a new conversation for the session
        Conversation? conversation = await _conversationService.GetConversationsForSessionAsync(sessionId);

        if (conversation == null)
        {
            conversation = await _conversationService.CreateConversationBySessionId(sessionId);
        }

        var conversationId = conversation!.Id;

        string oncApiToken = "{YOUR_ONC_TOKEN}";
        var messageHistory = await _messageService.GetGuestMessagesForConversationAsync(sessionId, conversationId);

        // var llmResponse = await _ragService.GenerateResponseAsync(request.Content, messageHistory);
        var (llmResponse, relevantDocTitles) = await _ragService.GenerateResponseAsync(request.Content, messageHistory, oncApiToken ?? string.Empty);

        var documents = await _fileService.GetFilesByTitlesAsync(relevantDocTitles);

        // Store the user's message
        var promptMessage = await _messageService.CreateMessageAsync(
            conversationId,
            null,
            request.Content,
            "user",
            request.XCoordinate,
            request.YCoordinate
        );

        // Store the LLM's response
        var responseMessage = await _messageService.CreateMessageAsync(
            conversationId,
            promptMessage?.Id,
            llmResponse,
            "assistant",
            request.ResponseXCoordinate,
            request.ResponseYCoordinate
        );

        if (responseMessage == null || promptMessage == null)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Failed to create messages.",
                Data = null
            });
        }

        await _conversationService.UpdateLastInteractionTime(conversationId);

        MessageEdge promptToResponseEdge = await _messageEdgeService.CreateEdgeAsync(new MessageEdge
        {
            SourceMessageId = promptMessage.Id,
            TargetMessageId = responseMessage.Id,
            SourceHandle = request.SourceHandle,
            TargetHandle = request.TargetHandle
        });

        MessageEdge[] edges = [];

        if (promptMessage?.Id != null && request.Sources != null && request.Sources.Length > 0)
        {
            edges = (await _messageEdgeService.CreateMessageEdgesFromSourcesAsync(
                promptMessage.Id,
                request.Sources
            )).ToArray();
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Error = null,
            Data = new
            {
                Response = llmResponse,
                Documents = documents,
                SessionId = sessionId,
                PromptMessageId = promptMessage?.Id,
                ResponseMessageId = responseMessage?.Id,
                CreatedEdges = (new[] { promptToResponseEdge }).Concat(edges).ToArray(),
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

    /// <summary>
    /// PATCH endpoint to update the coordinates of a message.
    /// </summary>
    [HttpPatch("messages/{messageId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> UpdateMessageCoordinates(int messageId, [FromBody] UpdateCoordinatesRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Error = "Unauthorized",
                Data = null
            });
        }

        var updated = await _messageService.UpdateMessageAsync(messageId, userId, request.XCoordinate, request.YCoordinate);

        if (updated == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Error = "Message not found or permission denied.",
                Data = null
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Error = null,
            Data = new { updated.Id, updated.XCoordinate, updated.YCoordinate }
        });
    }

    /// <summary>
    /// PATCH endpoint to update message feedback (thumbs up/down).
    /// </summary>
    [HttpPatch("messages/{messageId}/feedback")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 401)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> UpdateMessageFeedback(int messageId, [FromBody] UpdateFeedbackRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Error = "Unauthorized",
                Data = null
            });
        }

        var updated = await _messageService.UpdateMessageFeedbackAsync(userId, messageId, request.IsHelpful);

        if (updated == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Error = "Message not found or permission denied.",
                Data = null
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Error = null,
            Data = new { updated.Id, updated.IsHelpful }
        });
    }

    [HttpDelete("messages/{messageId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> DeleteMessage(int messageId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ApiResponse<Message>
            {
                Success = false,
                Error = "Unauthorized",
                Data = null
            });
        }

        var message = await _messageService.DeleteMessageAsync(userId, messageId);

        if (message == null)
        {
            return NotFound(new ApiResponse<Message>
            {
                Success = false,
                Error = "Message not found",
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
    public class CreateMessageRequest
    {
        public int? ConversationId { get; set; }
        public string Content { get; set; } = string.Empty;
        public float XCoordinate { get; set; }
        public float YCoordinate { get; set; }
        public float ResponseXCoordinate { get; set; }
        public float ResponseYCoordinate { get; set; }
        public string SourceHandle { get; set; } = string.Empty;
        public string TargetHandle { get; set; } = string.Empty;
        public string? SessionId { get; set; } = null;
        public PartialMessageEdge[]? Sources { get; set; } = null;
    }

    public class UpdateCoordinatesRequest
    {
        public float XCoordinate { get; set; }
        public float YCoordinate { get; set; }
    }

    /// <summary>
    /// Request model for updating message feedback.
    /// </summary>
    public class UpdateFeedbackRequest
    {
        /// <summary>
        /// Indicates whether the message was helpful. True for thumbs up (helpful), false for thumbs down (not helpful).
        /// </summary>
        public bool IsHelpful { get; set; }
    }

}