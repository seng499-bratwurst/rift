using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rift.App.Models;

// Single document model
public class Document
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

// Request models
public class AddRequest
{
    [Required]
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    [JsonPropertyName("collection_name")]
    public string CollectionName { get; set; } = "oceanographic_data";
}

public class BatchDocumentsRequest
{
    [Required]
    [JsonPropertyName("documents")]
    public List<Document> Documents { get; set; } = new();

    [JsonPropertyName("collection_name")]
    public string CollectionName { get; set; } = "oceanographic_data";
}

public class QueryRequest
{
    [Required]
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [Range(1, 50)]
    [JsonPropertyName("n_results")]
    public int NResults { get; set; } = 5;

    [JsonPropertyName("collection_name")]
    public string CollectionName { get; set; } = "oceanographic_data";

    [JsonPropertyName("where")]
    public Dictionary<string, object>? Where { get; set; }

    [JsonPropertyName("include")]
    public List<string> Include { get; set; } = new() { "documents", "metadatas", "distances" };
}

public class SemanticQueryRequest
{
    [Required]
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [Range(1, 50)]
    [JsonPropertyName("n_results")]
    public int NResults { get; set; } = 5;

    [JsonPropertyName("collection_name")]
    public string CollectionName { get; set; } = "oceanographic_data";

    [Range(0.0, 1.0)]
    [JsonPropertyName("similarity_threshold")]
    public double SimilarityThreshold { get; set; } = 0.0;

    [JsonPropertyName("where")]
    public Dictionary<string, object>? Where { get; set; }
}

public class HybridQueryRequest
{
    [Required]
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("keywords")]
    public List<string>? Keywords { get; set; }

    [Range(1, 50)]
    [JsonPropertyName("n_results")]
    public int NResults { get; set; } = 5;

    [JsonPropertyName("collection_name")]
    public string CollectionName { get; set; } = "oceanographic_data";

    [Range(0.0, 1.0)]
    [JsonPropertyName("semantic_weight")]
    public double SemanticWeight { get; set; } = 0.7;

    [JsonPropertyName("where")]
    public Dictionary<string, object>? Where { get; set; }
}

public class CollectionInfo
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

public class UpdateDocumentRequest
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

// Response models
public class VectorDBResponse<T>
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

public class QueryResult
{
    [JsonPropertyName("documents")]
    public List<List<string>> Documents { get; set; } = new();

    [JsonPropertyName("metadatas")]
    public List<List<Dictionary<string, object>?>> Metadatas { get; set; } = new();

    [JsonPropertyName("distances")]
    public List<List<double>> Distances { get; set; } = new();

    [JsonPropertyName("ids")]
    public List<List<string>>? Ids { get; set; }
}

public class QueryResponse
{
    [JsonPropertyName("query")]
    public string Query { get; set; } = string.Empty;

    [JsonPropertyName("results")]
    public QueryResult Results { get; set; } = new();

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("similarity_threshold")]
    public double? SimilarityThreshold { get; set; }

    [JsonPropertyName("filters")]
    public Dictionary<string, object>? Filters { get; set; }
}

public class DocumentResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("document")]
    public string Document { get; set; } = string.Empty;

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

public class CollectionResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    [JsonPropertyName("document_count")]
    public int? DocumentCount { get; set; }
}

public class HealthResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("collections_count")]
    public int CollectionsCount { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}

public class StatsResponse
{
    [JsonPropertyName("total_collections")]
    public int TotalCollections { get; set; }

    [JsonPropertyName("total_documents")]
    public int TotalDocuments { get; set; }

    [JsonPropertyName("collections")]
    public List<CollectionStatsInfo> Collections { get; set; } = new();
}

public class CollectionStatsInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("document_count")]
    public int DocumentCount { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

public class EmbeddingResponse
{
    [JsonPropertyName("embeddings")]
    public List<List<double>> Embeddings { get; set; } = new();

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("dimension")]
    public int Dimension { get; set; }
}

public class SimilarDocumentsResponse
{
    [JsonPropertyName("reference_document_id")]
    public string ReferenceDocumentId { get; set; } = string.Empty;

    [JsonPropertyName("similar_documents")]
    public QueryResult SimilarDocuments { get; set; } = new();

    [JsonPropertyName("count")]
    public int Count { get; set; }
}

// Utility classes for RAG processing
public class RAGContext
{
    public string Query { get; set; } = string.Empty;
    public List<RelevantDocument> RelevantDocuments { get; set; } = new();
    public Dictionary<string, object>? ApiData { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class RelevantDocument
{
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public double Relevance { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
    public string Source { get; set; } = string.Empty;
}
