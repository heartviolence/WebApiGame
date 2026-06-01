using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public static class ApiCallHelper
    {
        static public async Task<TResponse> PostAsync<TRequest, TResponse>(HttpClient client, string url, TRequest data, CancellationToken ct = default)
        {
            var response = await client.PostAsJsonAsync(url, data, cancellationToken: ct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        static public async Task<TResponse> PostAsync<TResponse>(HttpClient client, string url, CancellationToken ct = default)
        {
            var response = await client.PostAsync(url, new StringContent(""), cancellationToken: ct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        static public async Task PostAsync(HttpClient client, string url, CancellationToken ct = default)
        {
            var response = await client.PostAsync(url, new StringContent(""), cancellationToken: ct);
            response.EnsureSuccessStatusCode();
        }

        static public async Task<TResponse> GetAsync<TResponse>(HttpClient client, string url, CancellationToken ct = default)
        {
            var response = await client.GetAsync(url, cancellationToken: ct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        static public async Task PutAsync(HttpClient client, string url, CancellationToken ct = default)
        {
            var response = await client.PutAsync(url, new StringContent(""), cancellationToken: ct);
            response.EnsureSuccessStatusCode();
        }

        static public async Task DeleteAsync(HttpClient client, string url, CancellationToken ct = default)
        {
            var response = await client.DeleteAsync(url, cancellationToken: ct);
            response.EnsureSuccessStatusCode();
        }
    }
}
