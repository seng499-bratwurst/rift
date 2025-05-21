# rift
Backend for Bratwurst project written in C# using the .NET framework.

## Getting Started

To get started with developing the Rift project, install the [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download).

For making and running DB migrations install [EF Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools)

Once installed, to run the api and database:

* Navigate to the `/App` subdirectory
* Run `docker compose up --build` in the shell of your choice

## Creating and Running Migrations

1. Make changes to model classes (/App/Models)
2. run `dotnet ef migration add <migration-name>`
3. run `dotnet ef database update`

## Contributions

We will be using the [Microsoft .NET Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/). A linter will be set up soon, but until then I trust y'all to self-police.
