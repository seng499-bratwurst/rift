using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

public class Properties
{
    private readonly HttpClient _httpClient;
    private readonly string _token;

    public Properties(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _token = config["ONC_TOKEN"]!;
    }

    // properties ONC API call
    public async Task<JsonElement> GetPropertiesAsync(
    string? deviceCategoryCode = null,
    string? deviceCode = null,
    string? locationCode = null,
    string? propertyCode = null,
    string? description = null,
    string? propertyName = null)
    {
        var endpoint = new StringBuilder("https://data.oceannetworks.ca/api/properties?token=" + _token);
        // sample URL: https://data.oceannetworks.ca/api/properties?propertyCode=conductivity&locationCode=BACAX&deviceCategoryCode=CTD&token=

        if (!string.IsNullOrWhiteSpace(deviceCategoryCode))
        {
            endpoint.Append("&deviceCategoryCode=" + deviceCategoryCode);
        }

        if (!string.IsNullOrWhiteSpace(deviceCode))
        {
            endpoint.Append("&deviceCode=" + deviceCode);
        }

        if (!string.IsNullOrWhiteSpace(locationCode))
        {
            endpoint.Append("&locationCode=" + locationCode);
        }

        if (!string.IsNullOrWhiteSpace(propertyCode))
        {
            endpoint.Append("&propertyCode=" + propertyCode);
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            endpoint.Append("&description=" + description);
        }

        if (!string.IsNullOrWhiteSpace(propertyName))
        {
            endpoint.Append("&propertyName=" + propertyName);
        }

        string final_onc_url = endpoint.ToString();


        var oncResponse = await _httpClient.GetAsync(final_onc_url);
        if (!oncResponse.IsSuccessStatusCode)
            throw new HttpRequestException($"ONC API error: {oncResponse.StatusCode}");


        var oncContent = await oncResponse.Content.ReadAsStringAsync();
        var oncData = JsonDocument.Parse(oncContent).RootElement.Clone();
        
        return oncData;
    }

}
