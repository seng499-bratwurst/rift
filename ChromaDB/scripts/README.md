# RAG Data Processing Scripts

This directory contains scripts for processing and preparing data for the RAG (Retrieval-Augmented Generation) system.

## Directory Structure

```
scripts/
├── processors/           # Document processor implementations
│   ├── __init__.py      # Package exports
│   ├── base_document_processor.py
│   ├── confluence_documents.py
│   ├── research_papers.py
│   └── cambridge_bay_articles.py
├── rag_data_processing.py  # Main processing script
└── README.md           # This file
```

## Components

- `processors/`: Contains different document processors for various data sources
  - `base_document_processor.py`: Base class for document processing
  - `confluence_documents.py`: Processor for Confluence documents
  - `research_papers.py`: Processor for research papers
  - `cambridge_bay_articles.py`: Processor for Cambridge Bay articles

- `rag_data_processing.py`: Main script that orchestrates the processing of all document types

## Usage

To process all documents:

```python
from rag_data_processing import process_all_documents

# Process all documents
chunks = process_all_documents()
```

Each chunk contains:
- `text`: The processed text content
- `metadata`: Associated metadata including source type, section, and chunk index 