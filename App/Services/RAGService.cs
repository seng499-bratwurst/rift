using System.Text.Json;
using Rift.LLM;
using Rift.App.Clients;
using Rift.App.Models;
using Rift.Services;
using Rift.Models;

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

    public async Task<string> GenerateResponseAsync(string userQuery, List<Message>? messageHistory)
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

        var oncApiData = await _llmProvider.GatherOncAPIData(userQuery);

        // Might want to update this to return a list of Relevant Documents instead.
        var relevantData = await _chromaDbClient.GetRelevantDataAsync(userQuery, similarityThreshold: 0.5);

        // var reRankedData = _reRanker.ReRankAsync(oncApiData, RelevantDocuments.RelevantDocuments);

        var prompt = _promptBuilder.BuildPrompt(userQuery, messageHistory, oncApiData, relevantData.RelevantDocuments);

        Console.WriteLine("Generated Prompt:");
        Console.WriteLine("\tUser Query: " + prompt.UserQuery);
        Console.WriteLine("\tMessage History: " + JsonSerializer.Serialize(prompt.MessageHistory));
        Console.WriteLine("\tAPI Data:" + prompt.OncAPIData);
        Console.WriteLine("\tRelevant Data: " + JsonSerializer.Serialize(prompt.RelevantDocuments));

        var finalResponse = await _llmProvider.GenerateFinalResponseRAG(prompt);

        var cleanedResponse = _responseProcessor.ProcessResponse(finalResponse);

        return cleanedResponse;
    }
}