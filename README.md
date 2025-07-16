# rift
Backend for Bratwurst project written in C# using the .NET framework.

## Getting Started

To get started with developing the Rift project, install the [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download).

For making and running DB migrations install [EF Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools).

Once installed, to run the api and database:

* Navigate to the `/App` subdirectory
* Run `docker compose up --build` in the shell of your choice
* Once running, view the current endpoints [Here](http://localhost:5000/swagger/index.html)

**Tokens**
For the LLMs to work correctly you must generate and add several API tokens from our providers. In an effort to mitigate total cost, currently we are having developers working locally generate their own tokens for personal use. All of these tokens are free or have free quantities and do not require any financial information, only the creation of a free account.

* For the LLM's to work correctly you must generate an API token to use from [Google Gemma](https://aistudio.google.com/apikey).
  * Copy the generated API token and add the line below to your `.env` file.
  * `GOOGLE_API_KEY=<your-api-token>`
* Similarly, generate an API token to use from [Hugging Face](https://huggingface.co/docs/hub/en/security-tokens) and add it to your `.env` file.
  * `HF_API_KEY=<your-api-token>`
* Similarly, generate an API token to use from [Together AI](https://www.together.ai) and add it to your `.env` file.
  * `TOGETHER_API_KEY=<your-api-token>`
* You must also generate an API token to use from the [Ocean Networks Canada Data Portal](https://data.oceannetworks.ca) and add it to your `.env` file.
  * `ONC_TOKEN=<your-api-token>`
* **Note:** You will have to re-run `docker compose up --build` for the environment variables to take effect.

**(Optional)**
* If you would like to populate the VectorDB with the document dataset, open the ChromaDB endpoints [here](http://localhost:8000/docs#/default/add_initial_documents_documents_initial_post) (once the backend is running).
* You can run the `POST /documents/initial` endpoint to add all of the dataset to the VectorDB.
* If you want to add a few test documents, the `POST /documents/add` endpoint can be used.
* To clear the VectorDB run `docker volume rm rift_chroma_data`.


## Migrations

1. Migrations run automatically upon app startup.
2. If you need to generate new migrations `dotnet ef migrations add <MigrationName>`.
3. If you need to run them manually run `dotnet ef database update` in the 'App' directory.

## ChromaDB

The [ChromaDB](https://docs.trychroma.com/docs/overview/introduction) stores the document embeddings and is accessed though a small Python API found in the `/ChromaDB` subdirectory.

For contributing to the Python API you will need to complete the following steps:

* **(Optional)** Setup a Python virtual environment by running `python3 -m venv .venv` in the root directory of the project. Don't forget to activate the venv when working on the API!
* Install the required packages by running `pip install -r requirements.txt` in the `/ChromaDB` subdirectory.

More information on the ChromaDB configuration can be found [Here](ChromaDB/config/chroma_config.md)



## Contributions

We will be using the [Microsoft .NET Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/). A linter will be set up soon, but until then I trust y'all to self-police.
