# VectorDB Setup and Endpoint Documentation

## Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   .NET Backend  │    │  Python FastAPI │    │    ChromaDB     │
│                 │    │   (Port 8000)   │    │   Vector Store  │
│  Controllers    │◄──►│                 │◄──►│                 │
│  Clients        │    │  Embeddings     │    │  Collections    │
│  Models         │    │  REST API       │    │  Documents      │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## Endpoint Categories

### 1. Document Management

#### Add Single Document
- **Endpoint**: `POST /api/vectordb/documents`
- **Purpose**: Add a single document to the vector database
- **Request Body**:
```json
{
  "id": "unique_document_id",
  "text": "Document content text",
  "metadata": {
    "source": "ONC_API",
    "data_type": "sensor_data",
    "location": "Folger Passage",
    "depth": 25.5,
    "instrument_type": "CTD",
    "timestamp": "2024-01-15T14:30:00Z"
  },
  "collection_name": "oceanographic_data"
}
```

#### Add Multiple Documents (Batch)
- **Endpoint**: `POST /api/vectordb/documents/batch`
- **Purpose**: Efficiently add multiple documents at once
- **Request Body**:
```json
{
  "documents": [
    {
      "id": "doc1",
      "text": "Content 1",
      "metadata": {...}
    },
    {
      "id": "doc2",
      "text": "Content 2",
      "metadata": {...}
    }
  ],
  "collection_name": "oceanographic_data"
}
```

#### Get Document
- **Endpoint**: `GET /api/vectordb/documents/{documentId}`
- **Query Parameters**: `collection_name` (optional)
- **Purpose**: Retrieve a specific document by ID

#### Update Document
- **Endpoint**: `PUT /api/vectordb/documents/{documentId}`
- **Purpose**: Update document content or metadata
- **Request Body**:
```json
{
  "text": "Updated content (optional)",
  "metadata": {
    "updated_field": "new_value"
  }
}
```

#### Delete Document
- **Endpoint**: `DELETE /api/vectordb/documents/{documentId}`
- **Query Parameters**: `collection_name` (optional)
- **Purpose**: Remove a document from the collection

### 2. Collection Management

#### Create Collection
- **Endpoint**: `POST /api/vectordb/collections`
- **Purpose**: Create a new collection for organizing documents
- **Request Body**:
```json
{
  "name": "sensor_data",
  "description": "Time-series sensor readings",
  "metadata": {
    "data_types": ["temperature", "salinity", "pressure"],
    "location": "Pacific Northwest"
  }
}
```

#### List Collections
- **Endpoint**: `GET /api/vectordb/collections`
- **Purpose**: Get all available collections

#### Get Collection Info
- **Endpoint**: `GET /api/vectordb/collections/{collectionName}`
- **Purpose**: Get detailed information about a specific collection

#### Delete Collection
- **Endpoint**: `DELETE /api/vectordb/collections/{collectionName}`
- **Purpose**: Delete a collection and all its documents

### 3. Query Operations

#### Basic Similarity Search
- **Endpoint**: `POST /api/vectordb/query`
- **Purpose**: Find documents similar to a query text
- **Request Body**:
```json
{
  "text": "temperature measurements in Saanich Inlet",
  "n_results": 5,
  "collection_name": "oceanographic_data",
  "where": {
    "data_type": "sensor_data",
    "location": "Saanich Inlet"
  },
  "include": ["documents", "metadatas", "distances"]
}
```

#### Semantic Search with Filtering
- **Endpoint**: `POST /api/vectordb/query/semantic`
- **Purpose**: Advanced semantic search with similarity thresholds
- **Request Body**:
```json
{
  "text": "ocean acidification impacts on marine life",
  "n_results": 10,
  "similarity_threshold": 0.75,
  "collection_name": "research_data",
  "where": {
    "data_type": "research_data",
    "tags": {"$contains": "acidification"}
  }
}
```

