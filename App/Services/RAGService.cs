using System.Text.Json;
using Rift.LLM;
using Rift.App.Clients;
using Rift.Models;

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

        var relevantDocuments = (await _chromaDbClient.GetRelevantDataAsync(userQuery, similarityThreshold: 0.5)).RelevantDocuments;

        var relevantDocTitles = relevantDocuments
            .Select(doc => doc.Id.Split('_')[0])
            .Distinct()
            .ToList();
        // TODO: Add reranker back once new prompts are working
        // var rerankRequest = new RerankRequest
        // {
        //     Query = userQuery,
        //     Docs = relevantData.RelevantDocuments.Select(doc => doc.Content).ToList()
        // };
        // var rerankedResponse = await _reRankerClient.RerankAsync(rerankRequest);

        var prompt = _promptBuilder.BuildPrompt(
            userQuery,
            messageHistory,
            oncApiData,
            relevantDocuments
        );

        // Console.WriteLine("Generated Prompt:");
        // Console.WriteLine("\tUser Query: " + prompt.UserQuery);
        // Console.WriteLine("\tMessages: " + JsonSerializer.Serialize(prompt.Messages));
        // Console.WriteLine("\tAPI Data:" + prompt.OncAPIData);
        // Console.WriteLine("\tRelevant Data: " + JsonSerializer.Serialize(prompt.RelevantDocumentChunks));

        var finalResponse = await _llmProvider.GenerateFinalResponseRAG(prompt);

        var cleanedResponse = _responseProcessor.ProcessResponse(finalResponse);

        return (cleanedResponse, relevantDocTitles);
    }
}