import os
import logging
from typing import List, Optional, Dict, Any
from fastapi import FastAPI, HTTPException, status
from fastapi.middleware.cors import CORSMiddleware
import chromadb
from chromadb.config import Settings
from chromadb.utils import embedding_functions
from pydantic import BaseModel, Field
import uvicorn
import sys
from pathlib import Path
from datetime import datetime
from fastapi import UploadFile, File, Form
from rag_data_processing import SUPPORTED_TYPES
from enum import Enum

project_root = Path(__file__).parent.parent
scripts_dir = project_root / "scripts"
sys.path.extend([str(project_root), str(scripts_dir)])

from rag_data_processing import process_documents_by_doc_type

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI(
    title="Rift VectorDB API",
    description="Enhanced ChromaDB API for Oceanographic RAG System",
    version="1.0.0"
)

# Add CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# ChromaDB Configuration optimized for oceanographic data
CHROMA_SETTINGS = Settings(
    persist_directory="./chroma_data",
    anonymized_telemetry=False,
    allow_reset=True
)
chroma_client = chromadb.PersistentClient(path="./chroma_data")
embedding_function = embedding_functions.SentenceTransformerEmbeddingFunction(
    model_name="all-MiniLM-L6-v2"  # Good balance of performance and speed for scientific text
)

class Document(BaseModel):
    id: str
    text: str
    metadata: Optional[Dict[str, Any]] = None

class BatchDocuments(BaseModel):
    documents: List[Document]
    collection_name: str = "oceanographic_data"

class AddRequest(BaseModel):
    id: str
    text: str
    metadata: Optional[Dict[str, Any]] = None
    collection_name: str = "oceanographic_data"

class QueryRequest(BaseModel):
    text: str
    n_results: int = Field(default=5, ge=1, le=50)
    collection_name: str = "oceanographic_data"
    where: Optional[Dict[str, Any]] = None  # Metadata filters
    include: List[str] = Field(default=["documents", "metadatas", "distances"])

class SemanticQueryRequest(BaseModel):
    text: str
    n_results: int = Field(default=5, ge=1, le=50)
    collection_name: str = "oceanographic_data"
    similarity_threshold: float = Field(default=0.0, ge=0.0, le=1.0)
    where: Optional[Dict[str, Any]] = None

class HybridQueryRequest(BaseModel):
    text: str
    keywords: Optional[List[str]] = None
    n_results: int = Field(default=5, ge=1, le=50)
    collection_name: str = "oceanographic_data"
    semantic_weight: float = Field(default=0.7, ge=0.0, le=1.0)
    where: Optional[Dict[str, Any]] = None

class CollectionInfo(BaseModel):
    name: str
    description: Optional[str] = None
    metadata: Optional[Dict[str, Any]] = None

class UpdateDocumentRequest(BaseModel):
    text: Optional[str] = None
    metadata: Optional[Dict[str, Any]] = None

# Enum for supported doc types (for OpenAPI dropdown)
class DocTypeEnum(str, Enum):
    cambridge_bay_papers = "cambridge_bay_papers"
    cambridge_bay_web_articles = "cambridge_bay_web_articles"
    confluence_json = "confluence_json"

# Collection Management
def get_or_create_collection(collection_name: str):
    """Gets a collection or creates it if it doesn't exist."""
    try:
        collection = chroma_client.get_collection(name=collection_name)
        logger.info(f"Found existing collection: {collection_name}")
        return collection
    except:
        logger.info(f"Collection '{collection_name}' not found. Creating a new one.")
        collection = chroma_client.create_collection(
            name="oceanographic_data",
            metadata={
                "description": "Default collection for Ocean Networks Canada (ONC) data",
                "created_for": "rift_rag_system",
                "created_at": datetime.utcnow().isoformat() + 'Z'
            },
            embedding_function=embedding_function 
        )
        logger.info(f"Created collection: {collection_name}")
        return collection

@app.get("/collections")
async def list_collections():
    """List all available collections."""
    try:
        collections = chroma_client.list_collections()
        return {
            "collections": [
                {
                    "name": col.name,
                    "id": col.id,
                    "metadata": col.metadata
                } for col in collections
            ]
        }
    except Exception as e:
        logger.error(f"Failed to list collections: {str(e)}")
        raise HTTPException(status_code=500, detail="Failed to list collections")

