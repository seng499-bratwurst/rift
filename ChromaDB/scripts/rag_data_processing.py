import os
import json
from pathlib import Path
from typing import List, Dict, Tuple

from processors import ResearchPapers, ConfluenceJson, CambridgeBayArticles

DATA_DIR = Path(__file__).resolve().parents[2] / "Dataset" / "Markdown"
SUPPORTED_TYPES = {
    "cambridge_bay_papers": ("paper", ResearchPapers),
    "cambridge_bay_web_articles": ("web_article", CambridgeBayArticles),
    "confluence_json": ("json", ConfluenceJson),
}
ALLOWED_TYPES = {
    "application/pdf": ".pdf", "text/plain": ".txt",
    "application/json": ".json", "text/markdown": ".md"
}
ALLOWED_EXTENSIONS = set(ALLOWED_TYPES.values())


def process_documents_by_doc_type(doc_type: str) -> List[List[dict]]:
    """
    Process documents within a specific document type directory name.
    Returns a list where each element is a list of chunks for one file.
    The length of the returned list equals the number of files in the directory.
    """
    if doc_type not in SUPPORTED_TYPES:
        raise ValueError(f"Unsupported doc_type: {doc_type}. Supported types: {list(SUPPORTED_TYPES.keys())}")

    source_type, handler_cls = SUPPORTED_TYPES[doc_type]
    
    try:
        # Get the directory path for this doc_type
        full_path = Path(os.path.join(DATA_DIR, doc_type))
        if not full_path.exists():
            print(f"Directory not found: {full_path}")
            return []
        
        # Get all files in the directory
        files = sorted(full_path.glob("*"))
        print(f"Found {len(files)} files in {doc_type}")
        
        # Process each file individually
        all_document_chunks = []
        for file_path in files:
            try:
                # Load single file
                with open(file_path, "r", encoding="utf-8") as file:
                    raw_docs = [{'content': file.read(), 'filename': file_path.name}]
                
                # Process this single file
                processor = handler_cls(raw_docs)
                chunks_for_file = processor.chunk_with_metadata()
                
                # Group chunks by document (in case one file has multiple documents)
                doc_groups = {}
                for chunk in chunks_for_file:
                    doc_id = chunk['metadata'].get('source_type', file_path.stem)
                    doc_groups.setdefault(doc_id, []).append(chunk)
                
                # Add each document group as a separate element
                all_document_chunks.extend(doc_groups.values())
                
            except Exception as e:
                print(f"Error processing file {file_path}: {e}")
                continue
        
        print(f"Processed {len(all_document_chunks)} documents from {len(files)} files")
        return all_document_chunks
    except Exception as e:
        logger.error("fail")

# --- Centralized Helper Functions ---

def validate_file_type(filename: str, content_type: str):
    """
    Validates the file's extension and MIME type against allowed lists.
    Raises ValueError if the file type is not allowed.
    """
    if content_type not in ALLOWED_TYPES:
        raise ValueError(f"File type '{content_type}' is not allowed.")
    if not any(filename.lower().endswith(ext) for ext in ALLOWED_EXTENSIONS):
        raise ValueError("File extension is not in the list of allowed extensions.")

def prepare_collection_metadata(document_metadata: dict) -> dict:
    """
    Prepares a clean metadata dictionary for a ChromaDB collection from a chunk's metadata.
    It removes chunk-specific keys.
    """
    # Keys to exclude from the top-level collection metadata.
    excluded_keys = ['item_name', 'item_code', 'section', 'chunk_index', 'length']
    return {k: v for k, v in document_metadata.items() if k not in excluded_keys}

def prepare_documents_for_chroma(doc_chunks: list, collection_name: str) -> Tuple[List[str], List[str], List[Dict]]:
    """
    Prepares lists of documents, unique IDs, and metadata for a ChromaDB .add() call.
    """
    documents, ids, metadatas = [], [], []
    for i, chunk in enumerate(doc_chunks):
        documents.append(chunk['text'])
        # Creates a unique ID for the chunk to prevent collisions.
        unique_id = f"{collection_name}_{chunk['metadata'].get('source_doc', 'doc')}_{i}"
        ids.append(unique_id)
        metadatas.append(chunk['metadata'])
    return documents, ids, metadatas