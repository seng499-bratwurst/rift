using Microsoft.AspNetCore.Mvc;

// This is a temporary Controller to try the communication with the VectorDB.
// The actual communication with the VectorDB will be done through the ChromaDBClient within
// the backend application.

public class VectorDBController : ControllerBase
{
    private readonly ChromaDBClient _chroma;

    public VectorDBController(ChromaDBClient chroma)
    {
        _chroma = chroma;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddDocument([FromBody] AddRequest request)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Text) || string.IsNullOrWhiteSpace(request.Id))
            return BadRequest("Fields must be filled.");

        var response = await _chroma.AddAsync(request);
        return response ? Ok("Document added successfully.") : BadRequest("Failed to add document.");
    }

    [HttpPost("query")]
    public async Task<IActionResult> QueryVectorDB([FromBody] QueryRequest request)
    {
        var result = await _chroma.QueryAsync(request);
        return Ok(result);
    }
}