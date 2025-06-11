from base_document_processor import BaseDocumentProcessor
from typing import List, Dict
import re


class ResearchPapers(BaseDocumentProcessor):
    def clean(self) -> List[str]:
        return [re.sub(r"\\[a-zA-Z]+", "", doc) for doc in self.docs]

    def create_metadata(self) -> List[Dict]:
        metadata = []
        for doc in self.docs:
            source = self._extract_source(doc)
            title = self._extract_title(doc, source)
            metadata.append({
                'source_type': 'paper',
                'length': len(doc),
                'title': title,
                'source': source,
                'id': self._extract_id(doc)
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