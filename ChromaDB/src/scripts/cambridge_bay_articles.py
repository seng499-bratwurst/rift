from base_document_processor import BaseDocumentProcessor
from typing import List, Dict
import re


class CambridgeBayArticles(BaseDocumentProcessor):
    def clean(self) -> List[str]:
        return [re.sub(r"\s+", " ", re.sub(r"<[^>]+>", "", doc)).strip() for doc in self.docs]

    def create_metadata(self) -> List[Dict]:
        metadata = []
        for doc in self.docs:
            source = self._extract_source(doc)
            title = self._extract_title_from_url(source)
            metadata.append({
                'source_type': 'web_article',
                'length': len(doc),
                'title': title,
                'source': source,
                'id': self._extract_id(doc)
            })
        return metadata

    def chunk_with_metadata(self, max_tokens: int = 400, overlap: int = 50) -> List[Dict]:
        cleaned_docs = self.clean()
        metadata = self.create_metadata()
        return self._section_chunking(cleaned_docs, metadata, max_tokens, overlap)

    def _extract_title_from_url(self, url: str) -> str:
        return url.rstrip('/').split('/')[-1] if url != 'Unknown' else 'Untitled'
