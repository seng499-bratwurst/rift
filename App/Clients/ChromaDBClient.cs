using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Rift.App.Models;

namespace Rift.App.Clients;

public class ChromaDBClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly ILogger<ChromaDBClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ChromaDBClient(HttpClient httpClient, ILogger<ChromaDBClient> logger, string baseUrl = "http://chromadb:8000")
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl.TrimEnd('/');
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    #region Document Management

    /// <summary>
    /// Add a single document to the vector database
    /// </summary>
    public async Task<bool> AddDocumentAsync(AddRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/documents/add", request, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully added document {DocumentId} to collection {Collection}", 
                    request.Id, request.CollectionName);
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to add document {DocumentId}: {Error}", request.Id, errorContent);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while adding document {DocumentId}", request.Id);
            return false;
        }
    }

    /// <summary>
    /// Add multiple documents to the vector database in batch
    /// </summary>
    public async Task<bool> AddDocumentsBatchAsync(BatchDocumentsRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/documents/batch", request, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully added {Count} documents to collection {Collection}", 
                    request.Documents.Count, request.CollectionName);
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to add batch documents: {Error}", errorContent);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while adding batch documents");
            return false;
        }
    }

    /// <summary>
    /// Get a specific document by ID
    /// </summary>
    public async Task<DocumentResponse?> GetDocumentAsync(string documentId, string collectionName = "oceanographic_data")
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/documents/{documentId}?collection_name={collectionName}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DocumentResponse>(content, _jsonOptions);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Document {DocumentId} not found in collection {Collection}", documentId, collectionName);
                return null;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to get document {DocumentId}: {Error}", documentId, errorContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while getting document {DocumentId}", documentId);
            return null;
        }
    }

    /// <summary>
    /// Update a document's content or metadata
    /// </summary>
    public async Task<bool> UpdateDocumentAsync(string documentId, UpdateDocumentRequest request, string collectionName = "oceanographic_data")
    {
        try
        {
            var url = $"{_baseUrl}/documents/{documentId}?collection_name={collectionName}";
            var response = await _httpClient.PutAsJsonAsync(url, request, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully updated document {DocumentId}", documentId);
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to update document {DocumentId}: {Error}", documentId, errorContent);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while updating document {DocumentId}", documentId);
            return false;
        }
    }

    /// <summary>
    /// Delete a specific document
    /// </summary>
    public async Task<bool> DeleteDocumentAsync(string documentId, string collectionName = "oceanographic_data")
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/documents/{documentId}?collection_name={collectionName}");
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully deleted document {DocumentId}", documentId);
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to delete document {DocumentId}: {Error}", documentId, errorContent);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while deleting document {DocumentId}", documentId);
            return false;
        }
    }

    #endregion

    #region Collection Management

    /// <summary>
    /// Create a new collection
    /// </summary>
    public async Task<bool> CreateCollectionAsync(CollectionInfo collectionInfo)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/collections", collectionInfo, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully created collection {CollectionName}", collectionInfo.Name);
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to create collection {CollectionName}: {Error}", collectionInfo.Name, errorContent);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while creating collection {CollectionName}", collectionInfo.Name);
            return false;
        }
    }

    /// <summary>
    /// List all collections
    /// </summary>
    public async Task<List<CollectionResponse>?> ListCollectionsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/collections");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Dictionary<string, List<CollectionResponse>>>(content, _jsonOptions);
                return result?["collections"];
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to list collections: {Error}", errorContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while listing collections");
            return null;
        }
    }

    /// <summary>
    /// Get information about a specific collection
    /// </summary>
    public async Task<CollectionResponse?> GetCollectionInfoAsync(string collectionName)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/collections/{collectionName}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CollectionResponse>(content, _jsonOptions);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Collection {CollectionName} not found", collectionName);
                return null;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to get collection {CollectionName}: {Error}", collectionName, errorContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while getting collection {CollectionName}", collectionName);
            return null;
        }
    }

    /// <summary>
    /// Delete a collection
    /// </summary>
    public async Task<bool> DeleteCollectionAsync(string collectionName)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/collections/{collectionName}");
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully deleted collection {CollectionName}", collectionName);
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to delete collection {CollectionName}: {Error}", collectionName, errorContent);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while deleting collection {CollectionName}", collectionName);
            return false;
        }
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Perform basic similarity search
    /// </summary>
    public async Task<QueryResponse?> QueryAsync(QueryRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/query", request, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<QueryResponse>(content, _jsonOptions);
                
                _logger.LogInformation("Query completed successfully. Found {Count} results for query: {Query}", 
                    result?.Count ?? 0, request.Text);
                
                return result;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Query failed: {Error}", errorContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred during query: {Query}", request.Text);
            return null;
        }
    }

    /// <summary>
    /// Perform semantic search with similarity filtering
    /// </summary>
    public async Task<QueryResponse?> SemanticQueryAsync(SemanticQueryRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/query/semantic", request, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<QueryResponse>(content, _jsonOptions);
                
                _logger.LogInformation("Semantic query completed. Found {Count} results above threshold {Threshold} for query: {Query}", 
                    result?.Count ?? 0, request.SimilarityThreshold, request.Text);
                
                return result;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Semantic query failed: {Error}", errorContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred during semantic query: {Query}", request.Text);
            return null;
        }
    }

    /// <summary>
    /// Perform filtered query with metadata constraints
    /// </summary>
    public async Task<QueryResponse?> FilteredQueryAsync(QueryRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/query/filtered", request, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<QueryResponse>(content, _jsonOptions);
                
                _logger.LogInformation("Filtered query completed. Found {Count} results for query: {Query}", 
                    result?.Count ?? 0, request.Text);
                
                return result;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Filtered query failed: {Error}", errorContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred during filtered query: {Query}", request.Text);
            return null;
        }
    }

    /// <summary>
    /// Find documents similar to a specific document
    /// </summary>
    public async Task<SimilarDocumentsResponse?> FindSimilarDocumentsAsync(string documentId, string collectionName = "oceanographic_data", int nResults = 5)
    {
        try
        {
            var url = $"{_baseUrl}/similar/{documentId}?collection_name={collectionName}&n_results={nResults}";
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<SimilarDocumentsResponse>(content, _jsonOptions);
                
                _logger.LogInformation("Found {Count} similar documents for document {DocumentId}", 
                    result?.Count ?? 0, documentId);
                
                return result;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Document {DocumentId} not found for similarity search", documentId);
                return null;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Similar documents search failed: {Error}", errorContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred during similar documents search for {DocumentId}", documentId);
            return null;
        }
    }

    #endregion

    #region RAG-Specific Methods

    /// <summary>
    /// Get relevant data for RAG context - main method for the RAG system
    /// </summary>
    public async Task<RAGContext> GetRelevantDataAsync(string query, int maxResults = 5, double similarityThreshold = 0.7)
    {
        var ragContext = new RAGContext
        {
            Query = query,
            Timestamp = DateTime.UtcNow
        };

        try
        {
            // Perform semantic search to get relevant documents
            var semanticRequest = new SemanticQueryRequest
            {
                Text = query,
                NResults = maxResults,
                SimilarityThreshold = similarityThreshold,
                CollectionName = "oceanographic_data"
            };

            var queryResponse = await SemanticQueryAsync(semanticRequest);
            
            if (queryResponse?.Results?.Documents != null && queryResponse.Results.Documents.Count > 0)
            {
                var documents = queryResponse.Results.Documents[0];
                var metadatas = queryResponse.Results.Metadatas.Count > 0 ? queryResponse.Results.Metadatas[0] : new List<Dictionary<string, object>?>();
                var distances = queryResponse.Results.Distances.Count > 0 ? queryResponse.Results.Distances[0] : new List<double>();

                for (int i = 0; i < documents.Count; i++)
                {
                    var relevance = distances.Count > i ? 1.0 - distances[i] : 0.0; // Convert distance to similarity
                    var metadata = metadatas.Count > i ? metadatas[i] : null;

                    var relevantDoc = new RelevantDocument
                    {
                        Id = $"doc_{i}",
                        Content = documents[i],
                        Relevance = relevance,
                        Source = metadata?.TryGetValue("source", out var sourceObj) == true ? sourceObj?.ToString() ?? "unknown" : "unknown"
                    };

                    // Parse metadata if available
                    if (metadata != null)
                    {
                        relevantDoc.Metadata = new DocumentMetadata
                        {
                            Source = metadata.TryGetValue("source", out var srcObj) ? srcObj?.ToString() ?? "" : "",
                            DataType = metadata.TryGetValue("data_type", out var dtObj) ? dtObj?.ToString() ?? "" : "",
                            Timestamp = metadata.TryGetValue("timestamp", out var tsObj) ? tsObj?.ToString() : null,
                            Location = metadata.TryGetValue("location", out var locObj) ? locObj?.ToString() : null,
                            InstrumentType = metadata.TryGetValue("instrument_type", out var instObj) ? instObj?.ToString() : null
                        };

                        if (metadata.TryGetValue("depth", out var depthObj) && double.TryParse(depthObj?.ToString(), out var depth))
                        {
                            relevantDoc.Metadata.Depth = depth;
                        }

                        if (metadata.TryGetValue("tags", out var tagsObj) && tagsObj is JsonElement tagsElement && tagsElement.ValueKind == JsonValueKind.Array)
                        {
                            relevantDoc.Metadata.Tags = tagsElement.EnumerateArray()
                                .Where(tag => tag.ValueKind == JsonValueKind.String)
                                .Select(tag => tag.GetString()!)
                                .ToList();
                        }
                    }

                    ragContext.RelevantDocuments.Add(relevantDoc);
                }

                _logger.LogInformation("Retrieved {Count} relevant documents for RAG context", ragContext.RelevantDocuments.Count);
            }
            else
            {
                _logger.LogWarning("No relevant documents found for query: {Query}", query);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get relevant data for RAG context: {Query}", query);
        }

        return ragContext;
    }

    /// <summary>
    /// Get relevant data with specific filters for oceanographic data
    /// </summary>
    public async Task<RAGContext> GetRelevantOceanographicDataAsync(
        string query, 
        string? dataType = null, 
        string? location = null, 
        string? instrumentType = null,
        int maxResults = 5,
        double similarityThreshold = 0.7)
    {
        var filters = new Dictionary<string, object>();
        
        if (!string.IsNullOrEmpty(dataType))
            filters["data_type"] = dataType;
        if (!string.IsNullOrEmpty(location))
            filters["location"] = location;
        if (!string.IsNullOrEmpty(instrumentType))
            filters["instrument_type"] = instrumentType;

        var semanticRequest = new SemanticQueryRequest
        {
            Text = query,
            NResults = maxResults,
            SimilarityThreshold = similarityThreshold,
            CollectionName = "oceanographic_data",
            Where = filters.Count > 0 ? filters : null
        };

        var queryResponse = await SemanticQueryAsync(semanticRequest);
        
        var ragContext = new RAGContext
        {
            Query = query,
            Timestamp = DateTime.UtcNow
        };

        if (queryResponse?.Results?.Documents != null && queryResponse.Results.Documents.Count > 0)
        {
            var documents = queryResponse.Results.Documents[0];
            var metadatas = queryResponse.Results.Metadatas.Count > 0 ? queryResponse.Results.Metadatas[0] : new List<Dictionary<string, object>?>();
            var distances = queryResponse.Results.Distances.Count > 0 ? queryResponse.Results.Distances[0] : new List<double>();

            for (int i = 0; i < documents.Count; i++)
            {
                var relevance = distances.Count > i ? 1.0 - distances[i] : 0.0;
                var metadata = metadatas.Count > i ? metadatas[i] : null;

                var relevantDoc = new RelevantDocument
                {
                    Id = $"filtered_doc_{i}",
                    Content = documents[i],
                    Relevance = relevance,
                    Source = metadata?.TryGetValue("source", out var sourceObj) == true ? sourceObj?.ToString() ?? "oceanographic_database" : "oceanographic_database"
                };

                if (metadata != null)
                {
                    relevantDoc.Metadata = new DocumentMetadata
                    {
                        Source = metadata.TryGetValue("source", out var srcObj) ? srcObj?.ToString() ?? "" : "",
                        DataType = metadata.TryGetValue("data_type", out var dtObj) ? dtObj?.ToString() ?? "" : "",
                        Timestamp = metadata.TryGetValue("timestamp", out var tsObj) ? tsObj?.ToString() : null,
                        Location = metadata.TryGetValue("location", out var locObj) ? locObj?.ToString() : null,
                        InstrumentType = metadata.TryGetValue("instrument_type", out var instObj) ? instObj?.ToString() : null
                    };

                    if (metadata.TryGetValue("depth", out var depthObj) && double.TryParse(depthObj?.ToString(), out var depth))
                    {
                        relevantDoc.Metadata.Depth = depth;
                    }
                }

                ragContext.RelevantDocuments.Add(relevantDoc);
            }

            _logger.LogInformation("Retrieved {Count} filtered relevant documents for RAG context", ragContext.RelevantDocuments.Count);
        }

        return ragContext;
    }

    #endregion

    #region Analytics and Management

    /// <summary>
    /// Check the health of the vector database
    /// </summary>
    public async Task<HealthResponse?> GetHealthAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/health");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<HealthResponse>(content, _jsonOptions);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred during health check");
            return null;
        }
    }

    /// <summary>
    /// Get comprehensive statistics about the vector database
    /// </summary>
    public async Task<StatsResponse?> GetStatsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/stats");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<StatsResponse>(content, _jsonOptions);
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to get stats: {Error}", errorContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while getting stats");
            return null;
        }
    }

    /// <summary>
    /// Generate embeddings for texts without storing them
    /// </summary>
    public async Task<EmbeddingResponse?> GenerateEmbeddingsAsync(List<string> texts)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/embeddings", texts, _jsonOptions);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<EmbeddingResponse>(content, _jsonOptions);
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to generate embeddings: {Error}", errorContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while generating embeddings");
            return null;
        }
    }

    #endregion

    #region Legacy Support (Backward Compatibility)

    /// <summary>
    /// Legacy add method for backward compatibility
    /// </summary>
    [Obsolete("Use AddDocumentAsync instead")]
    public async Task<bool> AddAsync(AddRequest request)
    {
        return await AddDocumentAsync(request);
    }

    /// <summary>
    /// Legacy query method for backward compatibility
    /// </summary>
    [Obsolete("Use QueryAsync instead")]
    public async Task<string?> QueryLegacyAsync(QueryRequest request)
    {
        var response = await QueryAsync(request);
        if (response != null)
        {
            return JsonSerializer.Serialize(response, _jsonOptions);
        }
        return null;
    }

    #endregion
}