@app.get("/collections/{collection_name}")
async def get_collection_info(collection_name: str):
    """Get information about a specific collection."""
    try:
        collection = chroma_client.get_collection(collection_name)
        count = collection.count()
        return {
            "name": collection.name,
            "id": collection.id,
            "metadata": collection.metadata,
            "document_count": count
        }
    except Exception as e:
        logger.error(f"Failed to get collection {collection_name}: {str(e)}")
        raise HTTPException(status_code=404, detail="Collection not found")

@app.delete("/collections/{collection_name}")
async def delete_collection(collection_name: str):
    """Delete a collection and all its documents."""
    try:
        chroma_client.delete_collection(collection_name)
        logger.info(f"Deleted collection: {collection_name}")
        return {"status": "deleted", "name": collection_name}
    except Exception as e:
        logger.error(f"Failed to delete collection {collection_name}: {str(e)}")
        raise HTTPException(status_code=404, detail="Collection not found")

@app.post("/documents/initial")
async def add_initial_documents():
    """Add the initial documents that exist internally within the repo. Run with caution - this can take several minutes."""
    doc_types = list(SUPPORTED_TYPES.keys())
    batch_size = 100
    
    try:
        collection = get_or_create_collection(collection_name="oceanographic_data")
        
        # Prepare lists to hold all data from all doc_types
        all_documents = []
        all_ids = []
        all_metadatas = []
        name_counters = {} # To generate unique IDs

        for doc_type in doc_types:
            logger.info(f"Processing {doc_type}...")
            documents_chunks_list = process_documents_by_doc_type(doc_type) # chunking function

            # Process all chunks in a single, more efficient loop
            for doc_chunks in documents_chunks_list:
                for chunk in doc_chunks:
                    # Append data directly to the main lists
                    all_documents.append(chunk['text'])
                    all_metadatas.append(chunk['metadata'])
                    
                    # Generate and append unique ID in the same loop
                    name = chunk['metadata'].get('name', 'unnamed_doc')
                    idx = name_counters.get(name, 0)
                    all_ids.append(f"{name}_{idx}")
                    name_counters[name] = idx + 1
            logger.info(f"Completed processing {doc_type} - {len(all_documents)} total documents so far")
        
        if not all_documents:
            logger.info("No documents found to process.")
            return {"status": "no documents found"}

        # Add all collected documents to ChromaDB in batches
        total_batches = (len(all_documents) + batch_size - 1) // batch_size
        logger.info(f"Adding {len(all_documents)} documents in {total_batches} batches...")

        for i in range(0, len(all_documents), batch_size):
            batch_docs = all_documents[i:i + batch_size]
            batch_ids = all_ids[i:i + batch_size]
            batch_metadatas = all_metadatas[i:i + batch_size]
            
            logger.info(f"Processing batch {(i//batch_size)+1}/{total_batches}")

            collection.add(
                documents=batch_docs,
                ids=batch_ids,
                metadatas=batch_metadatas
            )

        logger.info(f"Successfully added {len(all_documents)} total documents to the 'oceanographic_data' collection.")
        return {"status": "added", "total_documents": len(all_documents)}
        
    except Exception as e:
        logger.error(f"Failed to add initial documents: {e}", exc_info=True)
        raise HTTPException(status_code=500, detail=f"An error occurred during initial document processing: {e}")

@app.get("/supported_doc_types")
def get_supported_doc_types():
    """Return the supported doc types for use in frontends."""
    return {"doc_types": list(SUPPORTED_TYPES.keys())}

