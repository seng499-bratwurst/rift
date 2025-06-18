import re
import json
from typing import List, Dict
from .base_document_processor import BaseDocumentProcessor
import os
import sys
from pathlib import Path

class ConfluenceDocuments(BaseDocumentProcessor):
    def __init__(self, docs: List[Dict]):
        super().__init__(docs)
        
    def chunk_with_metadata(self) -> List[Dict]:
        """
        Process documents and create chunks with metadata.
        Each chunk will contain relevant information from the JSON structure.
        """
        chunks = []
        
        for doc in self.docs:
            try:
                # Parse the JSON content
                json_data = json.loads(doc['content'])
                source_file = doc['filename']
                
                # Handle both array and single object cases
                if isinstance(json_data, list):
                    for item in json_data:
                        chunks.extend(self._process_item(item, source_file))
                else:
                    chunks.extend(self._process_item(json_data, source_file))
                    
            except json.JSONDecodeError as e:
                print(f"Error parsing JSON: {e}")
                continue
                
        return chunks
    
    def _process_item(self, item: Dict, source_file: str) -> List[Dict]:
        """
        Process a single JSON item and create chunks with metadata.
        """
        chunks = []
        
        metadata = {
            'type': 'confluence_json',
            'source_doc': source_file.replace('.json', '')
        }
        
        if 'propertyCode' in item:
            metadata['code'] = item['propertyCode']
            metadata['name'] = item.get('propertyName', '')
        elif 'deviceCode' in item:
            metadata['code'] = item['deviceCode']
            metadata['name'] = item.get('deviceName', '')
        elif 'deviceCategoryCode' in item:
            metadata['code'] = item['deviceCategoryCode']
            metadata['name'] = item.get('deviceCategoryName', '')
        elif 'locationCode' in item:
            metadata['code'] = item['locationCode']
            metadata['name'] = item.get('locationName', '')
        elif 'dataProductCode' in item:
            metadata['code'] = item['dataProductCode']
            metadata['name'] = item.get('dataProductName', '')
        
        text_parts = []
        
        for key, value in item.items():
            if isinstance(value, dict):
                for nested_key, nested_value in value.items():
                    if isinstance(nested_value, list):
                        text_parts.append(f"{nested_key}: {', '.join(str(v) for v in nested_value)}")
                    else:
                        text_parts.append(f"{nested_key}: {nested_value}")
            elif isinstance(value, list):
                if value:
                    text_parts.append(f"{key}: {', '.join(str(v) for v in value)}")
            else:
                text_parts.append(f"{key}: {value}")
        
        text = "\n".join(text_parts)
        
        if text.strip():
            chunks.append({
                'text': text,
                'metadata': metadata
            })
            
        return chunks