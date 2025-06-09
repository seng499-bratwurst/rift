using Microsoft.AspNetCore.Mvc;
using Rift.App.Clients;
using Rift.App.Models;
using System.ComponentModel.DataAnnotations;

namespace Rift.App.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class VectorDBController : ControllerBase
{
    private readonly ChromaDBClient _chromaClient;
    private readonly ILogger<VectorDBController> _logger;

    public VectorDBController(ChromaDBClient chromaClient, ILogger<VectorDBController> logger)
    {
        _chromaClient = chromaClient;
        _logger = logger;
    }

    #region Document Management

    /// <summary>
    /// Add a single document to the vector database
    /// </summary>
    /// <param name="request">Document to add</param>
    /// <returns>Success status</returns>
    [HttpPost("documents")]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDocument([FromBody] AddRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Invalid request data",
                Message = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
            });
        }

        if (string.IsNullOrWhiteSpace(request.Text) || string.IsNullOrWhiteSpace(request.Id))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "ID and Text fields are required"
            });
        }

        try
        {
            var success = await _chromaClient.AddDocumentAsync(request);
            
            if (success)
            {
                return Ok(new VectorDBResponse<string>
                {
                    Status = "success",
                    Data = request.Id,
                    Message = "Document added successfully"
                });
            }

            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Failed to add document to vector database"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding document {DocumentId}", request.Id);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while adding document"
            });
        }
    }

    /// <summary>
    /// Add multiple documents to the vector database in batch
    /// </summary>
    /// <param name="request">Batch of documents to add</param>
    /// <returns>Success status with count</returns>
    [HttpPost("documents/batch")]
    [ProducesResponseType(typeof(VectorDBResponse<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDocumentsBatch([FromBody] BatchDocumentsRequest request)
    {
        if (!ModelState.IsValid || request.Documents == null || !request.Documents.Any())
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Valid documents array is required"
            });
        }

        try
        {
            var success = await _chromaClient.AddDocumentsBatchAsync(request);
            
            if (success)
            {
                return Ok(new VectorDBResponse<int>
                {
                    Status = "success",
                    Data = request.Documents.Count,
                    Message = $"Successfully added {request.Documents.Count} documents"
                });
            }

            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Failed to add documents to vector database"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding batch documents");
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while adding documents"
            });
        }
    }

    /// <summary>
    /// Get a specific document by ID
    /// </summary>
    /// <param name="documentId">Document ID</param>
    /// <param name="collectionName">Collection name (optional)</param>
    /// <returns>Document data</returns>
    [HttpGet("documents/{documentId}")]
    [ProducesResponseType(typeof(VectorDBResponse<DocumentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocument(
        [FromRoute] string documentId, 
        [FromQuery] string collectionName = "oceanographic_data")
    {
        if (string.IsNullOrWhiteSpace(documentId))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Document ID is required"
            });
        }

        try
        {
            var document = await _chromaClient.GetDocumentAsync(documentId, collectionName);
            
            if (document != null)
            {
                return Ok(new VectorDBResponse<DocumentResponse>
                {
                    Status = "success",
                    Data = document,
                    Message = "Document retrieved successfully"
                });
            }

            return NotFound(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Document not found"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving document {DocumentId}", documentId);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while retrieving document"
            });
        }
    }

    /// <summary>
    /// Update a document's content or metadata
    /// </summary>
    /// <param name="documentId">Document ID</param>
    /// <param name="request">Update data</param>
    /// <param name="collectionName">Collection name (optional)</param>
    /// <returns>Success status</returns>
    [HttpPut("documents/{documentId}")]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDocument(
        [FromRoute] string documentId,
        [FromBody] UpdateDocumentRequest request,
        [FromQuery] string collectionName = "oceanographic_data")
    {
        if (string.IsNullOrWhiteSpace(documentId))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Document ID is required"
            });
        }

        if (string.IsNullOrWhiteSpace(request.Text) && (request.Metadata == null || !request.Metadata.Any()))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Either text or metadata must be provided for update"
            });
        }

        try
        {
            var success = await _chromaClient.UpdateDocumentAsync(documentId, request, collectionName);
            
            if (success)
            {
                return Ok(new VectorDBResponse<string>
                {
                    Status = "success",
                    Data = documentId,
                    Message = "Document updated successfully"
                });
            }

            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Failed to update document"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating document {DocumentId}", documentId);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while updating document"
            });
        }
    }

    /// <summary>
    /// Delete a specific document
    /// </summary>
    /// <param name="documentId">Document ID</param>
    /// <param name="collectionName">Collection name (optional)</param>
    /// <returns>Success status</returns>
    [HttpDelete("documents/{documentId}")]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteDocument(
        [FromRoute] string documentId,
        [FromQuery] string collectionName = "oceanographic_data")
    {
        if (string.IsNullOrWhiteSpace(documentId))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Document ID is required"
            });
        }

        try
        {
            var success = await _chromaClient.DeleteDocumentAsync(documentId, collectionName);
            
            if (success)
            {
                return Ok(new VectorDBResponse<string>
                {
                    Status = "success",
                    Data = documentId,
                    Message = "Document deleted successfully"
                });
            }

            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Failed to delete document"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting document {DocumentId}", documentId);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while deleting document"
            });
        }
    }

    #endregion

    #region Collection Management

    /// <summary>
    /// Create a new collection
    /// </summary>
    /// <param name="collectionInfo">Collection information</param>
    /// <returns>Success status</returns>
    [HttpPost("collections")]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCollection([FromBody] CollectionInfo collectionInfo)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(collectionInfo.Name))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Collection name is required"
            });
        }

        try
        {
            var success = await _chromaClient.CreateCollectionAsync(collectionInfo);
            
            if (success)
            {
                return StatusCode(StatusCodes.Status201Created, new VectorDBResponse<string>
                {
                    Status = "success",
                    Data = collectionInfo.Name,
                    Message = "Collection created successfully"
                });
            }

            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Failed to create collection"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating collection {CollectionName}", collectionInfo.Name);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while creating collection"
            });
        }
    }

    /// <summary>
    /// List all collections
    /// </summary>
    /// <returns>List of collections</returns>
    [HttpGet("collections")]
    [ProducesResponseType(typeof(VectorDBResponse<List<CollectionResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListCollections()
    {
        try
        {
            var collections = await _chromaClient.ListCollectionsAsync();
            
            return Ok(new VectorDBResponse<List<CollectionResponse>>
            {
                Status = "success",
                Data = collections ?? new List<CollectionResponse>(),
                Message = $"Found {collections?.Count ?? 0} collections"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing collections");
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while listing collections"
            });
        }
    }

    /// <summary>
    /// Get information about a specific collection
    /// </summary>
    /// <param name="collectionName">Collection name</param>
    /// <returns>Collection information</returns>
    [HttpGet("collections/{collectionName}")]
    [ProducesResponseType(typeof(VectorDBResponse<CollectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCollectionInfo([FromRoute] string collectionName)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Collection name is required"
            });
        }

        try
        {
            var collection = await _chromaClient.GetCollectionInfoAsync(collectionName);
            
            if (collection != null)
            {
                return Ok(new VectorDBResponse<CollectionResponse>
                {
                    Status = "success",
                    Data = collection,
                    Message = "Collection information retrieved successfully"
                });
            }

            return NotFound(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Collection not found"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting collection info {CollectionName}", collectionName);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while getting collection information"
            });
        }
    }

    /// <summary>
    /// Delete a collection
    /// </summary>
    /// <param name="collectionName">Collection name</param>
    /// <returns>Success status</returns>
    [HttpDelete("collections/{collectionName}")]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCollection([FromRoute] string collectionName)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Collection name is required"
            });
        }

        try
        {
            var success = await _chromaClient.DeleteCollectionAsync(collectionName);
            
            if (success)
            {
                return Ok(new VectorDBResponse<string>
                {
                    Status = "success",
                    Data = collectionName,
                    Message = "Collection deleted successfully"
                });
            }

            return NotFound(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Collection not found or failed to delete"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting collection {CollectionName}", collectionName);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while deleting collection"
            });
        }
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Perform basic similarity search query
    /// </summary>
    /// <param name="request">Query request</param>
    /// <returns>Query results</returns>
    [HttpPost("query")]
    [ProducesResponseType(typeof(VectorDBResponse<QueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Query([FromBody] QueryRequest request)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Query text is required"
            });
        }

        try
        {
            var result = await _chromaClient.QueryAsync(request);
            
            if (result != null)
            {
                return Ok(new VectorDBResponse<QueryResponse>
                {
                    Status = "success",
                    Data = result,
                    Message = $"Query completed successfully. Found {result.Count} results."
                });
            }

            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Query failed"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing query: {Query}", request.Text);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while executing query"
            });
        }
    }

    /// <summary>
    /// Perform semantic search with similarity filtering
    /// </summary>
    /// <param name="request">Semantic query request</param>
    /// <returns>Filtered query results</returns>
    [HttpPost("query/semantic")]
    [ProducesResponseType(typeof(VectorDBResponse<QueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SemanticQuery([FromBody] SemanticQueryRequest request)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Query text is required"
            });
        }

        try
        {
            var result = await _chromaClient.SemanticQueryAsync(request);
            
            if (result != null)
            {
                return Ok(new VectorDBResponse<QueryResponse>
                {
                    Status = "success",
                    Data = result,
                    Message = $"Semantic query completed. Found {result.Count} results above {request.SimilarityThreshold:F2} similarity threshold."
                });
            }

            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Semantic query failed"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing semantic query: {Query}", request.Text);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while executing semantic query"
            });
        }
    }

    /// <summary>
    /// Perform filtered query with metadata constraints
    /// </summary>
    /// <param name="request">Filtered query request</param>
    /// <returns>Filtered query results</returns>
    [HttpPost("query/filtered")]
    [ProducesResponseType(typeof(VectorDBResponse<QueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FilteredQuery([FromBody] QueryRequest request)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Query text is required"
            });
        }

        try
        {
            var result = await _chromaClient.FilteredQueryAsync(request);
            
            if (result != null)
            {
                return Ok(new VectorDBResponse<QueryResponse>
                {
                    Status = "success",
                    Data = result,
                    Message = $"Filtered query completed. Found {result.Count} results."
                });
            }

            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Filtered query failed"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing filtered query: {Query}", request.Text);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while executing filtered query"
            });
        }
    }

    /// <summary>
    /// Find documents similar to a specific document
    /// </summary>
    /// <param name="documentId">Reference document ID</param>
    /// <param name="collectionName">Collection name (optional)</param>
    /// <param name="nResults">Number of results to return</param>
    /// <returns>Similar documents</returns>
    [HttpGet("documents/{documentId}/similar")]
    [ProducesResponseType(typeof(VectorDBResponse<SimilarDocumentsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FindSimilarDocuments(
        [FromRoute] string documentId,
        [FromQuery] string collectionName = "oceanographic_data",
        [FromQuery] [Range(1, 50)] int nResults = 5)
    {
        if (string.IsNullOrWhiteSpace(documentId))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Document ID is required"
            });
        }

        try
        {
            var result = await _chromaClient.FindSimilarDocumentsAsync(documentId, collectionName, nResults);
            
            if (result != null)
            {
                return Ok(new VectorDBResponse<SimilarDocumentsResponse>
                {
                    Status = "success",
                    Data = result,
                    Message = $"Found {result.Count} similar documents"
                });
            }

            return NotFound(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Document not found or no similar documents found"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding similar documents for {DocumentId}", documentId);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while finding similar documents"
            });
        }
    }

    #endregion

    #region RAG-Specific Endpoints

    /// <summary>
    /// Get relevant data for RAG context - Main endpoint for the RAG system
    /// </summary>
    /// <param name="query">User query</param>
    /// <param name="maxResults">Maximum number of results</param>
    /// <param name="similarityThreshold">Minimum similarity threshold</param>
    /// <returns>RAG context with relevant documents</returns>
    [HttpGet("rag/context")]
    [ProducesResponseType(typeof(VectorDBResponse<RAGContext>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRAGContext(
        [FromQuery] [Required] string query,
        [FromQuery] [Range(1, 20)] int maxResults = 5,
        [FromQuery] [Range(0.0, 1.0)] double similarityThreshold = 0.7)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Query is required"
            });
        }

        try
        {
            var ragContext = await _chromaClient.GetRelevantDataAsync(query, maxResults, similarityThreshold);
            
            return Ok(new VectorDBResponse<RAGContext>
            {
                Status = "success",
                Data = ragContext,
                Message = $"Retrieved {ragContext.RelevantDocuments.Count} relevant documents for RAG context"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting RAG context for query: {Query}", query);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while getting RAG context"
            });
        }
    }

    /// <summary>
    /// Get relevant oceanographic data with specific filters for RAG context
    /// </summary>
    /// <param name="query">User query</param>
    /// <param name="dataType">Filter by data type (e.g., sensor_data, location_info)</param>
    /// <param name="location">Filter by location</param>
    /// <param name="instrumentType">Filter by instrument type</param>
    /// <param name="maxResults">Maximum number of results</param>
    /// <param name="similarityThreshold">Minimum similarity threshold</param>
    /// <returns>Filtered RAG context</returns>
    [HttpGet("rag/oceanographic")]
    [ProducesResponseType(typeof(VectorDBResponse<RAGContext>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOceanographicRAGContext(
        [FromQuery] [Required] string query,
        [FromQuery] string? dataType = null,
        [FromQuery] string? location = null,
        [FromQuery] string? instrumentType = null,
        [FromQuery] [Range(1, 20)] int maxResults = 5,
        [FromQuery] [Range(0.0, 1.0)] double similarityThreshold = 0.7)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Query is required"
            });
        }

        try
        {
            var ragContext = await _chromaClient.GetRelevantOceanographicDataAsync(
                query, dataType, location, instrumentType, maxResults, similarityThreshold);
            
            return Ok(new VectorDBResponse<RAGContext>
            {
                Status = "success",
                Data = ragContext,
                Message = $"Retrieved {ragContext.RelevantDocuments.Count} relevant oceanographic documents"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting oceanographic RAG context for query: {Query}", query);
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while getting oceanographic RAG context"
            });
        }
    }

    #endregion

    #region Analytics and Management

    /// <summary>
    /// Check the health of the vector database
    /// </summary>
    /// <returns>Health status</returns>
    [HttpGet("health")]
    [ProducesResponseType(typeof(VectorDBResponse<HealthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetHealth()
    {
        try
        {
            var health = await _chromaClient.GetHealthAsync();
            
            if (health != null && health.Status == "healthy")
            {
                return Ok(new VectorDBResponse<HealthResponse>
                {
                    Status = "success",
                    Data = health,
                    Message = "Vector database is healthy"
                });
            }

            return StatusCode(StatusCodes.Status503ServiceUnavailable, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Vector database is unhealthy"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking vector database health");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Health check failed"
            });
        }
    }

    /// <summary>
    /// Get comprehensive statistics about the vector database
    /// </summary>
    /// <returns>Database statistics</returns>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(VectorDBResponse<StatsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStats()
    {
        try
        {
            var stats = await _chromaClient.GetStatsAsync();
            
            return Ok(new VectorDBResponse<StatsResponse>
            {
                Status = "success",
                Data = stats ?? new StatsResponse(),
                Message = "Statistics retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vector database statistics");
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while getting statistics"
            });
        }
    }

    /// <summary>
    /// Generate embeddings for texts without storing them
    /// </summary>
    /// <param name="texts">List of texts to generate embeddings for</param>
    /// <returns>Generated embeddings</returns>
    [HttpPost("embeddings")]
    [ProducesResponseType(typeof(VectorDBResponse<EmbeddingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VectorDBResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateEmbeddings([FromBody] List<string> texts)
    {
        if (texts == null || !texts.Any() || texts.Any(string.IsNullOrWhiteSpace))
        {
            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Valid texts array is required"
            });
        }

        try
        {
            var embeddings = await _chromaClient.GenerateEmbeddingsAsync(texts);
            
            if (embeddings != null)
            {
                return Ok(new VectorDBResponse<EmbeddingResponse>
                {
                    Status = "success",
                    Data = embeddings,
                    Message = $"Generated embeddings for {texts.Count} texts"
                });
            }

            return BadRequest(new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Failed to generate embeddings"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating embeddings");
            return StatusCode(StatusCodes.Status500InternalServerError, new VectorDBResponse<string>
            {
                Status = "error",
                Error = "Internal server error occurred while generating embeddings"
            });
        }
    }

    #endregion

    #region Legacy Endpoints (Backward Compatibility)

    /// <summary>
    /// Legacy add endpoint for backward compatibility
    /// </summary>
    [HttpPost("add")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [Obsolete("Use POST /api/vectordb/documents instead")]
    public async Task<IActionResult> AddLegacy([FromBody] AddRequest request)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Text) || string.IsNullOrWhiteSpace(request.Id))
            return BadRequest("Fields must be filled.");

        var response = await _chromaClient.AddDocumentAsync(request);
        return response ? Ok("Document added successfully.") : BadRequest("Failed to add document.");
    }

    /// <summary>
    /// Legacy query endpoint for backward compatibility
    /// </summary>
    [HttpPost("query")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [Obsolete("Use POST /api/vectordb/query instead")]
    public async Task<IActionResult> QueryLegacy([FromBody] QueryRequest request)
    {
        var result = await _chromaClient.QueryLegacyAsync(request);
        return Ok(result);
    }

    #endregion
}