#### Filtered Query
- **Endpoint**: `POST /api/vectordb/query/filtered`
- **Purpose**: Query with advanced metadata constraints
- **Supports complex filtering**: Date ranges, numeric ranges, array contains, etc.

#### Find Similar Documents
- **Endpoint**: `GET /api/vectordb/documents/{documentId}/similar`
- **Query Parameters**: `collection_name`, `n_results`
- **Purpose**: Find documents similar to a specific document

### 4. RAG-Specific Endpoints

#### Get RAG Context (Primary RAG Endpoint)
- **Endpoint**: `GET /api/vectordb/rag/context`
- **Purpose**: Main endpoint for RAG system to get relevant documents
- **Query Parameters**:
  - `query`: User's natural language query
  - `maxResults`: Maximum number of relevant documents (1-20)
  - `similarityThreshold`: Minimum similarity score (0.0-1.0)
- **Example**: `/api/vectordb/rag/context?query=What is the temperature in Saanich Inlet&maxResults=5&similarityThreshold=0.7`

#### Get Oceanographic RAG Context
- **Endpoint**: `GET /api/vectordb/rag/oceanographic`
- **Purpose**: Filtered RAG context specific to oceanographic data types
- **Query Parameters**:
  - `query`: User query
  - `dataType`: Filter by data type (sensor_data, location_info, etc.)
  - `location`: Filter by location
  - `instrumentType`: Filter by instrument type
  - `maxResults`: Maximum results
  - `similarityThreshold`: Similarity threshold

### 5. Analytics and Management

#### Health Check
- **Endpoint**: `GET /api/vectordb/health`
- **Purpose**: Check if the vector database is healthy and operational

#### Database Statistics
- **Endpoint**: `GET /api/vectordb/stats`
- **Purpose**: Get comprehensive statistics about collections and documents

#### Generate Embeddings
- **Endpoint**: `POST /api/vectordb/embeddings`
- **Purpose**: Generate embeddings for texts without storing them
- **Request Body**: `["text1", "text2", "text3"]`

## Configuration for Oceanographic Data

### Embedding Model
- **Model**: `all-MiniLM-L6-v2`
- **Dimensions**: 384
- **Optimized for**: Scientific and technical content
- **Performance**: Good balance of accuracy and speed

### Default Collection Structure
```json
{
  "name": "oceanographic_data",
  "metadata": {
    "description": "Default collection for Ocean Networks Canada (ONC) data",
    "created_for": "rift_rag_system",
    "data_types": [
      "sensor_data",
      "location_info",
      "instrument_specs",
      "research_data"
    ]
  }
}
```

### Metadata Schema for Oceanographic Data

#### Required Fields
- `source`: Data source identifier (e.g., "ONC_API", "manual_entry")
- `data_type`: Type of data (sensor_data, location_info, instrument_spec, research_data)
- `timestamp`: ISO 8601 formatted timestamp

#### Optional Fields
- `location`: Geographic location or station name
- `depth`: Water depth in meters (numeric)
- `instrument_type`: Type of measuring instrument (CTD, ADCP, etc.)
- `tags`: Array of relevant keywords
- `coordinates`: Lat/Lon as object
- `temperature`: Temperature measurement (if applicable)
- `salinity`: Salinity measurement (if applicable)

### Example Metadata
```json
{
  "source": "ONC_VENUS",
  "data_type": "sensor_data",
  "timestamp": "2024-01-15T14:30:00Z",
  "location": "Saanich Inlet",
  "depth": 23.5,
  "instrument_type": "CTD",
  "tags": ["temperature", "salinity", "conductivity"],
  "coordinates": {
    "latitude": 48.7128,
    "longitude": -125.1234
  }
}
```

## Filtering Examples

### By Data Type
```json
{"data_type": "sensor_data"}
```

### By Location
```json
{"location": "Folger Passage"}
```

