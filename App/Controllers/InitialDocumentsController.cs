using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Rift.Models;
using Rift.App.Clients;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api")]
public class InitialDocumentsController : ControllerBase
{
    private readonly ChromaDBClient _chromaDbClient;
    private readonly FileDbContext _dbContext;


    public InitialDocumentsController(ChromaDBClient chromaDbClient, FileDbContext dbContext)
    {
        _dbContext = dbContext;
        _chromaDbClient = chromaDbClient;
    }

    [HttpGet("initial-documents")]
    public async Task<IActionResult> GetInitialDocuments()
    {
        Console.WriteLine("Seeding files...");

        _dbContext.Files.RemoveRange(_dbContext.Files);
        await _dbContext.SaveChangesAsync();

        var collections = await _chromaDbClient.ListCollectionsAsync();

        Console.WriteLine($"Found {collections?.Count} collections in ChromaDB.");

        if (collections == null || collections.Count == 0)
        {
            Console.WriteLine("No collections found. Skipping file seeding.");
            return Ok("No collections found. Skipping file seeding.");
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
                var title = document.Metadata?.ContainsKey("title") == true ? document.Metadata["title"]?.ToString() : string.Empty;
                var sourceDoc = document.Metadata?.ContainsKey("source_doc") == true ? document.Metadata["source_doc"]?.ToString() : string.Empty;

                names.Add(name);
                var fileEntity = new FileEntity
                {
                    Name = name,
                    UploadedBy = "system",
                    SourceLink = source ?? string.Empty,
                    SourceType = sourceType ?? string.Empty,
                    Title = title ?? string.Empty,
                    SourceDoc = sourceDoc ?? string.Empty,
                };
                fileEntities.Add(fileEntity);
            }

            _dbContext.Files.AddRange(fileEntities);
            await _dbContext.SaveChangesAsync();
        }

        Console.WriteLine("File seeding completed.");
        return Ok("File seeding completed.");
    }
}