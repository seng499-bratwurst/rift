# ChromaDB Configuration for Oceanographic RAG System

## Overview

This document outlines the optimal configuration for ChromaDB in the Rift project, specifically designed for Ocean Networks Canada (ONC) data and RAG (Retrieval-Augmented Generation) tasks.

## Embedding Configuration

### Model Selection
- **Primary Model**: `all-MiniLM-L6-v2`
- **Rationale**: 
  - Good balance between performance and accuracy
  - Well-suited for scientific/technical content
  - 384-dimensional embeddings (efficient storage)
  - Fast inference times suitable for real-time queries

### Alternative Models (for future consideration)
- `all-mpnet-base-v2`: Higher accuracy but slower (768 dimensions)
- `multi-qa-MiniLM-L6-cos-v1`: Optimized for question-answering tasks
- `paraphrase-multilingual-MiniLM-L12-v2`: For multilingual support

## Collection Configuration

### Default Collection: `oceanographic_data`
```json
{
  "name": "oceanographic_data",
  "embedding_function": "all-MiniLM-L6-v2",
  "metadata": {
    "description": "Default collection for Ocean Networks Canada (ONC) data",
    "created_for": "rift_rag_system",
    "data_types": ["sensor_data", "location_info", "instrument_specs", "research_data"]
  }
}
```

### Specialized Collections (recommended)
1. **`sensor_data`**: Time-series sensor readings
2. **`location_info`**: Geographic and bathymetric information
3. **`instrument_specs`**: Technical specifications and manuals
4. **`research_papers`**: Scientific publications and reports

## Metadata Schema for Oceanographic Data

### Required Fields
- `source`: Data source identifier
- `data_type`: Type of oceanographic data
- `timestamp`: ISO 8601 formatted timestamp
- `location`: Geographic location or station name

### Optional Fields
- `depth`: Water depth in meters (numeric)
- `instrument_type`: Type of measuring instrument
- `tags`: Array of relevant tags/keywords
- `temperature`: Water temperature (if applicable)
- `salinity`: Salinity measurements (if applicable)
- `coordinates`: Lat/Lon coordinates as object

### Example Metadata Structure
```json
{
  "source": "ONC_API",
  "data_type": "sensor_data",
  "timestamp": "2024-01-15T14:30:00Z",
  "location": "Folger Passage",
  "depth": 23.5,
  "instrument_type": "CTD",
  "tags": ["temperature", "salinity", "conductivity"],
  "coordinates": {
    "latitude": 48.7128,
    "longitude": -125.1234
  }
}
```

## Query Optimization Settings

### Semantic Search Parameters
- **Similarity Threshold**: 0.7 (default)
  - 0.8+: High precision, lower recall
  - 0.6-0.7: Balanced precision/recall
  - <0.6: High recall, lower precision

- **Max Results**: 5-10 for RAG context
- **Include Fields**: `["documents", "metadatas", "distances"]`

### Performance Tuning
- **Batch Size**: 100 documents per batch operation
- **Query Timeout**: 30 seconds
- **Connection Pool**: 10 connections max

## Storage and Persistence

### Data Directory Structure
```
/chroma_data/
├── collections/
│   ├── oceanographic_data/
│   ├── sensor_data/
│   └── location_info/
├── logs/
└── config/
```

### Backup Strategy
- **Frequency**: Daily backups of collection data
- **Retention**: 30 days of backup history
- **Location**: Persistent volume mount

## Filtering Strategies

### Data Type Filters
```python
# Sensor data only
{"data_type": "sensor_data"}

# Location information
{"data_type": "location_info"}

# Multiple types
{"data_type": {"$in": ["sensor_data", "location_info"]}}
```

### Temporal Filters
```python
# Recent data (last 30 days)
{
  "timestamp": {
    "$gte": "2024-01-01T00:00:00Z",
    "$lte": "2024-01-31T23:59:59Z"
  }
}
```

### Depth-based Filters
```python
# Surface waters (0-50m)
{"depth": {"$lte": 50}}

# Deep waters (>200m)
{"depth": {"$gte": 200}}

# Specific depth range
{
  "depth": {
    "$gte": 10,
    "$lte": 100
  }
}
```

### Location-based Filters
```python
# Specific location
{"location": "Folger Passage"}

# Multiple locations
{"location": {"$in": ["Folger Passage", "Saanich Inlet"]}}
```

## RAG-Specific Configuration

### Context Window Optimization
- **Max Context Length**: 4000 tokens
- **Document Chunking**: 512 tokens per chunk with 50 token overlap
- **Relevance Ranking**: Combine semantic similarity with metadata relevance

### Query Enhancement
- **Query Expansion**: Add domain-specific synonyms
- **Context Injection**: Include location and temporal context
- **Multi-modal**: Support for text + metadata queries

## Monitoring and Analytics

### Key Metrics
- Query response time (target: <500ms)
- Embedding generation time
- Collection size and growth rate
- Query success rate
- Similarity score distributions

### Health Checks
- Database connectivity
- Collection accessibility
- Embedding model availability
- Memory usage
- Disk space

## Security Configuration

### Access Control
- API key authentication for external access
- Rate limiting: 100 requests/minute per client
- Input validation for all endpoints

### Data Privacy
- No PII storage in embeddings
- Secure metadata handling
- Audit logging for data access

## Environment Variables

```bash
# ChromaDB Configuration
CHROMA_HOST=0.0.0.0
CHROMA_PORT=8000
CHROMA_PERSIST_DIRECTORY=/chroma_data
CHROMA_LOG_LEVEL=INFO

# Embedding Configuration
EMBEDDING_MODEL=all-MiniLM-L6-v2
EMBEDDING_BATCH_SIZE=32
EMBEDDING_CACHE_SIZE=1000

# Performance Settings
MAX_CONCURRENT_QUERIES=10
QUERY_TIMEOUT=30
CONNECTION_POOL_SIZE=10

# Security
API_KEY_REQUIRED=false
RATE_LIMIT_ENABLED=true
RATE_LIMIT_RPM=100
```

## Best Practices

### Data Ingestion
1. Validate metadata before ingestion
2. Use consistent ID schemes
3. Batch operations for better performance
4. Handle duplicate detection

### Query Optimization
1. Use appropriate similarity thresholds
2. Leverage metadata filters
3. Implement query caching
4. Monitor query performance

### Maintenance
1. Regular collection optimization
2. Monitor storage usage
3. Update embeddings for improved models
4. Archive old data periodically

## Troubleshooting

### Common Issues
1. **Slow Queries**: Check similarity thresholds and collection size
2. **Out of Memory**: Reduce batch sizes or increase container memory
3. **Connection Timeouts**: Increase timeout settings or check network
4. **Poor Results**: Adjust similarity thresholds or improve metadata

### Debug Commands
```bash
# Check collection health
curl http://chromadb:8000/health

# Get collection stats
curl http://chromadb:8000/stats

# Test embedding generation
curl -X POST http://chromadb:8000/embeddings \
  -H "Content-Type: application/json" \
  -d '["test oceanographic query"]'
```

## Future Enhancements

### Planned Features
1. Hybrid search (semantic + keyword)
2. Multi-modal embeddings (text + time series)
3. Federated search across multiple collections
4. Real-time data streaming integration
5. Advanced analytics dashboard

### Model Upgrades
- Evaluate domain-specific embedding models
- Fine-tune models on oceanographic data
- Implement model versioning and A/B testing