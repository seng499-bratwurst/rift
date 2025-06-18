#!/usr/bin/env python3
"""
Data Population Script for Oceanographic RAG System

This script helps populate the ChromaDB vector database with oceanographic data
from various sources including ONC API data, sample datasets, and manual entries.
"""

import asyncio
import json
import logging
import os
import sys
from datetime import datetime, timedelta, timezone
from pathlib import Path
from typing import Dict, List, Any, Optional

import requests
import pandas as pd
from sentence_transformers import SentenceTransformer

# Setup logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s'
)
logger = logging.getLogger(__name__)

# Configuration
CHROMADB_URL = os.getenv('CHROMADB_URL', 'http://localhost:8000')
CHUNK_SIZE = 512  # tokens per chunk
OVERLAP_SIZE = 50  # token overlap between chunks
BATCH_SIZE = 50   # documents per batch
DEFAULT_COLLECTION = "oceanographic_data"

class OceanographicDataPopulator:
    """Main class for populating ChromaDB with oceanographic data."""
    
    def __init__(self, chromadb_url: str = CHROMADB_URL):
        self.chromadb_url = chromadb_url.rstrip('/')
        self.session = requests.Session()
        self.session.headers.update({'Content-Type': 'application/json'})
        
    def _make_request(self, method: str, endpoint: str, **kwargs) -> requests.Response:
        """Make HTTP request to ChromaDB API."""
        url = f"{self.chromadb_url}{endpoint}"
        response = self.session.request(method, url, **kwargs)
        response.raise_for_status()
        return response
    
    def create_collection(self, collection_name: str, description: str = None) -> bool:
        """Create a new collection in ChromaDB."""
        try:
            data = {
                "name": collection_name,
                "description": description or f"Collection for {collection_name} data",
                "metadata": {
                    "created_by": "populate_data_script",
                    "created_at": datetime.now(timezone.utc).isoformat(),
                    "data_source": "oceanographic"
                }
            }
            
            response = self._make_request('POST', '/collections', json=data)
            logger.info(f"Created collection: {collection_name}")
            return True
            
        except requests.exceptions.HTTPError as e:
            if e.response.status_code == 400:
                logger.warning(f"Collection {collection_name} already exists")
                return True
            logger.error(f"Failed to create collection {collection_name}: {e}")
            return False
        except Exception as e:
            logger.error(f"Error creating collection {collection_name}: {e}")
            return False
    
    def add_document(self, doc_id: str, text: str, metadata: Dict[str, Any], 
                    collection_name: str = DEFAULT_COLLECTION) -> bool:
        """Add a single document to ChromaDB."""
        try:
            data = {
                "id": doc_id,
                "text": text,
                "metadata": metadata,
                "collection_name": collection_name
            }
            
            response = self._make_request('POST', '/documents/add', json=data)
            return True
            
        except Exception as e:
            logger.error(f"Failed to add document {doc_id}: {e}")
            return False
    
    def add_documents_batch(self, documents: List[Dict], collection_name: str = DEFAULT_COLLECTION) -> bool:
        """Add multiple documents in batch."""
        try:
            # Convert to expected format
            batch_docs = []
            for doc in documents:
                metadata = doc.get("metadata", {}).copy()
                
                # Convert any list values to comma-separated strings
                for key, value in metadata.items():
                    if isinstance(value, list):
                        metadata[key] = ",".join(str(item) for item in value)
                
                batch_docs.append({
                    "id": doc["id"],
                    "text": doc["text"],
                    "metadata": metadata
                })
            
            data = {
                "documents": batch_docs,
                "collection_name": collection_name
            }
            
            response = self._make_request('POST', '/documents/batch', json=data)
            logger.info(f"Added batch of {len(documents)} documents to {collection_name}")
            return True
            
        except Exception as e:
            logger.error(f"Failed to add document batch: {e}")
            return False
    
    def chunk_text(self, text: str, chunk_size: int = CHUNK_SIZE) -> List[str]:
        """Split text into chunks for better embedding."""
        # Simple word-based chunking (can be enhanced with proper tokenization)
        words = text.split()
        chunks = []
        
        for i in range(0, len(words), chunk_size - OVERLAP_SIZE):
            chunk_words = words[i:i + chunk_size]
            chunks.append(' '.join(chunk_words))
            
            if len(chunk_words) < chunk_size:
                break
                
        return chunks if chunks else [text]
    
    def populate_sample_oceanographic_data(self):
        """Populate with sample oceanographic data."""
        logger.info("Populating sample oceanographic data...")
        
        # Ensure collection exists
        self.create_collection(DEFAULT_COLLECTION, "Sample oceanographic data from various sources")
        
        # Sample data representing different types of oceanographic information
        sample_data = [
            {
                "id": "location_folger_passage",
                "text": """Folger Passage is a narrow waterway located in the Pacific Northwest, connecting the Strait of Georgia with the waters around the Gulf Islands. The passage experiences strong tidal currents reaching up to 8 knots during peak flow. Water depths range from 20 to 80 meters, with the deepest sections near the center channel. The area is characterized by rocky substrate with patches of sandy bottom in deeper areas. Marine life includes various species of rockfish, lingcod, and seasonal salmon runs. The passage serves as an important navigation route for both commercial and recreational vessels.""",
                "metadata": {
                    "source": "geographic_database",
                    "data_type": "location_info",
                    "location": "Folger Passage",
                    "timestamp": "2024-01-15T00:00:00Z",
                    "tags": "navigation,tidal_currents,marine_life,bathymetry"
                }
            },
            {
                "id": "instrument_ctd_basic",
                "text": """CTD (Conductivity, Temperature, Depth) instruments are fundamental tools in oceanographic research. These devices measure the electrical conductivity of seawater (which relates to salinity), water temperature, and pressure (which determines depth). Modern CTD systems can sample at rates up to 24 Hz and provide highly accurate measurements with temperature precision of ±0.001°C and conductivity precision of ±0.0003 S/m. CTD data is essential for understanding water mass properties, ocean circulation patterns, and marine ecosystem dynamics.""",
                "metadata": {
                    "source": "instrument_manual",
                    "data_type": "instrument_spec",
                    "instrument_type": "CTD",
                    "timestamp": "2024-01-10T00:00:00Z",
                    "tags": "conductivity,temperature,depth,salinity,oceanography"
                }
            },
            {
                "id": "sensor_data_temp_series",
                "text": """Temperature sensor data from Station VENUS, Saanich Inlet. Measurements show typical seasonal variation with summer surface temperatures reaching 15-18°C and winter temperatures dropping to 8-10°C. At depth (100m), temperatures remain more stable around 8-9°C year-round. Data quality is high with 98.5% uptime over the monitoring period. Sensors are calibrated monthly and show excellent agreement with CTD casts. Notable temperature anomalies were observed during the marine heatwave events, with surface temperatures exceeding 20°C for extended periods.""",
                "metadata": {
                    "source": "ONC_VENUS",
                    "data_type": "sensor_data",
                    "location": "Saanich Inlet",
                    "depth": 15.0,
                    "instrument_type": "temperature_sensor",
                    "timestamp": "2024-01-20T12:00:00Z",
                    "tags": "temperature,time_series,seasonal_variation,marine_heatwave"
                }
            },
            {
                "id": "research_oxygen_depletion",
                "text": """Oxygen depletion in coastal waters is an increasing concern due to climate change and anthropogenic impacts. Research in the Strait of Georgia shows declining oxygen levels at depth, particularly in fjords and inlets with restricted circulation. Dissolved oxygen concentrations below 2 mg/L create hypoxic conditions that stress marine organisms. Long-term monitoring indicates a trend of decreasing oxygen at rates of 0.1-0.3 mg/L per decade. This research combines sensor data, water sampling, and biogeochemical modeling to understand the drivers and impacts of deoxygenation.""",
                "metadata": {
                    "source": "research_publication",
                    "data_type": "research_data",
                    "location": "Strait of Georgia",
                    "timestamp": "2024-01-25T00:00:00Z",
                    "tags": "oxygen,hypoxia,climate_change,biogeochemistry,monitoring"
                }
            },
            {
                "id": "tsunami_warning_system",
                "text": """The Pacific Tsunami Warning System utilizes a network of deep-ocean pressure sensors (DART buoys) and coastal sea level stations to detect and track tsunamis. When seismic activity occurs, the system analyzes real-time data to determine if a tsunami has been generated. Detection relies on characteristic wave patterns and timing relationships between sensors. The Canadian component includes 5 DART stations and 15 coastal monitoring sites. Response times for local tsunamis must be under 5 minutes for effective warning dissemination to coastal communities.""",
                "metadata": {
                    "source": "emergency_management",
                    "data_type": "monitoring_system",
                    "location": "Pacific Ocean",
                    "instrument_type": "DART_buoy",
                    "timestamp": "2024-01-18T00:00:00Z",
                    "tags": "tsunami,warning_system,DART,seismic,emergency_response"
                }
            },
            {
                "id": "marine_protected_area",
                "text": """Marine Protected Areas (MPAs) in British Columbia waters serve to conserve critical habitats and support sustainable fisheries. The Rockfish Conservation Areas protect essential rockfish habitat from fishing impacts, particularly in areas with slow-growing, long-lived species. Monitoring within MPAs includes underwater video surveys, fish tagging studies, and habitat mapping. Effectiveness is measured through fish abundance, size distribution, and habitat quality indicators. Compliance monitoring uses vessel tracking and enforcement patrols to ensure fishing restrictions are observed.""",
                "metadata": {
                    "source": "conservation_database",
                    "data_type": "conservation_info",
                    "location": "British Columbia",
                    "timestamp": "2024-01-12T00:00:00Z",
                    "tags": "MPA,conservation,rockfish,fisheries,habitat_protection"
                }
            },
            {
                "id": "acoustic_monitoring_whales",
                "text": """Passive acoustic monitoring systems detect and classify marine mammal vocalizations to study behavior, migration patterns, and population dynamics. Hydrophone arrays in the Salish Sea continuously record underwater sounds, automatically detecting whale calls using machine learning algorithms. Killer whale echolocation clicks, humpback whale songs, and harbor porpoise clicks are identified and catalogued. This data helps assess the impacts of vessel noise on marine mammals and informs conservation strategies. Real-time detection capabilities support ship strike avoidance measures.""",
                "metadata": {
                    "source": "marine_mammal_research",
                    "data_type": "acoustic_data",
                    "location": "Salish Sea",
                    "instrument_type": "hydrophone",
                    "timestamp": "2024-01-22T00:00:00Z",
                    "tags": "marine_mammals,acoustics,whales,hydrophone,conservation"
                }
            },
            {
                "id": "ocean_acidification_study",
                "text": """Ocean acidification monitoring in Pacific Northwest waters reveals significant pH declines over the past decades. Surface water pH has decreased by 0.1 units since pre-industrial times, with seasonal variations driven by upwelling events and biological productivity. Carbonate chemistry measurements include pH, dissolved inorganic carbon, and aragonite saturation state. Laboratory studies demonstrate impacts on shell-forming organisms including oysters, mussels, and pteropods. Coastal monitoring stations provide high-frequency data to capture both long-term trends and short-term variability in ocean chemistry.""",
                "metadata": {
                    "source": "climate_research",
                    "data_type": "research_data",
                    "location": "Pacific Northwest",
                    "timestamp": "2024-01-28T00:00:00Z",
                    "tags": "ocean_acidification,pH,carbonate_chemistry,climate_change,shellfish"
                }
            }
        ]
        
        # Process and add documents
        processed_docs = []
        for doc in sample_data:
            # Chunk long texts if necessary
            text_chunks = self.chunk_text(doc["text"])
            
            for i, chunk in enumerate(text_chunks):
                chunk_id = f"{doc['id']}_chunk_{i}" if len(text_chunks) > 1 else doc["id"]
                
                # Add chunk information to metadata
                chunk_metadata = doc["metadata"].copy()
                if len(text_chunks) > 1:
                    chunk_metadata["chunk_index"] = i
                    chunk_metadata["total_chunks"] = len(text_chunks)
                    chunk_metadata["original_id"] = doc["id"]
                
                processed_docs.append({
                    "id": chunk_id,
                    "text": chunk,
                    "metadata": chunk_metadata
                })
        
        # Add documents in batches
        for i in range(0, len(processed_docs), BATCH_SIZE):
            batch = processed_docs[i:i + BATCH_SIZE]
            self.add_documents_batch(batch, DEFAULT_COLLECTION)
            
        logger.info(f"Successfully populated {len(processed_docs)} document chunks")
    
    def populate_from_csv(self, csv_path: str, collection_name: str = DEFAULT_COLLECTION):
        """Populate data from a CSV file."""
        logger.info(f"Populating data from CSV: {csv_path}")
        
        try:
            df = pd.read_csv(csv_path)
            
            # Ensure collection exists
            self.create_collection(collection_name, f"Data from {Path(csv_path).name}")
            
            processed_docs = []
            for idx, row in df.iterrows():
                # Construct text from relevant columns
                text_parts = []
                metadata = {}
                
                for col, value in row.items():
                    if pd.isna(value):
                        continue
                        
                    # Metadata fields (customize based on your CSV structure)
                    if col.lower() in ['id', 'timestamp', 'location', 'depth', 'instrument_type', 'data_type', 'source']:
                        metadata[col.lower()] = value
                    else:
                        # Include in text content
                        text_parts.append(f"{col}: {value}")
                
                if not text_parts:
                    continue
                    
                doc_id = metadata.get('id', f"csv_doc_{idx}")
                text = ". ".join(text_parts)
                
                # Set default metadata values
                metadata.setdefault('source', f"csv_{Path(csv_path).stem}")
                metadata.setdefault('data_type', 'imported_data')
                metadata.setdefault('timestamp', datetime.utcnow().isoformat())
                
                processed_docs.append({
                    "id": str(doc_id),
                    "text": text,
                    "metadata": metadata
                })
            
            # Add documents in batches
            for i in range(0, len(processed_docs), BATCH_SIZE):
                batch = processed_docs[i:i + BATCH_SIZE]
                self.add_documents_batch(batch, collection_name)
                
            logger.info(f"Successfully imported {len(processed_docs)} documents from CSV")
            
        except Exception as e:
            logger.error(f"Failed to import CSV data: {e}")
    
    def populate_from_json(self, json_path: str, collection_name: str = DEFAULT_COLLECTION):
        """Populate data from a JSON file."""
        logger.info(f"Populating data from JSON: {json_path}")
        
        try:
            with open(json_path, 'r') as f:
                data = json.load(f)
            
            # Ensure collection exists
            self.create_collection(collection_name, f"Data from {Path(json_path).name}")
            
            processed_docs = []
            
            # Handle different JSON structures
            if isinstance(data, list):
                documents = data
            elif isinstance(data, dict) and 'documents' in data:
                documents = data['documents']
            else:
                logger.error("Unsupported JSON structure")
                return
            
            for idx, doc in enumerate(documents):
                if not isinstance(doc, dict):
                    continue
                    
                doc_id = doc.get('id', f"json_doc_{idx}")
                text = doc.get('text', doc.get('content', ''))
                metadata = doc.get('metadata', {})
                
                if not text:
                    continue
                
                # Set default metadata values
                metadata.setdefault('source', f"json_{Path(json_path).stem}")
                metadata.setdefault('timestamp', datetime.utcnow().isoformat())
                
                processed_docs.append({
                    "id": str(doc_id),
                    "text": text,
                    "metadata": metadata
                })
            
            # Add documents in batches
            for i in range(0, len(processed_docs), BATCH_SIZE):
                batch = processed_docs[i:i + BATCH_SIZE]
                self.add_documents_batch(batch, collection_name)
                
            logger.info(f"Successfully imported {len(processed_docs)} documents from JSON")
            
        except Exception as e:
            logger.error(f"Failed to import JSON data: {e}")
    
    def get_collection_stats(self, collection_name: str = DEFAULT_COLLECTION):
        """Get statistics for a collection."""
        try:
            response = self._make_request('GET', f'/collections/{collection_name}')
            collection_info = response.json()
            
            logger.info(f"Collection: {collection_name}")
            logger.info(f"Document count: {collection_info.get('document_count', 'Unknown')}")
            logger.info(f"Metadata: {collection_info.get('metadata', {})}")
            
            return collection_info
            
        except Exception as e:
            logger.error(f"Failed to get collection stats: {e}")
            return None
    
    def test_query(self, query: str, collection_name: str = DEFAULT_COLLECTION, n_results: int = 3):
        """Test a query against the populated data."""
        logger.info(f"Testing query: '{query}'")
        
        try:
            data = {
                "text": query,
                "n_results": n_results,
                "collection_name": collection_name
            }
            
            response = self._make_request('POST', '/query', json=data)
            result = response.json()
            
            if 'results' in result and result['results']['documents']:
                documents = result['results']['documents'][0]
                distances = result['results']['distances'][0] if 'distances' in result['results'] else []
                
                logger.info(f"Found {len(documents)} results:")
                for i, (doc, dist) in enumerate(zip(documents, distances)):
                    # ChromaDB typically returns cosine distance, where smaller is better
                    # Convert to similarity score (higher is better)
                    if distances and isinstance(dist, (int, float)):
                        # For cosine distance: similarity = 1 - distance
                        # Clamp to [0, 1] range for display
                        similarity = max(0, min(1, 1 - dist))
                        similarity_str = f"{similarity:.3f}"
                    else:
                        similarity_str = "N/A"
                    
                    dist_str = f"{dist:.3f}" if isinstance(dist, (int, float)) else str(dist)
                    logger.info(f"  {i+1}. Similarity: {similarity_str} (Distance: {dist_str})")
                    logger.info(f"     Text: {doc[:200]}...")
            else:
                logger.info("No results found")
                
        except Exception as e:
            logger.error(f"Query test failed: {e}")


