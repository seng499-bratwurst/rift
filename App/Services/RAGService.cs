using System.Text.Json;
using Rift.LLM;
using Rift.App.Clients;
using Rift.App.Models;

public class RAGService
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

    public async Task<string> GenerateResponseAsync(string userQuery)
    {
        /// This is a rough outline of how the RAG service might work and is a starting point to
        /// work from. Please modify and update each of these methods as we build up the pipeline.

        // string ONCAPIJson = await _llmProvider.GenerateONCAPICall(userQuery);
        string ONCAPIData = "";

        var ragContext = await _chromaDbClient.GetRelevantDataAsync(userQuery);
        string relevantData = string.Join("\n", ragContext.RelevantDocuments.Select(d => d.Content));

        var relevantDocs = ragContext.RelevantDocuments.Select(d => d.Content).ToList();

        var rerankRequest = new RerankRequest
        {
            Query = userQuery,
            Docs = relevantDocs
        };
        var rerankResponse = await _reRankerClient.RerankAsync(rerankRequest);
        string reRankedData = string.Join("\n", rerankResponse.Reranked_Docs);

        // Build the prompt using the PromptBuilder
        string prompt = _promptBuilder.BuildPrompt(userQuery, reRankedData);

        // Generate a response using the LLM provider
        string responseFromLLM = await _llmProvider.GenerateFinalResponse(prompt, new JsonElement());

        string cleanedResponse = _responseProcessor.ProcessResponse(responseFromLLM);

        // Return the generated response
        return cleanedResponse;
    }
}