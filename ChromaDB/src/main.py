import os
import logging
from typing import List, Optional, Dict, Any
from fastapi import FastAPI, HTTPException, status, UploadFile, File
from fastapi.middleware.cors import CORSMiddleware
import chromadb
from chromadb.config import Settings
from chromadb.utils import embedding_functions
from pydantic import BaseModel, Field
import uvicorn
import sys
from pathlib import Path

project_root = Path(__file__).parent.parent
scripts_dir = project_root / "scripts"
sys.path.extend([str(project_root), str(scripts_dir)])

from scripts.rag_data_processing import process_documents_by_doc_type, validate_file_type

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI(
    title="Rift VectorDB API",
    description="Enhanced ChromaDB API for Oceanographic RAG System",
    version="1.0.0"
)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

CHROMA_SETTINGS = Settings(
    persist_directory="./chroma_data",
    anonymized_telemetry=False,
    allow_reset=True
)
chroma_client = chromadb.PersistentClient(path="./chroma_data")
embedding_function = embedding_functions.SentenceTransformerEmbeddingFunction(
    model_name="all-mpnet-base-v2"
)

class DocumentMetadata(BaseModel):
    source: str
    data_type: str
    timestamp: Optional[str] = None
    location: Optional[str] = None
    depth: Optional[float] = None
    instrument_type: Optional[str] = None
    tags: Optional[str] = None

class Document(BaseModel):
    id: str
    text: str
    metadata: Optional[DocumentMetadata] = None

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
    where: Optional[Dict[str, Any]] = None
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

# --- Helper Functions ---
def get_or_create_collection(collection_name: str, metadata: Optional[Dict[str, Any]] = None):
    """Gets a collection or creates it if it doesn't exist."""
    try:
        collection = chroma_client.get_collection(name=collection_name)
        logger.info(f"Found existing collection: {collection_name}")
        return collection
    except:
        logger.info(f"Collection '{collection_name}' not found. Creating a new one.")
        collection = chroma_client.create_collection(
            name=collection_name,
            metadata=metadata,
            embedding_function=embedding_function,
        )
        logger.info(f"Created collection: {collection_name}")
        return collection

# --- API Endpoints ---
@app.get("/collections")
async def list_collections():
    """List all available collections."""
    try:
        collections = chroma_client.list_collections()
        return {
            "collections": [
                {"name": col.name, "id": col.id, "metadata": col.metadata, "count": col.count()}
                for col in collections
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
        raise HTTPException(status_code=500, detail="Internal server error")

@app.post("/documents/add")
async def add_document(file: UploadFile = File(...), collection_name: str = "oceanographic_data"):
    """Add a single document to a collection from an uploaded file."""
    try:
        validate_file_type(file.filename, file.content_type)
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))
    try:
        collection = get_or_create_collection(collection_name)
        file_content = await file.read()
        document_text = file_content.decode("utf-8")
        file_id = file.filename
        
        collection.add(
            documents=[document_text],
            ids=[file_id],
            metadatas=[{"filename": file.filename, "source": "file_upload"}]
        )
        logger.info(f"Added document {file_id} to {collection_name}")
        return {"status": "added", "file": file.filename, "collection": collection_name}
    except Exception as e:
        logger.error(f"Failed to add document from file {file.filename}: {str(e)}")
        raise HTTPException(status_code=500, detail=f"Failed to add document: {str(e)}")

