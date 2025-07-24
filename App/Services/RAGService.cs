using System.Text.Json;
using Rift.LLM;
using Rift.App.Clients;
using Rift.Models;
using Rift.App.Models;

public class RAGService : IRAGService
{
    private readonly ILlmProvider _llmProvider;
    private readonly ChromaDBClient _chromaDbClient;
    private readonly PromptBuilder _promptBuilder;
    private readonly ReRankerClient _reRankerClient;
    private readonly ResponseProcessor _responseProcessor;

    public RAGService(
        ILlmProvider llmProvider,
        ChromaDBClient chromaDbClient,
        PromptBuilder promptBuilder,
        ReRankerClient reRankerClient,
        ResponseProcessor responseProcessor)
    {
        _llmProvider = llmProvider;
        _chromaDbClient = chromaDbClient;
        _promptBuilder = promptBuilder;
        _reRankerClient = reRankerClient;
        _responseProcessor = responseProcessor;
    }

    public async Task<(string cleanedResponse, List<string> relevantDocTitles)> GenerateResponseAsync(string userQuery, List<Message>? messageHistory)
    {
        messageHistory ??= new List<Message>();

        var oncApiData = await _llmProvider.GatherOncAPIData(userQuery);

        var chromaDocuments = (await _chromaDbClient.GetRelevantDataAsync(userQuery, 20, similarityThreshold: 0.5)).RelevantDocuments;

        var relevantDocuments = chromaDocuments.Select(doc => {
            var documentTitle = string.Empty;
            if (doc.Metadata?.GetValueOrDefault("source_type")?.ToString() != "confluence_json")
            {
                documentTitle = doc.Metadata?.GetValueOrDefault("source_doc")?.ToString();
            } 
            else
            {
                documentTitle = "ONC Confluence Data (" + doc.Metadata?.GetValueOrDefault("source_doc")?.ToString() + ")";
            }
            return new DocumentChunk
            {
                Title = documentTitle ?? string.Empty,
                Content = doc.Content
            };
        }).ToList();


        // Console.WriteLine($"Relevant Document Titles: {string.Join(", ", relevantDocuments.Select(doc => doc.Title))}");
        var rerankRequest = new RerankRequest
        {
            Query = userQuery,
            Docs = relevantDocuments
        };
        var rerankedDocuments = (await _reRankerClient.RerankAsync(rerankRequest)).Reranked_Docs;

        var prompt = _promptBuilder.BuildPrompt(
            userQuery,
            messageHistory,
            oncApiData,
            rerankedDocuments
        );

        Console.WriteLine("Generated Prompt:");
        Console.WriteLine("\tUser Query: " + prompt.UserQuery);
        // Console.WriteLine("\tMessages: " + JsonSerializer.Serialize(prompt.Messages));
        Console.WriteLine("\tAPI Data:" + prompt.OncAPIData);
        Console.WriteLine("\tRelevant Data: " + JsonSerializer.Serialize(prompt.RelevantDocumentChunks));

        var finalResponse = await _llmProvider.GenerateFinalResponseRAG(prompt);

        var cleanedResponse = _responseProcessor.ProcessResponse(finalResponse);

        return (cleanedResponse, rerankedDocuments.Select(doc => doc.Title).Distinct().ToList());
    }
}