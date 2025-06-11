using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

public class Deployments
{
    private readonly HttpClient _httpClient;
    // private readonly string _baseUrl = "https://data.oceannetworks.ca/api/";
    private readonly string _token;

    public Deployments(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _token = config["ONC_TOKEN"]!;
    }

    // /deployments ONC API
    public async Task<JsonElement> GetDeploymentsAsync(
    string? deviceCategoryCode = null,
    string? deviceCode = null,
    string? locationCode = null,
    string? propertyCode = null,
    string? dateFrom = null,
    string? dateTo = null)
    {
        var endpoint = new StringBuilder("https://data.oceannetworks.ca/api/deployments?token=" + _token);
        // sample URL: https://data.oceannetworks.ca/api/deployments?deviceCategoryCode=AISRECEIVER&dateFrom=2015-09-17T00%3A00%3A00.000Z&dateTo=2015-09-17T13%3A00%3A00.000Z&token=
        // https://data.oceannetworks.ca/api/deployments?token=

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

        if (!string.IsNullOrWhiteSpace(dateFrom))
        {
            endpoint.Append("&dateFrom=" + dateFrom);
        }

        if (!string.IsNullOrWhiteSpace(dateTo))
        {
            endpoint.Append("&dateTo=" + dateTo);
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
