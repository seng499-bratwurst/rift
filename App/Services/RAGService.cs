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
    // TODO: This will need to be updated with some Object that includes the ONC API data instead of a string
    string ONCAPIData = _llmProvider.GatherONCAPIData(userQuery);

    string relevantData = _chromaDbClient.GetRelevantDataAsync(userQuery);

    string reRankedData = _reRanker.ReRankAsync(ONCAPIData, relevantData);

    // Build the prompt using the PromptBuilder
    string prompt = _promptBuilder.BuildPrompt(userQuery, reRankedData);

    // Generate a response using the LLM provider
    string responseFromLLM = await _llmProvider.GenerateResponseAsync(prompt);

    string cleanedResponse = _responseProcessor.ProcessResponse(responseFromLLM);

    // Return the generated response
    return cleanedResponse;
  }
}