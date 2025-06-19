from .base_document_processor import BaseDocumentProcessor
from typing import List, Dict
import re
from datetime import datetime


class ResearchPapers(BaseDocumentProcessor):
    def __init__(self, docs: List[Dict]):
        super().__init__(docs)
        
    def clean(self) -> List[str]:
        return [re.sub(r"\\[a-zA-Z]+", "", doc['content']) for doc in self.docs]

    def create_metadata(self) -> List[Dict]:
        metadata = []
        created_at = datetime.utcnow().isoformat() + 'Z'
        for doc in self.docs:
            source = self._extract_source(doc['content'])
            title = self._extract_title(doc['content'], source)
            metadata.append({
                'source_type': 'paper',
                'length': len(doc['content']),
                'title': title,
                'source': source,
                'name': self._extract_id(doc['content']),
                'source_doc': doc['filename'].replace('.md', ''),
                'created_at': created_at
            })
        return metadata

    def chunk_with_metadata(self, max_tokens: int = 500, overlap: int = 50) -> List[Dict]:
        cleaned_docs = self.clean()
        metadata = self.create_metadata()
        return self._section_chunking(cleaned_docs, metadata, max_tokens, overlap)

    def _extract_title(self, text: str, source: str) -> str:
        for line in text.splitlines():
            if line.strip() and not line.startswith("#"):
                return line.strip()[:100]
        return source[:80] if source != 'Unknown' else "Untitled"