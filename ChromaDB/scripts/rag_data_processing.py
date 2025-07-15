import os
from pathlib import Path
from typing import List

from processors import ResearchPapers, ConfluenceJson, CambridgeBayArticles

DATA_DIR = Path(__file__).resolve().parents[2] / "Dataset" / "Markdown"
SUPPORTED_TYPES = {
    "cambridge_bay_papers": ResearchPapers,
    "cambridge_bay_web_articles": CambridgeBayArticles,
    "confluence_json": ConfluenceJson
}
ALLOWED_TYPES = {
    "application/pdf": ".pdf", "text/plain": ".txt",
    "application/json": ".json", "text/markdown": ".md"
}
ALLOWED_EXTENSIONS = set(ALLOWED_TYPES.values())


def process_documents_by_doc_type(doc_type: str) -> List[List[dict]]:
    """
    Process documents into chunks within a specific document type directory name.
    Returns a list where each element is a list of chunks for one file.
    The length of the returned list equals the number of files in the directory.
    """
    if doc_type not in SUPPORTED_TYPES:
        raise ValueError(f"Unsupported doc_type: {doc_type}. Supported types: {list(SUPPORTED_TYPES.keys())}")

    handler_cls = SUPPORTED_TYPES[doc_type]
    
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
        print(f"An error occurred during processing: {e}")