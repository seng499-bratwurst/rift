FROM python:3.12-slim

WORKDIR /src

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt


COPY src/ /src/
COPY scripts/ /src/scripts/


ENV PYTHONPATH="/src:/src/scripts"

EXPOSE 8000

CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]