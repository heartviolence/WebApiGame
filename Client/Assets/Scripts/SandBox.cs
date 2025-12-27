using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SandBox : MonoBehaviour
{
    HttpClient _client;
    LoginResponse _loginResult;

    public Button GachaButton;
    void Start()
    {
        _client = new HttpClient();
        _client.BaseAddress = new System.Uri("https://localhost:7067/");
        _client.Timeout = TimeSpan.FromSeconds(5);

        GachaButton.onClick.AddListener(() => GachaTest());
        LoginCall();
    }

    async Task LoginCall()
    {
        Debug.Log("Starting Login Call...");
        await RegisterCallTest();

        var loginRequest = new LoginRequest()
        {
            Username = "admin",
            Password = "password"
        };
        try
        {
            var loginResponse = await _client.PostAsJsonAsync($"Login/Login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();
            var loginResponseContent = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
            _loginResult = loginResponseContent;
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
            HttpResponseMessage response = await _client.SendAsync(request);
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        try
        {
            HttpResponseMessage response = await _client.GetAsync("Login/LoginTest");
            response.EnsureSuccessStatusCode();
            Debug.Log($"Login Test Call 2 Success");
            await CharacterCallTest();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Login Test Call 2 Failed: {ex.Message}");
        }
    }

    async Task CharacterCallTest()
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"Character/GetCharacters");
            response.EnsureSuccessStatusCode();
            Debug.Log($"CharacterCallTestSuccess");
            if (response.Content.Headers.ContentLength > 0)
            {
                var result = await response.Content.ReadFromJsonAsync<List<GameCharacterDTO>>();
                Debug.Log("Character Info---------------------------");
                foreach (var character in result)
                {
                    Debug.Log($"Character ID: {character.CharacterID}, Level: {character.Level}, EXP: {character.EXP}");
                }
                Debug.Log("---------------------------");
            }
            else
            {
                Debug.Log($"Chracters is null");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"CharacterCallTestFailed : {ex.Message}");
        }
    }

    async Task RegisterCallTest()
    {
        Debug.Log("Starting Register Call...");
        var loginRequest = new LoginRequest()
        {
            Username = "admin",
            Password = "password"
        };
        try
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("Login/Register", loginRequest);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();
            if (result.IsSuccess)
            {
                Debug.Log("Register Call Success");
            }
            else
            {
                Debug.Log($"Register Call Failed,message:{result.Message}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Register Call Failed: {ex.Message}");
            throw;
        }
    }

    async Task GachaTest()
    {
        Debug.Log("Starting Gacha Call...");
        try
        {
            HttpResponseMessage response = await _client.PostAsync($"Character/Gacha", new StringContent(""));
            response.EnsureSuccessStatusCode();
            await CharacterCallTest();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Gacha Call Failed: {ex.Message}");
        }
    }
}