@app.post("/documents/initial")
async def add_initial_documents():
    """Add initial documents that exist internally within the repo. Run with caution - this can take several minutes."""
    doc_types = ["cambridge_bay_papers", "cambridge_bay_web_articles", "confluence_json"]
    BATCH_SIZE = 100

    try:
        for doc_type in doc_types:
            # Get a list of lists: each sublist is chunks for one document
            documents_chunks = process_documents_by_doc_type(doc_type)
            for doc_chunks in documents_chunks:
                if not doc_chunks:
                    continue
                
                document_metadata = doc_chunks[0]['metadata']
                collection_name = document_metadata['name']
                collection_metadata = {
                    'created_at': document_metadata.get('created_at'),
                    'name': document_metadata.get('name'),
                    'source_type': document_metadata.get('source_type')
                }
                collection = get_or_create_collection(collection_name, collection_metadata)
                # Prepare data for ChromaDB
                documents = [chunk['text'] for chunk in doc_chunks]
                ids = [f"{collection_name}_{i}" for i, chunk in enumerate(doc_chunks)]
                metadatas = [chunk['metadata'] for chunk in doc_chunks]

                # Add documents in batches
                for i in range(0, len(documents), BATCH_SIZE):
                    batch_docs = documents[i:i + BATCH_SIZE]
                    batch_ids = ids[i:i + BATCH_SIZE]
                    batch_metadatas = metadatas[i:i + BATCH_SIZE]
                    
                    collection.add(
                        documents=batch_docs,
                        ids=batch_ids,
                        metadatas=batch_metadatas
                    )

                logger.info(f"Added {len(documents)} documents to {collection_name}")
        return {"status": "added"}
    except Exception as e:
        logger.error(f"Failed to add batch documents: {str(e)}")
        raise HTTPException(status_code=400, detail=f"Failed to add documents: {str(e)}")


@app.post("/documents/batch")
async def add_batch_documents(files: List[UploadFile] = File(...), collection_name: str = "oceanographic_data"):
    """Adds multiple documents to a collection in batch from uploaded files."""
    collection = get_or_create_collection(collection_name)
    added_files, errors = [], []

    for file in files:
        try:
            validate_file_type_util(file.filename, file.content_type)
            content = await file.read()
            # TODO: chuck content
            collection.add(
                documents=[content.decode("utf-8")],
                ids=[file.filename],
                metadatas=[{"filename": file.filename, "source": "batch_upload"}]
            )
            added_files.append(file.filename)
        except (ValueError, Exception) as e:
            logger.error(f"Failed to process file {file.filename} in batch: {e}")
            errors.append({"file": file.filename, "error": str(e)})

    if errors:
        return {"status": "completed_with_errors", "added_files": added_files, "errors": errors}
    
    return {"status": "added", "files": added_files, "collection": collection_name}

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

@app.put("/documents/{document_id}")
async def update_document(document_id: str, request: UpdateDocumentRequest, collection_name: str = "oceanographic_data"):
    """Update a document's content or metadata."""
    try:
        collection = chroma_client.get_collection(
            name=collection_name
        )
        
        update_data = {}
        if request.text:
            update_data["documents"] = [request.text]
        if request.metadata:
            # Clean metadata to remove None values
            clean_metadata = {k: v for k, v in request.metadata.items() if v is not None}
            if clean_metadata:
                update_data["metadatas"] = [clean_metadata]
            
        if not update_data:
            raise HTTPException(status_code=400, detail="No update data provided")
            
        collection.update(ids=[document_id], **update_data)
        
        logger.info(f"Updated document {document_id}")
        return {"status": "updated", "id": document_id}
        
    except Exception as e:
        logger.error(f"Failed to update document {document_id}: {str(e)}")
        raise HTTPException(status_code=400, detail=f"Failed to update document: {str(e)}")

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
        
        # Filter by similarity threshold
        filtered_results = {
            "documents": [[]],
            "metadatas": [[]],
            "distances": [[]]
        }
        
        if result["documents"] and result["documents"][0]:
            for i, distance in enumerate(result["distances"][0]):
                similarity = 1 - distance  # Convert distance to similarity
                if similarity >= request.similarity_threshold and len(filtered_results["documents"][0]) < request.n_results:
                    filtered_results["documents"][0].append(result["documents"][0][i])
                    filtered_results["metadatas"][0].append(result["metadatas"][0][i] if result["metadatas"] else None)
                    filtered_results["distances"][0].append(distance)
        
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

if __name__ == "__main__":
    uvicorn.run(
        "main:app",
        host="0.0.0.0",
        port=8000,
        reload=True,
        log_level="info"
    )