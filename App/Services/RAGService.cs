using System.Text.Json;
using Rift.LLM;
using Rift.App.Clients;
using Rift.App.Models;
using Rift.Services;

public class RAGService
{

    private readonly IMessageService _messageService;
    private readonly ILlmProvider _llmProvider;
    private readonly ChromaDBClient _chromaDbClient;
    private readonly PromptBuilder _promptBuilder;
    private readonly ReRankerClient _reRankerClient;
    private readonly ResponseProcessor _responseProcessor;

    public RAGService(
        IMessageService messageService,
        ILlmProvider llmProvider,
        ChromaDBClient chromaDbClient,
        PromptBuilder promptBuilder,
        ReRankerClient reRankerClient,
        ResponseProcessor responseProcessor)
    {
        _messageService = messageService;
        _llmProvider = llmProvider;
        _chromaDbClient = chromaDbClient;
        _promptBuilder = promptBuilder;
        _reRankerClient = reRankerClient;
        _responseProcessor = responseProcessor;
    }

    public async Task<string> GenerateResponseAsync(string userQuery, string userId, int conversationId)
    {
        /* 
        Rough outline of the steps to generate a response a response:
            1. Get userID from the JWT token
            2. Gather User Converstion History
            3. Gather ONC API Data using small LLM
            4. Get relevant data from vector database
            5. Re-rank data
            6. Build prompt using PromptBuilder
            7. Generate final response using larger LLM
            8. Process response using ResponseProcessor
            9. Return the final response to the user
        */

        var messageHistory = await _messageService.GetMessagesForConversationAsync(userId, conversationId);

        var oncApiData = await _llmProvider.GatherOncAPIData(userQuery);

        // Will probably want to clean this up and have the RAGContext object created in
        // here and have these methods update it or something.
        var relevantData = await _chromaDbClient.GetRelevantDataAsync(userQuery);

        // var reRankedData = _reRanker.ReRankAsync(oncApiData, RelevantDocuments.RelevantDocuments);

        // Might not be necessary to use the PromptBilder but we could move the system prompt into here as well
        // and maybe some extra context/items here. (userType, specialInstructions, etc.)
        var prompt = _promptBuilder.BuildPrompt(userQuery, messageHistory, oncApiData, relevantData.RelevantDocuments);

        var finalResponse = await _llmProvider.GenerateFinalResponseRAG(prompt);

        var cleanedResponse = _responseProcessor.ProcessResponse(finalResponse);

        return cleanedResponse;
    }
}