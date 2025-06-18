using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public class RerankRequest
{
    public string query { get; set; }
    public List<string> docs { get; set; }
}

public class RerankResponse
{
    public List<string> reranked_docs { get; set; }
}

public class ReRanker
{
    private readonly HttpClient _client;

    public ReRanker()
    {
        _client = new HttpClient();
    }

    public async Task<string> ReRankAsync(string oncApiData, string relevantData)
    {
        // Assume relevantData is a comma-separated string of docs
        List<string> docs = new List<string>(relevantData.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

        var request = new RerankRequest
        {
            query = oncApiData,
            docs = docs
        };

        try
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("http://localhost:8080/rerank", request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RerankResponse>(json);

            // Join re-ranked docs into a single string for return
            return string.Join("\n", result.reranked_docs);
        }
        catch (Exception ex)
        {
            //improve error handling here if needed
            return $"Error: {ex.Message}";
        }
    }
}

