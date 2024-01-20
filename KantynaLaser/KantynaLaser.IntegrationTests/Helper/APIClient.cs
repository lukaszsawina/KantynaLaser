using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KantynaLaser.IntegrationTests.Helper;

public class APIClient
{
    private readonly string BaseUrl = "https://localhost:5001/api";
    private readonly HttpClient _client;

    public APIClient()
    {
        _client = new HttpClient();
    }

    public async Task<HttpResponseMessage> LoginAsync(string email = "kowal@wp.pl", string password = "Haslo1234!")
    {
        var loginRequestData = new
        {
            Email = email,
            Password = password
        };

        var jsonBody = JsonConvert.SerializeObject(loginRequestData);

        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"{BaseUrl}/Identity/login", content);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        return response;
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        return await _client.GetAsync($"{BaseUrl}/{url}");
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string apiUrl, object requestBody)
    {
        var jsonBody = JsonConvert.SerializeObject(requestBody);

        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        return await _client.PostAsync($"{BaseUrl}/{apiUrl}", content);
    }

    public async Task<HttpResponseMessage> PutAsync<T>(string apiUrl, object requestBody)
    {
        var jsonBody = JsonConvert.SerializeObject(requestBody);

        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        return await _client.PutAsync($"{BaseUrl}/{apiUrl}", content);
    }
    public async Task<HttpResponseMessage> DeleteAsync(string apiUrl)
    {
        return await _client.DeleteAsync($"{BaseUrl}/{apiUrl}");
    }
}
