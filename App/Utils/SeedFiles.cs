using Microsoft.EntityFrameworkCore;
using Rift.App.Clients;
using Rift.Models;

public class SeedFiles
{
    private readonly ChromaDBClient _chromaDbClient;

    public SeedFiles(ChromaDBClient chromaDbClient)
    {
        _chromaDbClient = chromaDbClient;
    }

    public async Task SeedAsync(FileDbContext dbContext)
    {
        Console.WriteLine("Seeding files...");

        var numFiles = await dbContext.Files.CountAsync();

        if (numFiles > 0)
        {
            Console.WriteLine("Files already exist in the database. Skipping seeding.");
            return;
        }

        var collections = await _chromaDbClient.ListCollectionsAsync();

        Console.WriteLine($"Found {collections?.Count} collections in ChromaDB.");

        if (collections == null || collections.Count == 0)
        {
            Console.WriteLine("No collections found. Skipping file seeding.");
            return;
        }

        foreach (var collection in collections)
        {
            Console.WriteLine($"Processing collection: {collection.Name}");
            var documents = await _chromaDbClient.GetCollectionDocumentsAsync(collection.Name);
            if (documents == null)
            {
                Console.WriteLine($"No documents found in collection '{collection.Name}'.");
                continue;
            }
            Console.WriteLine($"Found {documents.Length} documents in collection '{collection.Name}'.");

            var names = new List<string>();
            var fileEntities = new List<FileEntity>();

            foreach (var document in documents)
            {
                var name = document.Metadata?.ContainsKey("name") == true ? document.Metadata["name"]?.ToString() : null;

                if (string.IsNullOrEmpty(name) || names.Contains(name))
                {
                    continue;
                }

                var sourceType = document.Metadata?.ContainsKey("source_type") == true ? document.Metadata["source_type"]?.ToString() : null;
                var source = document.Metadata?.ContainsKey("source") == true ? document.Metadata["source"]?.ToString() : string.Empty;

                names.Add(name);
                var fileEntity = new FileEntity
                {
                    Name = name,
                    UploadedBy = "system",
                    SourceLink = source ?? string.Empty,
                    SourceType = sourceType ?? string.Empty,
                };
                fileEntities.Add(fileEntity);
            }

            dbContext.Files.AddRange(fileEntities);
            await dbContext.SaveChangesAsync();
        }

        Console.WriteLine("File seeding completed.");
    }
}