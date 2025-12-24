using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    HttpClient client;
    void Start()
    {
        client = new HttpClient();
        client.BaseAddress = new System.Uri("https://localhost:7067/");
        client.Timeout = TimeSpan.FromSeconds(5);

        LoginCall();
    }

    async Task LoginCall()
    {
        Debug.Log("Starting Login Call...");

        var loginRequest = new LoginRequest()
        {
            Username = "admin",
            Password = "password"
        };
        try
        {
            var loginResponse = await client.PostAsJsonAsync($"Login/Login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();
            var loginResponseContent = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

            Debug.Log($"Login Success: {loginResponseContent.IsSuccess}, Token: {loginResponseContent.Token}");
            await LoginTestCall2(loginResponseContent.Token);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Login Call Failed: {ex.Message}");
        }
    }

    async Task LoginTestCall(string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "Login/LoginTest");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Debug.Log($"Login Test Call Success");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Login Test Call Failed: {ex.Message}");
        }
    }

    async Task LoginTestCall2(string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        try
        {
            HttpResponseMessage response = await client.GetAsync("Login/LoginTest");
            response.EnsureSuccessStatusCode();
            Debug.Log($"Login Test Call 2 Success");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Login Test Call 2 Failed: {ex.Message}");
        }
    }
}
