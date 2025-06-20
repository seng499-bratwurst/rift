# RAG Data Processing Scripts

This directory contains scripts for processing and preparing data for the RAG (Retrieval-Augmented Generation) system.

## Directory Structure

```
scripts/
├── processors/           # Document processor implementations
│   ├── __init__.py      # Package exports
│   ├── base_document_processor.py
│   ├── confluence_json.py
│   ├── research_papers.py
│   └── cambridge_bay_articles.py
├── rag_data_processing.py  # Main processing script
└── README.md           # This file
```

## Components

- `processors/`: Contains different document processors for various data sources
  - `base_document_processor.py`: Base class for document processing
  - `confluence_json.py`: Processor for JSON files of Confluence API reference wiki pages
  - `research_papers.py`: Processor for research papers
  - `cambridge_bay_articles.py`: Processor for Cambridge Bay articles

- `rag_data_processing.py`: Main script that orchestrates the processing of document types

## Usage

To process documents by type:

```python
from rag_data_processing import process_documents_by_doc_type

# Supported doc_types:
#   - "cambridge_bay_papers"
#   - "cambridge_bay_web_articles"
#   - "confluence_json"

# Example: Process all Cambridge Bay papers
chunks_by_file = process_documents_by_doc_type("cambridge_bay_papers")

# Each element in chunks_by_file is a list of chunk dicts for one file.
# Each chunk contains:
#   - 'text': The processed text content
#   - 'metadata': Associated metadata including source type, section, and chunk index
```

- The function returns a list where each element is a list of chunks for one file.
- See the docstring in `rag_data_processing.py` for more details. 