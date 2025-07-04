FROM python:3.12-slim

WORKDIR /src

COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt

COPY . .

EXPOSE 6000

CMD ["uvicorn", "reRank:app", "--host", "0.0.0.0", "--port", "6000"]