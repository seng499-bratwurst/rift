import os
from pathlib import Path
from typing import List, Dict

from research_papers import ResearchPapers
from confluence_documents import ConfluenceDocuments
from cambridge_bay_articles import CambridgeBayArticles

DATA_DIR = Path(__file__).resolve().parents[3] / "Dataset" / "Markdown"
SUPPORTED_TYPES = {
    "cambridge_bay_papers": ("papers", ResearchPapers)
}

def load_markdown_files_from_dir(directory: Path) -> List[str]:
    """
    Load all markdown files from a given directory.
    Returns a list of file contents as strings.
    """
    if not directory.exists():
        raise FileNotFoundError(f"Directory not found: {directory}")
    
    documents = []
    for file_path in sorted(directory.glob("*.md")):
        try:
            with open(file_path, "r", encoding="utf-8") as file:
                documents.append(file.read())
        except Exception as e:
            print(f"Error reading file {file_path}: {e}")
    return documents

def chunk_text(text: str, max_length: int = 500) -> List[str]:
    """
    Naive chunking: paragraph-based, keeping chunks under `max_length` characters.
    """
    if not text.strip():
        return []

    paragraphs = text.split("\n\n")
    chunks = []
    current_chunk = ""
    for para in paragraphs:
        if len(current_chunk) + len(para) < max_length:
            current_chunk += para + "\n\n"
        else:
            chunks.append(current_chunk.strip())
            current_chunk = para + "\n\n"
    if current_chunk:
        chunks.append(current_chunk.strip())
    return chunks

def process_data_by_type(type_dir: str, source_type: str, handler_cls) -> List[Dict]:
    """
    Process all data of one type using the appropriate handler.
    Returns a list of dicts with 'text' and 'metadata'.
    """
    full_path = Path(os.path.join(DATA_DIR, type_dir))
    raw_docs = load_markdown_files_from_dir(full_path)
    processor = handler_cls(raw_docs)
    cleaned_docs = processor.clean()
    metadata_list = processor.create_metadata()

    processed_chunks = []
    for doc, meta in zip(cleaned_docs, metadata_list):
        chunks = chunk_text(doc)
        for i, chunk in enumerate(chunks):
            chunk_meta = meta.copy()
            chunk_meta.update({
                "chunk_index": i,
                "source_type": source_type,
            })
            processed_chunks.append({
                "text": chunk,
                "metadata": chunk_meta
            })
    return processed_chunks

def process_all_documents() -> List[Dict]:
    """
    Process all documents across supported types.
    Returns a list of all chunks with associated metadata.
    """
    all_chunks = []
    for type_dir, (source_type, handler_cls) in SUPPORTED_TYPES.items():
        print(f"Processing {type_dir}...")
        try:
            chunks = process_data_by_type(type_dir, source_type, handler_cls)
            all_chunks.extend(chunks)
        except Exception as e:
            print(f"Error processing {type_dir}: {e}")
    return all_chunks

if __name__ == "__main__":
    try:
        all_docs = process_all_documents()

        print(f"Processed {len(all_docs)} chunks across all data types.")
        if all_docs:
            first_doc_metadata = all_docs[480]['metadata']
            print("\nSample first doc metadata:\n")
            print("Source type: ", first_doc_metadata['source_type'])
            print("length: ", first_doc_metadata['length'])
            print("Title: ", first_doc_metadata['title'])
            print("Source: ", first_doc_metadata['source'])
            print("ID: ", first_doc_metadata['id'])
            print("Chunk index: ", first_doc_metadata['chunk_index'], "\n")
            print("Chunk Text:\n", all_docs[1]['text'], "\n")
    except Exception as e:
        print(f"An error occurred during processing: {e}")