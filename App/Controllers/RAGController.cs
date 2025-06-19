using Microsoft.AspNetCore.Mvc;
using Rift.Services;

namespace Rift.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RAGController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly RAGService _ragService;

        public RAGController(RAGService ragService, JwtTokenService jwtTokenService)

        {
            _ragService = ragService;
            _jwtTokenService = jwtTokenService;

        }

        public class GenerateResponseRequest
        {
            public string UserQuery { get; set; } = string.Empty;
            public string ConversationIdStr { get; set; } = string.Empty;
        }

    //     [HttpPost("generate-response")]
    //     public async Task<IActionResult> GenerateResponse([FromBody] GenerateResponseRequest request)
    //     {
    //         if (string.IsNullOrWhiteSpace(request.UserQuery))
    //         {
    //             return BadRequest("User query cannot be empty.");
    //         }
    //         // Not sure if this should be the conversation ID or session ID?
    //         if (string.IsNullOrWhiteSpace(request.ConversationIdStr))
    //         {
    //             return BadRequest("Conversation ID cannot be empty.");
    //         }
    //         // Validate and convert conversationIdStr to an integer
    //         if (!int.TryParse(request.ConversationIdStr, out int conversationId))
    //         {
    //             return BadRequest("Invalid conversation ID format.");
    //         }

    //         // TEMPORARY: Use a hardcoded user ID for testing purposes
    //         var userId = "1234";

    //         // var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
    //         // var userId = _jwtTokenService.GetUserIdFromBearerToken(authHeader);
    //         // if (string.IsNullOrEmpty(userId))
    //         // {
    //         //     return Unauthorized("Could not retrieve user ID from token.");
    //         // }

    //         try
    //         {
    //             var response = await _ragService.GenerateResponseAsync(request.UserQuery, userId, conversationId);
    //             return Ok(new { response });
    //         }
    //         catch (Exception ex)
    //         {
    //             return StatusCode(500, $"Internal server error: {ex.Message}");
    //         }
    //     }
    }
}
