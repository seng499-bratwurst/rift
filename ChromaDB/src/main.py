from fastapi import FastAPI
import chromadb
from chromadb.config import Settings
from pydantic import BaseModel

app = FastAPI()


chroma_client = chromadb.Client(Settings(persist_directory="/chroma_data"))
collection = chroma_client.create_collection(name="test_collection")

class AddRequest(BaseModel):
    id: str
    text: str

class QueryRequest(BaseModel):
    text: list[float]
    n_results: int = 5

@app.post("/add")
def add(req: AddRequest):
    collection.add(
        documents=[req.text],
        ids=[req.id],
    )
    return {"status": "added"}

@app.post("/query")
def query(req: QueryRequest):
    result = collection.query(
        query_texts=[req.text],
        n_results=req.n_results
    )
    return result