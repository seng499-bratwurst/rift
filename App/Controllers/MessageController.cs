using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Rift.Models;
using Rift.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

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
    private readonly IGeminiTitleService? _geminiTitleService;
    private readonly IGroup2CompanyService _group2CompanyService;

    public MessageController(
        IMessageService messageService,
        IConversationService conversationService,
        IRAGService ragService,
        IMessageEdgeService messageEdgeService,
        IFileService fileService,
        IMessageFilesService messageFileService,
        IGroup2CompanyService group2CompanyService,
        IGeminiTitleService? geminiTitleService = null
        )
    {
        _messageService = messageService;
        _messageEdgeService = messageEdgeService;
        _ragService = ragService;
        _messageFileService = messageFileService;
        _conversationService = conversationService;
        _fileService = fileService;
        _geminiTitleService = geminiTitleService;
        _group2CompanyService = group2CompanyService;
    }

    /// <summary>
    /// Endpoint for authenticated and anonymous users (existing behavior).
    /// </summary>
    [HttpPost("messages")]
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

        var oncApiToken = User.FindFirst("ONCApiToken")?.Value ?? string.Empty;

        // if Company is Group2Company
        if (request.Company == (int)Companies.Group2Company)
        {
            // Use Group2Company service to get response
            var (group2Response, responseType) = await _group2CompanyService.GetResponseAsync(request.Content, messageHistory, oncApiToken);

            // Create the user prompt message
            var promptMessage = await _messageService.CreateMessageAsync(
                conversationId,
                null,
                request.Content,
                "user",
                request.XCoordinate,
                request.YCoordinate
            );

            // Create the assistant response message
            var responseMessage = await _messageService.CreateMessageAsync(
                conversationId,
                promptMessage?.Id,
                group2Response,
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
            await GenerateConversationTitleIfNeeded(conversationId, request.Content, group2Response);

            // Create edge between prompt and response
            MessageEdge promptToResponseEdge = await _messageEdgeService.CreateEdgeAsync(new MessageEdge
            {
                SourceMessageId = promptMessage.Id,
                TargetMessageId = responseMessage.Id,
                SourceHandle = request.SourceHandle,
                TargetHandle = request.TargetHandle
            });

            // Handle source edges if provided
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
                    Documents = (object?)null, // Group2Company service doesn't return documents
                    Response = group2Response,
                    PromptMessageId = promptMessage?.Id,
                    ResponseMessageId = responseMessage?.Id,
                    CreatedEdges = (new[] { promptToResponseEdge }).Concat(edges).ToArray(),
                }
            });
        }
        else
        {
            var (llmResponse, relevantDocs) = await _ragService.GenerateResponseAsync(request.Content, messageHistory, oncApiToken);

            var documents = await _fileService.GetFilesByTitlesAsync(relevantDocs.Select(doc => doc.SourceId).ToList());

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

            await GenerateConversationTitleIfNeeded(conversationId, request.Content, llmResponse);

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
                    Documents = relevantDocs,
                    Response = llmResponse,
                    PromptMessageId = promptMessage?.Id,
                    ResponseMessageId = responseMessage?.Id,
                    CreatedEdges = (new[] { promptToResponseEdge }).Concat(edges).ToArray(),
                }
            });
        }
    }

    /// <summary>
    /// Streaming endpoint for authenticated users that streams LLM responses in real-time.
    /// </summary>
    [HttpPost("messages/stream")]
    public async Task<IActionResult> CreateStreamingMessage([FromBody] CreateMessageRequest request)
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

        var conversationId = request.ConversationId ?? conversation!.Id;
        var messageHistory = await _messageService.GetMessagesForConversationAsync(userId, conversationId);
        var oncApiToken = User.FindFirst("ONCApiToken")?.Value ?? string.Empty;

        // Create the message with the user's prompt first
        var promptMessage = await _messageService.CreateMessageAsync(
            conversationId,
            null,
            request.Content,
            "user",
            request.XCoordinate,
            request.YCoordinate
        );

        if (promptMessage == null)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Failed to create prompt message.",
                Data = null
            });
        }

        // Set up Server-Sent Events
        Response.Headers["Content-Type"] = "text/event-stream";
        Response.Headers["Cache-Control"] = "no-cache";
        Response.Headers["Connection"] = "keep-alive";
        Response.Headers["Access-Control-Allow-Origin"] = "*";

        await Response.Body.FlushAsync();

        var fullResponse = new StringBuilder();
        List<App.Models.DocumentChunk> relevantDocs = new();

        try
        {
            await foreach (var (chunk, relevantDoc) in _ragService.StreamResponseAsync(request.Content, messageHistory, oncApiToken))
            {
                if (HttpContext.RequestAborted.IsCancellationRequested)
                    break;

                relevantDocs = relevantDoc;
                fullResponse.Append(chunk);

                var eventData = new
                {
                    type = "chunk",
                    data = chunk,
                    promptMessageId = promptMessage.Id
                };

                var jsonData = JsonSerializer.Serialize(eventData);
                await Response.WriteAsync($"data: {jsonData}\n\n");
                await Response.Body.FlushAsync();
            }

            // Create the response message with the complete response
            var responseMessage = await _messageService.CreateMessageAsync(
                conversationId,
                promptMessage.Id,
                fullResponse.ToString(),
                "assistant",
                request.ResponseXCoordinate,
                request.ResponseYCoordinate
            );

            if (responseMessage != null)
            {
                var documents = await _fileService.GetFilesByTitlesAsync(relevantDocs.Select(doc => doc.SourceId).ToList());
                await _messageFileService.InsertMessageFilesAsync(documents, responseMessage.Id);
                await _conversationService.UpdateLastInteractionTime(conversationId);

                await GenerateConversationTitleIfNeeded(conversationId, request.Content, fullResponse.ToString());

                MessageEdge promptToResponseEdge = await _messageEdgeService.CreateEdgeAsync(new MessageEdge
                {
                    SourceMessageId = promptMessage.Id,
                    TargetMessageId = responseMessage.Id,
                    SourceHandle = request.SourceHandle,
                    TargetHandle = request.TargetHandle
                });

                MessageEdge[] edges = Array.Empty<MessageEdge>();

                if (request.Sources != null && request.Sources.Length > 0)
                {
                    edges = (await _messageEdgeService.CreateMessageEdgesFromSourcesAsync(
                        promptMessage.Id,
                        request.Sources
                    )).ToArray();
                }

                // Send completion event
                var completionData = new
                {
                    type = "complete",
                    data = new
                    {
                        conversationId = conversationId,
                        documents = relevantDocs,
                        response = fullResponse.ToString(),
                        promptMessageId = promptMessage.Id,
                        responseMessageId = responseMessage.Id,
                        createdEdges = (new[] { promptToResponseEdge }).Concat(edges).ToArray(),
                    }
                };

                Console.WriteLine($"[STREAMING DEBUG - Auth] Sending completion with {documents.Count()} documents");
                var completionJson = JsonSerializer.Serialize(completionData);
                await Response.WriteAsync($"data: {completionJson}\n\n");
                await Response.Body.FlushAsync();
            }

            await Response.WriteAsync("data: [DONE]\n\n");
            await Response.Body.FlushAsync();
        }
        catch (Exception ex)
        {
            var errorData = new
            {
                type = "error",
                error = ex.Message
            };

            var errorJson = JsonSerializer.Serialize(errorData);
            await Response.WriteAsync($"data: {errorJson}\n\n");
            await Response.Body.FlushAsync();
        }

        return new EmptyResult();
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

        string oncApiToken = string.Empty; // Use empty string to fall back to system token
        var messageHistory = await _messageService.GetGuestMessagesForConversationAsync(sessionId, conversationId);

        // var llmResponse = await _ragService.GenerateResponseAsync(request.Content, messageHistory);
        var (llmResponse, relevantDocs) = await _ragService.GenerateResponseAsync(request.Content, messageHistory, oncApiToken ?? string.Empty);

        var documents = await _fileService.GetFilesByTitlesAsync(relevantDocs.Select(doc => doc.SourceId).ToList());

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

        await GenerateConversationTitleIfNeeded(conversationId, request.Content, llmResponse);

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
                Documents = relevantDocs,
                SessionId = sessionId,
                PromptMessageId = promptMessage?.Id,
                ResponseMessageId = responseMessage?.Id,
                CreatedEdges = (new[] { promptToResponseEdge }).Concat(edges).ToArray(),
            }
        });
    }

    /// <summary>
    /// Streaming endpoint for guest users using a temporary UUID session.
    /// </summary>
    [HttpPost("messages/guest/stream")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateGuestStreamingMessage([FromBody] CreateMessageRequest request)
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
        var messageHistory = await _messageService.GetGuestMessagesForConversationAsync(sessionId, conversationId);
        var oncApiToken = string.Empty; // Guests don't have ONC tokens

        // Create the message with the user's prompt first
        var promptMessage = await _messageService.CreateMessageAsync(
            conversationId,
            null,
            request.Content,
            "user",
            request.XCoordinate,
            request.YCoordinate
        );

        if (promptMessage == null)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Failed to create prompt message.",
                Data = null
            });
        }

        // Set up Server-Sent Events
        Response.Headers["Content-Type"] = "text/event-stream";
        Response.Headers["Cache-Control"] = "no-cache";
        Response.Headers["Connection"] = "keep-alive";
        Response.Headers["Access-Control-Allow-Origin"] = "*";

        await Response.Body.FlushAsync();

        var fullResponse = new StringBuilder();
        List<string> relevantDocTitles = new();

        try
        {
            await foreach (var (chunk, relevantDocs) in _ragService.StreamResponseAsync(request.Content, messageHistory, oncApiToken))
            {
                if (HttpContext.RequestAborted.IsCancellationRequested)
                    break;

                relevantDocTitles = relevantDocs.Select(doc => doc.SourceId).ToList();
                fullResponse.Append(chunk);

                var eventData = new
                {
                    type = "chunk",
                    data = chunk,
                    promptMessageId = promptMessage.Id
                };

                var jsonData = JsonSerializer.Serialize(eventData);
                await Response.WriteAsync($"data: {jsonData}\n\n");
                await Response.Body.FlushAsync();
            }

            // Create the response message with the complete response
            var responseMessage = await _messageService.CreateMessageAsync(
                conversationId,
                promptMessage.Id,
                fullResponse.ToString(),
                "assistant",
                request.ResponseXCoordinate,
                request.ResponseYCoordinate
            );

            if (responseMessage != null)
            {
                Console.WriteLine($"[STREAMING DEBUG - Guest] RAG service returned {relevantDocTitles.Count} document titles");
                var documents = await _fileService.GetFilesByTitlesAsync(relevantDocTitles);
                Console.WriteLine($"[STREAMING DEBUG - Guest] File service returned {documents.Count()} documents");
                await _messageFileService.InsertMessageFilesAsync(documents, responseMessage.Id);
                await _conversationService.UpdateLastInteractionTime(conversationId);

                await GenerateConversationTitleIfNeeded(conversationId, request.Content, fullResponse.ToString());

                MessageEdge promptToResponseEdge = await _messageEdgeService.CreateEdgeAsync(new MessageEdge
                {
                    SourceMessageId = promptMessage.Id,
                    TargetMessageId = responseMessage.Id,
                    SourceHandle = request.SourceHandle,
                    TargetHandle = request.TargetHandle
                });

                MessageEdge[] edges = Array.Empty<MessageEdge>();

                if (request.Sources != null && request.Sources.Length > 0)
                {
                    edges = (await _messageEdgeService.CreateMessageEdgesFromSourcesAsync(
                        promptMessage.Id,
                        request.Sources
                    )).ToArray();
                }

                // Send completion event
                var completionData = new
                {
                    type = "complete",
                    data = new
                    {
                        response = fullResponse.ToString(),
                        documents = documents,
                        sessionId = sessionId,
                        promptMessageId = promptMessage.Id,
                        responseMessageId = responseMessage.Id,
                        createdEdges = (new[] { promptToResponseEdge }).Concat(edges).ToArray(),
                    }
                };

                Console.WriteLine($"[STREAMING DEBUG - Guest] Sending completion with {documents.Count()} documents");
                var completionJson = JsonSerializer.Serialize(completionData);
                await Response.WriteAsync($"data: {completionJson}\n\n");
                await Response.Body.FlushAsync();
            }

            await Response.WriteAsync("data: [DONE]\n\n");
            await Response.Body.FlushAsync();
        }
        catch (Exception ex)
        {
            var errorData = new
            {
                type = "error",
                error = ex.Message
            };

            var errorJson = JsonSerializer.Serialize(errorData);
            await Response.WriteAsync($"data: {errorJson}\n\n");
            await Response.Body.FlushAsync();
        }

        return new EmptyResult();
    }

    /// <summary>
    /// Endpoint for external companies to use our LLM and RAG services.
    /// Requires company token authentication via X-Company-Token header.
    /// Optionally accepts message history to provide conversation context.
    /// </summary>
    [HttpPost("messages/company")]
    [AllowAnonymous]
    [EnableRateLimiting("PerCompanyToken")]
    public async Task<IActionResult> CreateCompanyMessage([FromBody] CreateCompanyMessageRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Error = "Message content cannot be empty.",
                Data = null
            });
        }

        // Validate message history if provided
        if (request.MessageHistory != null)
        {
            foreach (var historyItem in request.MessageHistory)
            {
                if (string.IsNullOrWhiteSpace(historyItem.Role) || 
                    (historyItem.Role != "user" && historyItem.Role != "assistant"))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Error = "Message history role must be either 'user' or 'assistant'.",
                        Data = null
                    });
                }
                
                if (string.IsNullOrWhiteSpace(historyItem.Content))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Error = "Message history content cannot be empty.",
                        Data = null
                    });
                }
            }
        }

        // Get company token from header
        string? companyToken = Request.Headers["X-Company-Token"].FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(companyToken))
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Error = "Company token is required in X-Company-Token header.",
                Data = null
            });
        }

        // Validate company token exists in database
        using var scope = HttpContext.RequestServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var companyRecord = await dbContext.CompanyAPITokens.FirstOrDefaultAsync(t => t.ONCApiToken == companyToken);
        
        if (companyRecord == null)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Error = "Invalid company token.",
                Data = null
            });
        }

        try
        {
            // Log company API usage for monitoring
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var tokenHash = BitConverter.ToString(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(companyToken ?? ""))).Replace("-", "");
                Console.WriteLine($"Company API request from token hash: {tokenHash} Content length: {request.Content.Length}");
            }
            
            // Generate LLM response using RAG service
            // Use provided message history or empty list if not provided
            var messageHistory = ConvertToMessageList(request.MessageHistory);
            var (llmResponse, relevantDocs) = await _ragService.GenerateResponseAsync(request.Content, messageHistory, "{YOUR_ONC_TOKEN}");

            var documents = await _fileService.GetFilesByTitlesAsync(relevantDocs.Select(doc => doc.SourceId).ToList());

            Console.WriteLine($"Company API response generated. Documents found: {documents.Count()}");

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Error = null,
                Data = new
                {
                    Response = llmResponse,
                    Documents = relevantDocs,
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing company message: {ex.Message}");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Error = "An error occurred while processing your request.",
                Data = null
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

    private async Task GenerateConversationTitleIfNeeded(int conversationId, string userPrompt, string assistantResponse)
    {
        try
        {
            // First, check if the conversation already has a title
            var conversation = await _conversationService.GetConversationByIdOnly(conversationId);
            
            if (conversation != null && !string.IsNullOrEmpty(conversation.Title))
            {
                // Title already exists, don't override it
                return;
            }

            // Generate new title using Gemini
            if (_geminiTitleService != null)
            {
                var generatedTitle = await _geminiTitleService.GenerateTitleAsync(userPrompt, assistantResponse);
                
                // Only update if we get a non-fallback title
                if (!string.IsNullOrEmpty(generatedTitle) && generatedTitle != "New Conversation")
                {
                    await _conversationService.UpdateConversationTitle(conversationId, generatedTitle);
                    Console.WriteLine($"Generated and stored conversation title: {generatedTitle}");
                }
            }
            else
            {
                // No Gemini service available, use default title
                await _conversationService.UpdateConversationTitle(conversationId, "New Conversation");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to generate conversation title: {ex.Message}");
        }
    }

    /// <summary>
    /// Converts company message history items to Message objects for RAG service.
    /// </summary>
    private List<Message> ConvertToMessageList(List<CompanyMessageHistoryItem>? messageHistory)
    {
        if (messageHistory == null || !messageHistory.Any())
        {
            return new List<Message>();
        }

        return messageHistory.Select((item, index) => new Message
        {
            Id = index + 1, // Temporary ID for processing
            ConversationId = 0, // Not stored, just for processing
            PromptMessageId = null,
            Content = item.Content,
            Role = item.Role,
            CreatedAt = DateTime.UtcNow,
            XCoordinate = 0,
            YCoordinate = 0
        }).ToList();
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
        public int? Company { get; set; } = 0; // 0 = BratwurstCompany (default), 1 = Group2Company
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

    /// <summary>
    /// Request model for company message endpoint.
    /// </summary>
    public class CreateCompanyMessageRequest
    {
        /// <summary>
        /// The user's message content.
        /// </summary>
        public string Content { get; set; } = string.Empty;
        
        /// <summary>
        /// Optional conversation history to provide context for the LLM response.
        /// Should be ordered chronologically with alternating user/assistant messages.
        /// </summary>
        public List<CompanyMessageHistoryItem>? MessageHistory { get; set; } = null;
    }

    /// <summary>
    /// Represents a message history item for company requests.
    /// </summary>
    public class CompanyMessageHistoryItem
    {
        /// <summary>
        /// The role of the message sender. Should be either "user" or "assistant".
        /// </summary>
        public string Role { get; set; } = string.Empty;
        
        /// <summary>
        /// The content of the message.
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }

}