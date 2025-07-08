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

    public async IAsyncEnumerable<string> GenerateResponseAsync(string userQuery, List<Message>? messageHistory)
    {
        /* 
        Rough outline of the steps to generate a response a response:
            1. Gather ONC API Data using small LLM
            2. Get relevant data from vector database
            3. Re-rank data
            4. Build prompt using PromptBuilder
            5. Generate final response using larger LLM
            6. Process response using ResponseProcessor
            7. Return the final response to the user
        */
    
        messageHistory ??= new List<Message>();
        var startTime = DateTime.UtcNow;
        var oncApiData = await _llmProvider.GatherOncAPIData(userQuery);
        // Might want to update this to return a list of Relevant Documents instead.
        var relevantData = await _chromaDbClient.GetRelevantDataAsync(userQuery, similarityThreshold: 0.5);

        var rerankRequest = new RerankRequest
        {
            Query = userQuery,
            Docs = relevantData.RelevantDocuments.Select(doc => doc.Content).ToList()
        };
        var rerankedResponse = await _reRankerClient.RerankAsync(rerankRequest);

        var prompt = _promptBuilder.BuildPrompt(
            userQuery,
            messageHistory,
            oncApiData,
            rerankedResponse?.Reranked_Docs ?? new List<string>()
        );
    
        var responseBuilder = new System.Text.StringBuilder();
        await foreach (var chunk in _llmProvider.GenerateFinalResponseRAG(prompt))
        {
            yield return chunk;
        }
        yield return responseBuilder.ToString();
    }
}