def main():
    """Main function to run the data population script."""
    import argparse
    
    parser = argparse.ArgumentParser(description="Populate ChromaDB with oceanographic data")
    parser.add_argument('--chromadb-url', default=CHROMADB_URL, help='ChromaDB URL')
    parser.add_argument('--action', choices=['sample', 'csv', 'json', 'stats', 'test'], 
                       default='sample', help='Action to perform')
    parser.add_argument('--file', help='File path for CSV or JSON import')
    parser.add_argument('--collection', default=DEFAULT_COLLECTION, help='Collection name')
    parser.add_argument('--query', help='Test query string')
    
    args = parser.parse_args()
    
    populator = OceanographicDataPopulator(args.chromadb_url)
    
    if args.action == 'sample':
        populator.populate_sample_oceanographic_data()
    elif args.action == 'csv':
        if not args.file:
            logger.error("CSV file path required for CSV import")
            sys.exit(1)
        populator.populate_from_csv(args.file, args.collection)
    elif args.action == 'json':
        if not args.file:
            logger.error("JSON file path required for JSON import")
            sys.exit(1)
        populator.populate_from_json(args.file, args.collection)
    elif args.action == 'stats':
        populator.get_collection_stats(args.collection)
    elif args.action == 'test':
        if not args.query:
            logger.error("Query string required for testing")
            sys.exit(1)
        populator.test_query(args.query, args.collection)
    
    logger.info("Script completed successfully")


if __name__ == "__main__":
    main()