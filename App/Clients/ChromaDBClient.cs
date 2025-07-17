using System.Globalization;
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
            // Normalize file extension from .txt to .md format
            if (request.Metadata != null)
            {
                // Check for common filename fields and normalize them
                var filenameFields = new[] { "filename", "file_name", "fileName", "name" };
                foreach (var field in filenameFields)
                {
                    if (request.Metadata.ContainsKey(field))
                    {
                        var fileName = request.Metadata[field]?.ToString();
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            request.Metadata[field] = NormalizeFileExtension(fileName);
                        }
                    }
                }
            }

            // Preprocess metadata to handle any remaining lists that might be in the Dictionary<string, object>
            if (request.Metadata != null)
            {
                foreach (var key in request.Metadata.Keys.ToList())
                {
                    if (request.Metadata[key] is List<string> list)
                    {
                        request.Metadata[key] = string.Join(",", list);
                    }
                    else if (request.Metadata[key] is List<object> objectList)
                    {
                        request.Metadata[key] = string.Join(",", objectList.Select(o => o.ToString()));
                    }
                }
            }

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
            // Normalize file extensions from .txt to .md format for all documents in the batch
            foreach (var document in request.Documents)
            {
                if (document.Metadata != null)
                {
                    // Check for common filename fields and normalize them
                    var filenameFields = new[] { "filename", "file_name", "fileName", "name" };
                    foreach (var field in filenameFields)
                    {
                        if (document.Metadata.ContainsKey(field))
                        {
                            var fileName = document.Metadata[field]?.ToString();
                            if (!string.IsNullOrWhiteSpace(fileName))
                            {
                                document.Metadata[field] = NormalizeFileExtension(fileName);
                            }
                        }
                    }
                }
            }

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

    /// <summary>
    /// Normalize file extensions to ensure all files are stored as .md format
    /// </summary>
    private static string NormalizeFileExtension(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return fileName;

        // Check if the file has a .txt extension and convert it to .md
        if (fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
        {
            return fileName.Substring(0, fileName.Length - 4) + ".md";
        }

        // If no extension or already .md, return as is
        return fileName;
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
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Query cannot be null or empty", nameof(query));
        }

        if (maxResults <= 0)
        {
            throw new ArgumentException("Max results must be greater than 0", nameof(maxResults));
        }

        if (similarityThreshold < 0.0 || similarityThreshold > 1.0)
        {
            throw new ArgumentException("Similarity threshold must be between 0.0 and 1.0", nameof(similarityThreshold));
        }

        var ragContext = new RAGContext
        {
            Query = query,
            Timestamp = DateTime.UtcNow
        };

        try
        {
            _logger.LogDebug("Performing semantic search for query: {Query} with maxResults: {MaxResults}, threshold: {Threshold}",
                query, maxResults, similarityThreshold);

            // Perform semantic search to get relevant documents
            // Request more results than needed to allow for proper filtering by similarity threshold
            var semanticRequest = new SemanticQueryRequest
            {
                Text = query,
                NResults = Math.Max(maxResults * 2, 10), // Request more to account for filtering
                SimilarityThreshold = 0.0, // Let ChromaDB return all results, we'll filter by similarity ourselves
                CollectionName = "oceanographic_data"
            };

            var queryResponse = await SemanticQueryAsync(semanticRequest);

            if (queryResponse?.Results?.Documents == null || queryResponse?.Results?.Ids == null || queryResponse.Results.Documents.Count == 0)
            {
                _logger.LogWarning("No documents returned from semantic query for: {Query}", query);
                return ragContext;
            }

            var documents = queryResponse.Results.Documents[0];
            var ids = queryResponse.Results.Ids[0];
            var metadatas = queryResponse.Results.Metadatas.Count > 0 ? queryResponse.Results.Metadatas[0] : new List<Dictionary<string, object>?>();
            var distances = queryResponse.Results.Distances.Count > 0 ? queryResponse.Results.Distances[0] : new List<double>();

            if (documents.Count == 0)
            {
                _logger.LogWarning("Empty document list returned for query: {Query}", query);
                return ragContext;
            }

            _logger.LogDebug("Processing {DocumentCount} documents from ChromaDB response", documents.Count);

            var candidateDocuments = new List<(RelevantDocument doc, double similarity)>();

            for (int i = 0; i < documents.Count && i < ids.Count && i < distances.Count; i++)
            {
                var distance = distances[i];

                // ChromaDB returns cosine distance where 0 = identical, 2 = completely opposite
                // Convert to similarity score where 1 = identical, 0 = completely different
                var similarity = Math.Max(0.0, 1.0 - (distance / 2.0));

                // Only include documents that meet the similarity threshold
                if (similarity < similarityThreshold)
                {
                    _logger.LogDebug("Skipping document {Index} with similarity {Similarity} below threshold {Threshold}",
                        i, similarity, similarityThreshold);
                    continue;
                }

                var metadata = metadatas.Count > i ? metadatas[i] : null;
                var documentId = ids[i];

                var relevantDoc = new RelevantDocument
                {
                    Id = documentId,
                    Content = documents[i] ?? string.Empty,
                    Relevance = similarity,
                    Source = ExtractMetadataValue(metadata, "source", "unknown")
                };

                // Parse metadata if available
                if (metadata != null)
                {
                    relevantDoc.Metadata = metadata;
                }

                candidateDocuments.Add((relevantDoc, similarity));

                _logger.LogDebug("Added document {Id} with similarity {Similarity} (distance: {Distance})",
                    documentId, similarity, distance);
            }

            // Sort by similarity (highest first) and take only the requested number
            var topDocuments = candidateDocuments
                .OrderByDescending(x => x.similarity)
                .Take(maxResults)
                .Select(x => x.doc)
                .ToList();

            ragContext.RelevantDocuments.AddRange(topDocuments);

            var avgSimilarity = topDocuments.Count > 0 ? topDocuments.Average(d => d.Relevance) : 0.0;

            _logger.LogInformation("Retrieved {Count} relevant documents for RAG context with average similarity {AvgSimilarity:F3} (threshold: {Threshold})",
                ragContext.RelevantDocuments.Count, avgSimilarity, similarityThreshold);

            if (ragContext.RelevantDocuments.Count == 0)
            {
                _logger.LogWarning("No documents met the similarity threshold {Threshold} for query: {Query}",
                    similarityThreshold, query);
            }
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HTTP error occurred while getting relevant data for query: {Query}", query);
            throw new InvalidOperationException("Failed to communicate with ChromaDB service", httpEx);
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "JSON parsing error occurred while processing ChromaDB response for query: {Query}", query);
            throw new InvalidOperationException("Failed to parse ChromaDB response", jsonEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while getting relevant data for query: {Query}", query);
            throw;
        }

        return ragContext;
    }

    private static string ExtractMetadataValue(Dictionary<string, object>? metadata, string key, string defaultValue = "")
    {
        return metadata?.TryGetValue(key, out var valueObj) == true ? valueObj?.ToString() ?? defaultValue : defaultValue;
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
                    relevantDoc.Metadata = metadata;
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
