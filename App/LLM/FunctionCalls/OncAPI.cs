
using System.Text;
using System.Text.Json;

public class OncAPI
{
    private readonly HttpClient _httpClient;
    private readonly string _oncToken;

    public OncAPI(HttpClient httpClient, IConfiguration config)
    {
        if (string.IsNullOrEmpty(config["ONC_TOKEN"]))
        {
            throw new InvalidOperationException("ONC token is not configured.");
        }
        _oncToken = config["ONC_TOKEN"]!;
        _httpClient = httpClient;

    }

    public async Task<JsonElement> GetDataAsync(
        string DataEndpoint,
        Dictionary<string, string?>? queryParams = null
    )
    {
        var urlPath = new StringBuilder(DataEndpoint);
        urlPath.Append("?token=" + _oncToken);

        if (queryParams != null && queryParams.Count > 0)
        {
            var validParams = queryParams
                .Where(kv => !string.IsNullOrEmpty(kv.Value))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            if (validParams.Count > 0)
            {
                urlPath.Append("&" + string.Join("&", validParams.Select(kv => $"{kv.Key}={kv.Value}")));
            }
        }
        Console.WriteLine("urlPath: "+urlPath.ToString());
       
        var oncResponse = new HttpResponseMessage();
        try{
            oncResponse = await _httpClient.GetAsync(urlPath.ToString());
            if (!oncResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"ONC API error: {oncResponse.StatusCode}");
            }
        }
        catch(Exception ex){
            var generalErrorResponse = new
            {
                error = "API Error",
                message = "An error occurred while fetching data from ONC API.",
                statusCode = (int)oncResponse.StatusCode,
                details = ex.Message
            };
            return JsonDocument.Parse(JsonSerializer.Serialize(generalErrorResponse)).RootElement.Clone();
        }

        var oncContent = await oncResponse.Content.ReadAsStringAsync();
        var oncData = JsonDocument.Parse(oncContent).RootElement.Clone();
        // Console.WriteLine("oncData: " + oncData);
        
        return oncData;
    }
}
