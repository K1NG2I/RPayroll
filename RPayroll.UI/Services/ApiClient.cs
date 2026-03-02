using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RPayroll.UI.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly TokenStore _tokenStore;

    public ApiClient(HttpClient httpClient, TokenStore tokenStore)
    {
        _httpClient = httpClient;
        _tokenStore = tokenStore;
    }

    public async Task<TResponse?> GetAsync<TResponse>(string url)
    {
        ApplyAuthHeader();
        return await _httpClient.GetFromJsonAsync<TResponse>(url);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest request)
    {
        ApplyAuthHeader();
        var response = await _httpClient.PostAsJsonAsync(url, request);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    private void ApplyAuthHeader()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
        if (!string.IsNullOrWhiteSpace(_tokenStore.Token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _tokenStore.Token);
        }
    }
}
