using Microsoft.AspNetCore.Mvc;

namespace Rift.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RAGController : ControllerBase
    {
        private readonly RAGService _ragService;

        public RAGController(RAGService ragService)
        {
            _ragService = ragService;
        }

        [HttpPost("generate-response")]
        public async Task<IActionResult> GenerateResponse([FromBody] string userQuery)
        {
            if (string.IsNullOrWhiteSpace(userQuery))
            {
                return BadRequest("User query cannot be empty.");
            }

            try
            {
                var response = await _ragService.GenerateResponseAsync(userQuery);
                return Ok(new { response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}