@app.post("/documents/add")
async def add_batch_documents(
    file: UploadFile = File(...),
    doc_type: DocTypeEnum = Form(...),
    collection_name: str = Form("oceanographic_data")
):
    """Add a single document to the collection via file upload and doc type selection (Enum). Only adds new documents; returns error if any document ID already exists."""
    batch_size = 100
    try:
        collection = get_or_create_collection(collection_name=collection_name)
        all_documents = []
        all_ids = []
        all_metadatas = []
        name_counters = {}
        duplicate_ids = []
        content = (await file.read()).decode("utf-8")
        filename = file.filename
        raw_docs = [{'content': content, 'filename': filename}]
        if doc_type not in SUPPORTED_TYPES:
            raise HTTPException(status_code=400, detail=f"Unsupported doc_type: {doc_type}")
        processor_cls = SUPPORTED_TYPES[doc_type]
        processor = processor_cls(raw_docs)
        chunks = processor.chunk_with_metadata()
        for chunk in chunks:
            name = chunk['metadata'].get('name', filename)
            idx = name_counters.get(name, 0)
            chunk_id = f"{name}_{idx}"
            name_counters[name] = idx + 1
            # Check if this ID already exists
            try:
                existing = collection.get(ids=[chunk_id], include=["ids"])
                ids_list = existing.get("ids", [])
                exists = ids_list and ids_list[0] is not None
            except Exception:
                exists = False
            if exists:
                raise HTTPException(status_code=400, detail=f"Document ID already exists: {chunk_id}")
            else:
                all_documents.append(chunk['text'])
                all_metadatas.append(chunk['metadata'])
                all_ids.append(chunk_id)
        if duplicate_ids:
            return {"status": "error", "detail": f"The following document IDs already exist and were not added: {duplicate_ids}", "duplicate_ids": duplicate_ids}, 400
        if not all_documents:
            return {"status": "no documents found"}
        # Add in batches (should only be one batch, but keep for consistency)
        for i in range(0, len(all_documents), batch_size):
            batch_docs = all_documents[i:i + batch_size]
            batch_ids = all_ids[i:i + batch_size]
            batch_metadatas = all_metadatas[i:i + batch_size]
            collection.add(documents=batch_docs, ids=batch_ids, metadatas=batch_metadatas)
        return {
            "status": "completed",
            "results": [
                {"id": cid, "status": "added"} for cid in all_ids
            ],
            "file": file.filename,
            "doc_type": str(doc_type)
        }
    except Exception as e:
        logger.error(f"Failed to add document: {e}", exc_info=True)
        raise HTTPException(status_code=500, detail=f"Failed to add document: {e}")


@app.get("/documents/{document_id}")
async def get_document(document_id: str, collection_name: str = "oceanographic_data"):
    """Get a specific document by ID."""
    try:
        collection = chroma_client.get_collection(
            name=collection_name
        )

        result = collection.get(ids=[document_id], include=["documents", "metadatas"])

        if not result["documents"] or len(result["documents"]) == 0:
            raise HTTPException(status_code=404, detail="Document not found")

        return {
            "id": document_id,
            "document": result["documents"][0],
            "metadata": result["metadatas"][0] if result["metadatas"] else None
        }

    except HTTPException:
        raise
    except Exception as e:
        logger.error(f"Failed to get document {document_id}: {str(e)}")
        raise HTTPException(status_code=500, detail="Failed to retrieve document")

@app.get("/documents/by-source/{source_doc}")
async def get_documents_by_source(
    source_doc: str,
    collection_name: str = "oceanographic_data"
):
    """
    Fetch *all* documents whose metadata.source_doc == source_doc.
    """
    collection = chroma_client.get_collection(name=collection_name)

    # Pull back ids, docs and metadatas matching the filter
    result = collection.get(
        where   = {"source_doc": {"$eq": source_doc}},
        include = ["documents", "metadatas"]
    )

    ids   = result.get("ids", [])
    docs  = result.get("documents", [])
    metas = result.get("metadatas", [])

    if not ids:
        raise HTTPException(
            status_code=404,
            detail=f"No documents found with source_doc = '{source_doc}'"
        )

    # Build a list of JSON objects
    out: List[Dict[str, Any]] = []
    for _id, text, meta in zip(ids, docs, metas):
        out.append({
            "id":       _id,
            "document": text,
            "metadata": meta
        })

    return {"documents": out}

@app.get("/collections/{collection_name}/documents")
async def get_all_documents(collection_name: str = "oceanographic_data"):
    try:
        collection = chroma_client.get_collection(collection_name)
        if not collection:
            raise HTTPException(status_code=404, detail="Collection not found")

        # Only include valid options
        results = collection.get(include=["documents", "metadatas"])
        # results will have "ids" key by default

        documents = [
            {
                "id": doc_id,
                "text": doc_text,
                "metadata": metadata
            }
            for doc_id, doc_text, metadata in zip(
                results.get("ids", []),
                results.get("documents", []),
                results.get("metadatas", [])
            )
        ]
        return {"documents": documents}
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Failed to get documents: {str(e)}")

