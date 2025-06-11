# rift
Backend for Bratwurst project written in C# using the .NET framework.

## Getting Started

To get started with developing the Rift project, install the [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download).

For making and running DB migrations install [EF Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools)

Once installed, to run the api and database:

* Navigate to the `/App` subdirectory
* Run `docker compose up --build` in the shell of your choice
* Once running, view the current endpoints [Here](http://localhost:5000/swagger/index.html)


## Creating and Running Migrations

1. Navigate to the `App` subdirectory
2. run `dotnet ef database update`

## ChromaDB

The [ChromaDB](https://docs.trychroma.com/docs/overview/introduction) stores the document embeddings and is accessed though a small Python API found in the `/ChromaDB` subdirectory.

For contributing to the Python API you will need to complete the following steps:

* **(Optional)** Setup a Python virtual environment by running `python3 -m venv .venv` in the root directory of the project. Don't forget to activate the venv when working on the API!
* Install the required packages by running `pip install -r requirements.txt` in the `/ChromaDB` subdirectory



## Contributions

We will be using the [Microsoft .NET Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/). A linter will be set up soon, but until then I trust y'all to self-police.
