using System.Text.Json;

public class OncAPI
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://data.oceannetworks.ca/api/";
    private readonly string _token;

    public OncAPI(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _token = config["ONC_TOKEN"]!;
    }

    public async Task<JsonDocument> GetDeviceCategoriesAsync(
    string? deviceCategoryCode = null,
    string? deviceCategoryName = null,
    string? locationCode = null,
    string? description = null,
    string? propertyCode = null)
    {
        // logic for /deviceCategories

    }

}
