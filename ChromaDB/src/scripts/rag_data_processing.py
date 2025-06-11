import os
from pathlib import Path
from typing import List, Dict

from research_papers import ResearchPapers
from confluence_documents import ConfluenceDocuments
from cambridge_bay_articles import CambridgeBayArticles

DATA_DIR = Path(__file__).resolve().parents[3] / "Dataset" / "Markdown"
SUPPORTED_TYPES = {
    "cambridge_bay_papers": ("paper", ResearchPapers),
    "cambridge_bay_web_articles": ("web_article", CambridgeBayArticles)
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

def process_data_by_type(type_dir: str, source_type: str, handler_cls) -> List[Dict]:
    """
    Process all data of one type using the appropriate handler.
    Returns a list of dicts with 'text' and 'metadata'.
    """
    full_path = Path(os.path.join(DATA_DIR, type_dir))
    raw_docs = load_markdown_files_from_dir(full_path)
    processor = handler_cls(raw_docs)
    chunks_with_meta = processor.chunk_with_metadata()

    processed_chunks = []
    for chunk_data in chunks_with_meta:
        chunk_meta = chunk_data['metadata'].copy()
        chunk_meta['source_type'] = source_type

        processed_chunks.append({
            'text': chunk_data['text'],
            'metadata': chunk_meta
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
    # Testing purposes only
    try:
        all_docs = process_all_documents()

        print(f"Processed {len(all_docs)} chunks across all data types.")
        if all_docs:
            print("\n", all_docs[0]['text'])
            print("\n", all_docs[0]['metadata'], "\n")
    except Exception as e:
        print(f"An error occurred during processing: {e}")