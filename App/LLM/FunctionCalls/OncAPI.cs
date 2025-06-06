using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

public class OncAPI
{
    private readonly HttpClient _httpClient;
    // private readonly string _baseUrl = "https://data.oceannetworks.ca/api/";
    private readonly string _token;

    public OncAPI(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _token = config["ONC_TOKEN"]!;
    }

    public async Task<JsonElement> GetDeviceCategoriesAsync(
    string? deviceCategoryCode = null,
    string? deviceCategoryName = null,
    string? locationCode = null,
    string? description = null,
    string? propertyCode = null)
    {
        var endpoint = new StringBuilder("https://data.oceannetworks.ca/api/deviceCategories?token=" + _token);
        Console.WriteLine("[DEBUG] ONC API base URL: " + endpoint);
        // sample URL: https://data.oceannetworks.ca/api/deviceCategories?deviceCategoryCode=ACCELEROMETER&token=
        // https://data.oceannetworks.ca/api/deviceCategories?token=
        // https://data.oceannetworks.ca/api/deviceCategories?deviceCategoryCode=CTD&deviceCategoryName=Conductivity&description=Temperature&token=
        // logic for /deviceCategories

        if (!string.IsNullOrWhiteSpace(deviceCategoryCode))
        {
            endpoint.Append("&deviceCategoryCode=" + deviceCategoryCode);
        }

        if (!string.IsNullOrWhiteSpace(deviceCategoryName))
        {
            endpoint.Append("&deviceCategoryName=" + deviceCategoryName);
        }

        if (!string.IsNullOrWhiteSpace(locationCode))
        {
            endpoint.Append("&locationCode=" + locationCode);
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            endpoint.Append("&description=" + description);
        }

        if (!string.IsNullOrWhiteSpace(propertyCode))
        {
            endpoint.Append("&propertyCode=" + propertyCode);
        }

        string final_onc_url = endpoint.ToString();

        Console.WriteLine("[DEBUG] ONC API full URL: " + final_onc_url);

        var oncResponse = await _httpClient.GetAsync(final_onc_url);
        if (!oncResponse.IsSuccessStatusCode)
            throw new HttpRequestException($"ONC API error: {oncResponse.StatusCode}");


        var oncContent = await oncResponse.Content.ReadAsStringAsync();
        var oncData = JsonDocument.Parse(oncContent).RootElement.Clone();
        Console.WriteLine("[DEBUG] ONC API Data: " + oncData);

        return oncData;
    }
    
    // /deployments API
    public async Task<JsonElement> GetDeploymentsAsync(
    string? deviceCategoryCode = null,
    string? deviceCode = null,
    string? locationCode = null,
    string? propertyCode = null,
    string? dateFrom = null,
    string? dateTo = null)
    {
        var endpoint = new StringBuilder("https://data.oceannetworks.ca/api/deployments?token=" + _token);
        Console.WriteLine("[DEBUG] ONC API base URL: " + endpoint);
        // sample URL: https://data.oceannetworks.ca/api/deviceCategories?deviceCategoryCode=ACCELEROMETER&token=
        // https://data.oceannetworks.ca/api/deviceCategories?token=
        // https://data.oceannetworks.ca/api/deployments?deviceCategoryCode=AISRECEIVER&dateFrom=2015-09-17T00%3A00%3A00.000Z&dateTo=2015-09-17T13%3A00%3A00.000Z&token=
        // logic for /deviceCategories

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

        Console.WriteLine("[DEBUG] ONC API full URL: " + final_onc_url);

        var oncResponse = await _httpClient.GetAsync(final_onc_url);
        if (!oncResponse.IsSuccessStatusCode)
            throw new HttpRequestException($"ONC API error: {oncResponse.StatusCode}");


        var oncContent = await oncResponse.Content.ReadAsStringAsync();
        var oncData = JsonDocument.Parse(oncContent).RootElement.Clone();
        Console.WriteLine("[DEBUG] ONC API Data: " + oncData);

        return oncData;
    }

}
