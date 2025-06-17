import re
import tiktoken
from typing import List, Dict


class BaseDocumentProcessor:
    def __init__(self, docs: List[str]):
        self.docs = docs

    def _section_chunking(self, texts: List[str], metas: List[Dict], max_tokens: int, overlap: int) -> List[Dict]:
        chunks = []
        for text, base_meta in zip(texts, metas):
            sections = re.split(r'(?m)^##\s+', text)
            headings = re.findall(r'(?m)^##\s+(.+)', text)
            titles = ["Introduction"] + headings
            for title, section in zip(titles, sections):
                for i, chunk in enumerate(self._chunk_text(section, max_tokens, overlap)):
                    meta = base_meta.copy()
                    meta.update({'section': title, 'chunk_index': i})
                    chunks.append({'text': chunk, 'metadata': meta})
        return chunks

    @staticmethod
    def _chunk_text(text: str, max_tokens: int, overlap: int) -> List[str]:
        encoder = tiktoken.get_encoding("cl100k_base")
        tokens = encoder.encode(text)
        chunks = []
        start = 0
        while start < len(tokens):
            chunk = encoder.decode(tokens[start:start + max_tokens])
            chunks.append(chunk)
            start += max_tokens - overlap
        return chunks

    @staticmethod
    def _extract_source(text: str) -> str:
        match = re.search(r'^# Source:\s*(.+)', text, re.MULTILINE)
        return match.group(1).strip() if match else 'Unknown'

    @staticmethod
    def _extract_id(text: str) -> str:
        match = re.search(r'^# ID:\s*(.+)', text, re.MULTILINE)
        return match.group(1).strip() if match else 'Unknown'
