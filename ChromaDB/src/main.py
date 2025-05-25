import chromadb

chroma_client = chromadb.Client()

collection = chroma_client.create_collection(name="test_collection")

collection.upsert(
  documents=[
    "This is a document about water.",
    "This is a document about land.",
    "This is a document about coding.",
  ],
  ids=["id1", "id2", "id3"],
)

results = collection.query(
  query_texts=["This is a query about the Oceans."],
  n_results=3
)

print("Results:", results)
