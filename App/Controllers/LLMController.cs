using Microsoft.AspNetCore.Mvc;
using Rift.Models;         // For PromptRequest model
using Rift.LLM;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Rift.Services;            // For ILlmProvider interface

namespace Rift.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LLMController : ControllerBase
    {
        private readonly ILlmProvider _llmProvider;
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _jwtTokenService;

        public LLMController(
            ILlmProvider llmProvider,
            UserManager<User> userManager,
            IConfiguration configuration
        )
        {
            _llmProvider = llmProvider;
            _userManager = userManager;
            _jwtTokenService = new JwtTokenService(configuration);

        }

        [HttpPost("ask")]
        [AllowAnonymous]
        public async Task<IActionResult> Ask([FromBody] PromptRequest request)
        {
            string userId = _jwtTokenService.GetUserIdFromBearerToken(Request.Headers.Authorization.ToString());

            // If there is a user, store the message and the response the the database
            // If there is a conversation_id, use that otherwise create a new conversation

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt cannot be empty.");

            var response = await _llmProvider.GenerateResponseAsync(request.Prompt);
            return Ok(new { response });
        }

        // [HttpGet("messages")]
        // [Authorize(AuthenticationSchemes = "Bearer")]
        // public IActionResult GetMessages()
        // {
        //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //     // if (string.IsNullOrEmpty(userId))
        //     // {
        //     //     return Unauthorized(new ApiResponse<IEnumerable<Conversation>>
        //     //     {
        //     //         Success = false,
        //     //         Error = "Unauthorized",
        //     //         Data = null
        //     //     });
        //     // }

        //     // var conversations = await _conversationService.GetConversationsForUserAsync(userId);

        //     return Ok(new ApiResponse<IEnumerable<Conversation>>
        //     {
        //         Success = true,
        //         Error = null,
        //         Data = null
        //     });
        // }
    }
}
