# Embedding Structure for Collections and Chunks of Documents

This document describes the actual embedding structure for collections and chunks of documents in the Rift VectorDB (ChromaDB) system.

---

## Collection Structure

Collection metadata is a dictionary of key-value pairs. The following fields are set in the code:

| Field         | Type   | Set In           | Description                                                      |
|---------------|--------|------------------|------------------------------------------------------------------|
| description   | str    | main.py | Human-readable description of the collection                     |
| created_for   | str    | main.py          | Purpose or system for which the collection was created           |

**Example from code:**
```json
  {
    "name": "oceanographic_data",
    "id": "3b868bc0-f1b5-4c28-9453-573b07376102",
    "metadata": {
      "description": "Default collection for Ocean Networks Canada (ONC) data",
      "created_for": "rift_rag_system"
    }
  }
```

---

## Chunk Struture

Chunk metadata is a dictionary. There are multiple chunks per document. The following fields are set by the code (see `main.py`, `rag_data_processing.py`, and the processors): 

**Metadata fields for Research Paper Documents:**
| Field         | Type   | Set In                               | Description                                 |
|---------------|--------|--------------------------------------|---------------------------------------------|
| source_type   | str    | processors/research_papers.py        | Always set to 'paper'                       |
| length        | int    | processors/research_papers.py        | Length of the document content (characters) |
| title         | str    | processors/research_papers.py        | Title extracted from the content            |
| source        | str    | processors/research_papers.py        | Paper citation in APA format          |
| name          | str    | processors/research_papers.py        | Extracted from content        |
| source_doc    | str    | processors/research_papers.py        | Original filename without extension         |
| created_at    | str    | processors/research_papers.py        | ISO 8601 timestamp when processed           |

**Metadata fields for Articles:**
| Field         | Type   | Set In                                   | Description                                 |
|---------------|--------|------------------------------------------|---------------------------------------------|
| source_type   | str    | processors/cambridge_bay_articles.py     | Always set to 'web_article'                 |
| length        | int    | processors/cambridge_bay_articles.py     | Length of the document content (characters) |
| title         | str    | processors/cambridge_bay_articles.py     | Title extracted from the source URL         |
| source        | str    | processors/cambridge_bay_articles.py     | Article URL                    |
| name          | str    | processors/cambridge_bay_articles.py     | Extracted from content      |
| source_doc    | str    | processors/cambridge_bay_articles.py     | Original filename without extension         |
| created_at    | str    | processors/cambridge_bay_articles.py     | ISO 8601 timestamp when processed           |

**Metadata fields for Confluence JSON Documents:**
| Field         | Type   | Set In                               | Description                                                                                                      |
|---------------|--------|--------------------------------------|------------------------------------------------------------------------------------------------------------------|
| source_type   | str    | processors/confluence_json.py        | Always set to 'confluence_json'                                                                                  |
| name          | str    | processors/confluence_json.py        | Name of the source JSON file (without extension)                                                                 |
| created_at    | str    | processors/confluence_json.py        | ISO 8601 timestamp when processed                                                                                |
| chunk_index   | int    | processors/confluence_json.py        | Index of the chunk for the document                                                                              |
| item_code     | str    | processors/confluence_json.py        | Code for the item (from `propertyCode`, `deviceCode`, `deviceCategoryCode`, `locationCode`, or `dataProductCode`) |
| item_name     | str    | processors/confluence_json.py        | Name for the item (from `propertyName`, `deviceName`, `deviceCategoryName`, `locationName`, or `dataProductName`) |

**Example from code:**
```json
{
  "id": "locations_0",
  "document": "maxDepth: None\nmaxLat: 47.773125\nmaxLon: -53.999706\nminDepth: None\nminLat: 47.773125\nminLon: -53.999706\ndataSearchURL: https://data.oceannetworks.ca/DataSearch?location=AAAGV\ndeployments: 1\ndepth: None\ndescription:  Arnold Cove is located in Placentia Bay in Newfoundland.\nhasDeviceData: true\nhasPropertyData: false\nlat: 47.773125\nlocationCode: AAAGV\nlocationName: Arnold Cove\nlon: -53.999706",
  "metadata": {
    "source_type": "confluence_json",
    "name": "locations",
    "item_name": "Arnold Cove",
    "chunk_index": 0,
    "created_at": "2025-06-19T21:55:08.055108Z",
    "item_code": "AAAGV"
  }
}
```