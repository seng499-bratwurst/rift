from fastapi import FastAPI
from pydantic import BaseModel
from typing import List
from transformers import AutoTokenizer, AutoModelForSequenceClassification
import torch
import uvicorn

app = FastAPI()

tokenizer = AutoTokenizer.from_pretrained("BAAI/bge-reranker-base")
model = AutoModelForSequenceClassification.from_pretrained("BAAI/bge-reranker-base")
model.eval()

class RerankRequest(BaseModel):
    query: str
    docs: List[str]

@app.post("/rerank")
async def rerank(request: RerankRequest):
    query = request.query
    docs = request.docs

    pairs = [f"{query} [SEP] {doc}" for doc in docs]
    inputs = tokenizer(pairs, return_tensors='pt', padding=True, truncation=True)
    with torch.no_grad():
        scores = model(**inputs).logits.squeeze(-1).tolist()

    reranked = sorted(zip(docs, scores), key=lambda x: x[1], reverse=True)
    return {"reranked_docs": [doc for doc, score in reranked]}

@app.get("/rerank")
async def test():
    return {"message": "Please use POST method with JSON body containing 'query' and 'docs'."}

if __name__ == "__main__":
    uvicorn.run(
        "reRank:app",
        host="0.0.0.0",
        port=6000,
        reload=True,
        log_level="info"
    )
