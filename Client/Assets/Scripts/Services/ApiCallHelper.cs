using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public static class ApiCallHelper
    {
        static public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var response = await GameApiClient.Client.PostAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        static public async Task<TResponse> PostAsync<TResponse>(string url)
        {
            var response = await GameApiClient.Client.PostAsync(url, new StringContent(""));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        static public async Task PostAsync(string url)
        {
            var response = await GameApiClient.Client.PostAsync(url, new StringContent(""));
            response.EnsureSuccessStatusCode();
        }

        static public async Task<TResponse> GetAsync<TResponse>(string url)
        {
            var response = await GameApiClient.Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        static public async Task PutAsync(string url)
        {
            var response = await GameApiClient.Client.PutAsync(url, new StringContent(""));
            response.EnsureSuccessStatusCode();
        }

        static public async Task DeleteAsync(string url)
        {
            var response = await GameApiClient.Client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}