@app.delete("/documents/{document_id}")
async def delete_document(document_id: str, collection_name: str = "oceanographic_data"):
    """Delete a specific document."""
    try:
        collection = chroma_client.get_collection(
            name=collection_name
        )

        collection.delete(ids=[document_id])

        logger.info(f"Deleted document {document_id}")
        return {"status": "deleted", "id": document_id}

    except Exception as e:
        logger.error(f"Failed to delete document {document_id}: {str(e)}")
        raise HTTPException(status_code=400, detail=f"Failed to delete document: {str(e)}")

@app.delete("/documents/by-source/{source_doc}")
async def delete_documents_by_source_doc(
    source_doc: str,
    collection_name: str = "oceanographic_data"
):
    """
    Delete all documents whose metadata.source_doc == source_doc.
    """
    collection = chroma_client.get_collection(name=collection_name)

    # find all matching IDs
    results        = collection.get(
        where   = {"source_doc": {"$eq": source_doc}},
#         include = ["ids"]
    )
    ids_to_delete = results["ids"]

    if not ids_to_delete:
        raise HTTPException(
            status_code=404,
            detail=f"No documents found with source_doc '{source_doc}'"
        )

    deleted_count = collection.delete(ids=ids_to_delete)

    return {
        "status":     "deleted",
        "source_doc": source_doc,
        "deleted_ids": ids_to_delete,
        "count":      deleted_count
    }

# Query Endpoints
@app.post("/query")
async def basic_query(request: QueryRequest):
    """Basic similarity search query."""
    try:
        collection = chroma_client.get_collection(
            name=request.collection_name
        )

        result = collection.query(
            query_texts=[request.text],
            n_results=request.n_results,
            where=request.where,
            include=request.include
        )

        return {
            "query": request.text,
            "results": result,
            "count": len(result["documents"][0]) if result["documents"] else 0
        }

    except Exception as e:
        logger.error(f"Query failed: {str(e)}")
        raise HTTPException(status_code=400, detail=f"Query failed: {str(e)}")

@app.post("/query/semantic")
async def semantic_query(request: SemanticQueryRequest):
    """Enhanced semantic search with similarity filtering."""
    try:
        collection = chroma_client.get_collection(
            name=request.collection_name
        )

        result = collection.query(
            query_texts=[request.text],
            n_results=min(request.n_results * 2, 100),  # Get more results for filtering
            where=request.where,
            include=["documents", "metadatas", "distances"]
        )

        # Filter by similarity threshold with improved similarity calculation
        filtered_results = {
            "documents": [[]],
            "metadatas": [[]],
            "distances": [[]],
            "ids": [[]]
        }

        if result["documents"] and result["documents"][0]:
            for i, distance in enumerate(result["distances"][0]):
                # Handle different distance metrics more robustly
                # For cosine distance: similarity = 1 - distance
                # Clamp distance to reasonable range to avoid negative similarities
                clamped_distance = max(0.0, min(2.0, distance))
                similarity = 1.0 - (clamped_distance / 2)

                # Use a more lenient threshold if the requested threshold is too high
                effective_threshold = min(request.similarity_threshold, 0.5) if request.similarity_threshold > 0.8 else request.similarity_threshold

                if similarity >= effective_threshold and len(filtered_results["documents"][0]) < request.n_results:
                    filtered_results["documents"][0].append(result["documents"][0][i])
                    filtered_results["metadatas"][0].append(result["metadatas"][0][i] if result["metadatas"] else None)
                    filtered_results["distances"][0].append(distance)
                    filtered_results["ids"][0].append(result["ids"][0][i] if "ids" in result and result["ids"] else None)
                    logger.debug(f"Document {i}: distance={distance:.4f}, similarity={similarity:.4f}, threshold={effective_threshold:.4f}")

        return {
            "query": request.text,
            "results": filtered_results,
            "count": len(filtered_results["documents"][0]),
            "similarity_threshold": request.similarity_threshold
        }

    except Exception as e:
        logger.error(f"Semantic query failed: {str(e)}")
        raise HTTPException(status_code=400, detail=f"Semantic query failed: {str(e)}")

@app.post("/query/filtered")
async def filtered_query(request: QueryRequest):
    """Query with advanced metadata filtering."""
    try:
        collection = chroma_client.get_collection(
            name=request.collection_name
        )

        result = collection.query(
            query_texts=[request.text],
            n_results=request.n_results,
            where=request.where,
            include=request.include
        )

        return {
            "query": request.text,
            "filters": request.where,
            "results": result,
            "count": len(result["documents"][0]) if result["documents"] else 0
        }

    except Exception as e:
        logger.error(f"Filtered query failed: {str(e)}")
        raise HTTPException(status_code=400, detail=f"Filtered query failed: {str(e)}")

