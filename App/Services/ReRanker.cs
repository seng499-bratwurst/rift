using System.Text.Json;
using Rift.App.Models;

public class RerankRequest
{
    public string? Query { get; set; }
    public List<DocumentChunk>? Docs { get; set; }
}


public class RerankResponse
{
    public List<DocumentChunk> Reranked_Docs { get; set; } = new List<DocumentChunk>();
}

public class ReRankerClient
{
    private readonly HttpClient _httpClient;

    public ReRankerClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://reranker:6000/");
    }

    public async Task<RerankResponse> RerankAsync(RerankRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("rerank", request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RerankResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new RerankResponse();
    }

    public async Task<string> TestAsync()
    {
        var response = await _httpClient.GetAsync("rerank");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}