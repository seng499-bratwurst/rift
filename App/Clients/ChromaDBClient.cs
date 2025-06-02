public class ChromaDBClient
{
  private readonly HttpClient _httpClient;
  private readonly string _baseUrl;

  public ChromaDBClient(HttpClient httpClient, string baseUrl = "http://chromadb:8000")
  {
    _httpClient = httpClient;
    _baseUrl = baseUrl.TrimEnd('/');
  }

  public async Task<bool> AddAsync(AddRequest request)
  {
    var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/add", request);
    return response.IsSuccessStatusCode;
  }

  public async Task<string?> QueryAsync(QueryRequest request)
  {
    var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/query", request);
    if (!response.IsSuccessStatusCode) return null;
    var result = await response.Content.ReadAsStringAsync();

    return result;
  }

  public string GetRelevantDataAsync(string query)
  {
    throw new NotImplementedException("The Gathering of relevant data is from DB is not implemented yet.");
  }
}