@app.get("/similar/{document_id}")
async def find_similar(document_id: str, collection_name: str = "oceanographic_data", n_results: int = 5):
    """Find documents similar to a specific document."""
    try:
        collection = chroma_client.get_collection(
            name=collection_name
        )

        # Get the document first
        doc_result = collection.get(ids=[document_id], include=["documents"])
        if not doc_result["documents"] or len(doc_result["documents"]) == 0:
            raise HTTPException(status_code=404, detail="Document not found")

        # Query for similar documents
        result = collection.query(
            query_texts=[doc_result["documents"][0]],
            n_results=n_results + 1,  # +1 to exclude the original document
            include=["documents", "metadatas", "distances"]
        )

        # Remove the original document from results
        filtered_results = {
            "documents": [[]],
            "metadatas": [[]],
            "distances": [[]],
            "ids": [[]]
        }

        if result["documents"] and result["documents"][0]:
            for i, doc in enumerate(result["documents"][0]):
                if result["ids"][0][i] != document_id and len(filtered_results["documents"][0]) < n_results:
                    filtered_results["documents"][0].append(doc)
                    filtered_results["metadatas"][0].append(result["metadatas"][0][i] if result["metadatas"] else None)
                    filtered_results["distances"][0].append(result["distances"][0][i])
                    filtered_results["ids"][0].append(result["ids"][0][i])

        return {
            "reference_document_id": document_id,
            "similar_documents": filtered_results,
            "count": len(filtered_results["documents"][0])
        }

    except HTTPException:
        raise
    except Exception as e:
        logger.error(f"Similar document search failed: {str(e)}")
        raise HTTPException(status_code=400, detail=f"Similar document search failed: {str(e)}")

# Analytics and Management
@app.get("/health")
async def health_check():
    """Health check endpoint."""
    try:
        collections = chroma_client.list_collections()
        return {
            "status": "healthy",
            "collections_count": len(collections),
            "version": "1.0.0"
        }
    except Exception as e:
        logger.error(f"Health check failed: {str(e)}")
        raise HTTPException(status_code=503, detail="Service unhealthy")

@app.get("/stats")
async def get_stats():
    """Get comprehensive statistics about the vector database."""
    try:
        collections = chroma_client.list_collections()
        stats = {
            "total_collections": len(collections),
            "collections": []
        }

        total_documents = 0
        for collection in collections:
            col = chroma_client.get_collection(collection.name)
            count = col.count()
            total_documents += count

            stats["collections"].append({
                "name": collection.name,
                "document_count": count,
                "metadata": collection.metadata
            })

        stats["total_documents"] = total_documents
        return stats

    except Exception as e:
        logger.error(f"Stats retrieval failed: {str(e)}")
        raise HTTPException(status_code=500, detail="Failed to retrieve stats")

@app.post("/embeddings")
async def generate_embeddings(texts: List[str]):
    """Generate embeddings for given texts without storing them."""
    try:
        # For now, return a placeholder - embeddings are handled internally by ChromaDB
        return {
            "message": "Embeddings are generated internally by ChromaDB",
            "texts_count": len(texts),
            "status": "processed"
        }
    except Exception as e:
        logger.error(f"Embedding generation failed: {str(e)}")
        raise HTTPException(status_code=400, detail=f"Embedding generation failed: {str(e)}")

# Initialize default collection on startup
@app.on_event("startup")
async def startup_event():
    """Initialize default collection for oceanographic data."""
    try:
        # Try to get existing collection
        try:
            collection = chroma_client.get_collection(
                name="oceanographic_data"
            )
            logger.info("Found existing oceanographic_data collection")
        except:
            # Create default collection if it doesn't exist
            collection = chroma_client.create_collection(
                name="oceanographic_data",
                metadata={
                    "description": "Default collection for Ocean Networks Canada (ONC) data",
                    "created_for": "rift_rag_system"
                },
                embedding_function = embedding_function
            )
            logger.info("Created default oceanographic_data collection with improved embedding function")

    except Exception as e:
        logger.error(f"Failed to initialize default collection: {str(e)}")

if __name__ == "__main__":
    uvicorn.run(
        "main:app",
        host="0.0.0.0",
        port=8000,
        reload=True,
        log_level="info"
    )
