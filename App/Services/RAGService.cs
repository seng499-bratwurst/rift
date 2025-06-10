using System.Text.Json;
using Rift.LLM;

public class RAGService
{

    private readonly ILlmProvider _llmProvider;
    private readonly ChromaDBClient _chromaDbClient;
    private readonly PromptBuilder _promptBuilder;
    private readonly ReRanker _reRanker;
    private readonly ResponseProcessor _responseProcessor;

    public RAGService(
        ILlmProvider llmProvider,
        ChromaDBClient chromaDbClient,
        PromptBuilder promptBuilder,
        ReRanker reRanker,
        ResponseProcessor responseProcessor)
    {
        _llmProvider = llmProvider;
        _chromaDbClient = chromaDbClient;
        _promptBuilder = promptBuilder;
        _reRanker = reRanker;
        _responseProcessor = responseProcessor;
    }

    public async Task<string> GenerateResponseAsync(string userQuery)
    {
        /// This is a rough outline of how the RAG service might work and is a starting point to
        /// work from. Please modify and update each of these methods as we build up the pipeline.

        // string ONCAPIJson = await _llmProvider.GenerateONCAPICall(userQuery);
        string ONCAPIData = "";

        string relevantData = _chromaDbClient.GetRelevantDataAsync(userQuery);

        string reRankedData = _reRanker.ReRankAsync(ONCAPIData, relevantData);

        // Build the prompt using the PromptBuilder
        string prompt = _promptBuilder.BuildPrompt(userQuery, reRankedData);

        // Generate a response using the LLM provider
        string responseFromLLM = await _llmProvider.GenerateFinalResponse(prompt, new JsonElement());

        string cleanedResponse = _responseProcessor.ProcessResponse(responseFromLLM);

        // Return the generated response
        return cleanedResponse;
    }
}