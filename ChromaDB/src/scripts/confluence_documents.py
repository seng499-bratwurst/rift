import re
from typing import List, Dict
from base_document_processor import BaseDocumentProcessor

class ConfluenceDocuments(BaseDocumentProcessor):
    def __init__(self, docs: List[str]):
        super().__init__(docs)

    def clean(self) -> List[str]:
        cleaned = []
        for doc in self.docs:
            text = re.sub(r'<[^>]+>', '', doc)
            text = re.sub(r'\r?\n', '\n', text).strip()
            cleaned.append(text)
        return cleaned

    def create_metadata(self) -> List[Dict]:
        metas = []
        for doc in self.docs:
            source = self._extract_source(doc)
            doc_id = self._extract_id(doc)
            description = self._extract_description(doc)
            metas.append({
                'source_type': 'wiki',
                'source': source,
                'id': doc_id,
                'description': description
            })
        return metas

    def chunk_with_metadata(self) -> List[Dict]:
        cleaned = self.clean()
        metas = self.create_metadata()
        all_chunks = []
        for doc_text, base_meta in zip(cleaned, metas):
            lines = doc_text.split('\n')
            header_idx = next((i for i,l in enumerate(lines) if l.strip().startswith('|') and '|' in l and not l.strip().startswith('|-')), None)
            if header_idx is None:
                continue
            header_line = lines[header_idx]
            cols = [c.strip() for c in header_line.strip('|').split('|')]
            row_start = header_idx + 2
            row_lines = [l for l in lines[row_start:] if l.strip().startswith('|')]
            for row in row_lines:
                parts = [c.strip() for c in row.strip('|').split('|')]
                if len(parts) < len(cols):
                    parts += [''] * (len(cols) - len(parts))
                row_data = dict(zip(cols, parts))
                text = ' '.join(f"{k}: {v}" for k,v in row_data.items() if v)
                if not text.strip():
                    continue
                meta = base_meta.copy()
                meta['row_data'] = row_data
                all_chunks.append({'text': text, 'metadata': meta})
        return all_chunks


    @staticmethod
    def _extract_description(doc: str) -> str:
        m = re.search(r'^# Description:\s*(.+)$', doc, re.MULTILINE)
        return m.group(1).strip() if m else ''
