using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rift.App.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReRankerController : ControllerBase
{
    private readonly ReRankerClient _reRankerClient;
    private readonly ILogger<ReRankerController> _logger;

    public ReRankerController(ReRankerClient reRankerClient, ILogger<ReRankerController> logger)
    {
        _reRankerClient = reRankerClient;
        _logger = logger;
    }

    public class RerankRequestDto
    {
        [Required]
        public string Query { get; set; }
        [Required]
        public List<string> Docs { get; set; }
    }

    /// <summary>
    /// Re-rank a list of documents based on a query using the Python reRanker microservice.
    /// </summary>
    [HttpPost("rerank")]
    [ProducesResponseType(typeof(RerankResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Rerank([FromBody] RerankRequestDto request)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Query) || request.Docs == null || !request.Docs.Any())
        {
            return BadRequest("Query and non-empty docs list are required.");
        }

        try
        {
            var result = await _reRankerClient.RerankAsync(new RerankRequest
            {
                Query = request.Query,
                Docs = request.Docs
            });
            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "ReRanker microservice error");
            return StatusCode(StatusCodes.Status502BadGateway, $"ReRanker microservice error: {ex.Message}");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error calling reRanker microservice");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred while calling reRanker microservice.");
        }
    }

    /// <summary>
    /// Test endpoint for the reRanker microservice.
    /// </summary>
    [HttpGet("rerank")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Test()
    {
        try
        {
            var json = await _reRankerClient.TestAsync();
            return Ok(JsonDocument.Parse(json));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "ReRanker microservice error");
            return StatusCode(StatusCodes.Status502BadGateway, $"ReRanker microservice error: {ex.Message}");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error calling reRanker microservice test endpoint");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred while calling reRanker microservice.");
        }
    }
}