### By Depth Range
```json
{
  "depth": {
    "$gte": 10,
    "$lte": 100
  }
}
```

### By Date Range
```json
{
  "timestamp": {
    "$gte": "2024-01-01T00:00:00Z",
    "$lte": "2024-01-31T23:59:59Z"
  }
}
```

### Complex Filters
```json
{
  "data_type": "sensor_data",
  "location": {"$in": ["Saanich Inlet", "Folger Passage"]},
  "depth": {"$lte": 50},
  "tags": {"$contains": "temperature"}
}
```

## RAG Integration Workflow

1. **User Query**: Natural language query received
2. **Context Retrieval**: Call `/api/vectordb/rag/context` or `/api/vectordb/rag/oceanographic`
3. **Relevance Filtering**: Documents filtered by similarity threshold
4. **Context Assembly**: Relevant documents combined with API data
5. **LLM Processing**: Context passed to language model for final response

### RAG Context Response Structure
```json
{
  "status": "success",
  "data": {
    "query": "user query",
    "relevantDocuments": [
      {
        "id": "doc_1",
        "content": "relevant text content",
        "relevance": 0.85,
        "metadata": {...},
        "source": "ONC_API"
      }
    ],
    "timestamp": "2024-01-15T14:30:00Z"
  },
  "message": "Retrieved 5 relevant documents for RAG context"
}
```

## Performance Configuration

### Query Settings
- **Similarity Threshold**: 0.7 (default for balanced precision/recall)
- **Max Results**: 5-10 for RAG context
- **Query Timeout**: 30 seconds
- **Batch Size**: 50 documents per batch operation

### Storage Settings
- **Persistence**: `/chroma_data` directory
- **Backup**: Daily automated backups
- **Collection Optimization**: Automatic background optimization

## Development Tools

### Data Population Script
```bash
# Populate with sample data
python ChromaDB/scripts/populate_data.py --action sample

# Import from CSV
python ChromaDB/scripts/populate_data.py --action csv --file data.csv

# Import from JSON
python ChromaDB/scripts/populate_data.py --action json --file data.json

# Test queries
python ChromaDB/scripts/populate_data.py --action test --query "temperature sensors"
```
.venv/Scripts/Activate
### Health Monitoring
```bash
# Check health
curl http://localhost:8000/health

# Get statistics
curl http://localhost:8000/stats

# Test basic query
curl -X POST http://localhost:8000/query \
  -H "Content-Type: application/json" \
  -d '{"text": "ocean temperature", "n_results": 3}'
```

## Legacy Endpoints (Backward Compatibility)

### Legacy Add
- **Endpoint**: `POST /api/vectordb/add`
- **Status**: Deprecated, use `POST /api/vectordb/documents`

### Legacy Query
- **Endpoint**: `POST /api/vectordb/query` (different from new query endpoint)
- **Status**: Deprecated, use new query endpoints

## Future Enhancements

### Planned Features
1. **Hybrid Search**: Combine semantic and keyword search
2. **Multi-modal Embeddings**: Support for text + time series data
3. **Real-time Streaming**: Live data ingestion from ONC API
4. **Advanced Analytics**: Query performance dashboards
5. **Model Versioning**: A/B testing for embedding models

### Specialized Collections
1. `sensor_data`: Time-series sensor readings
2. `location_info`: Geographic and bathymetric data
3. `instrument_specs`: Technical documentation
4. `research_papers`: Scientific publications
5. `emergency_data`: Tsunami and weather alerts

## Getting Started

1. **Start Services**: `docker-compose up`
2. **Populate Sample Data**: Run the population script
3. **Test Health**: Check `/api/vectordb/health`
4. **Test Query**: Use `/api/vectordb/rag/context` with a sample query
5. **Integrate**: Use RAG endpoints in your application logic

This VectorDB system provides a robust foundation for the Rift oceanographic RAG application, enabling efficient semantic search and retrieval of relevant oceanographic data to enhance LLM responses.
