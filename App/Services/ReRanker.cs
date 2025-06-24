using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

public class RerankRequest
{
    public string Query { get; set; }
    public List<string> Docs { get; set; }
}

public class RerankResponse
{
    public List<string> Reranked_Docs { get; set; }
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
        return JsonSerializer.Deserialize<RerankResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<string> TestAsync()
    {
        var response = await _httpClient.GetAsync("rerank");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}