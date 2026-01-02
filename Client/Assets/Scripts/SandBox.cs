using Assets.Scripts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public Button CharacterDeleteButton;
    public Button RequestMissionStartButton;
    public Button RequestMissionCompleteButton;

    List<GameCharacterDTO> _characters = new();
    void Start()
    {
        _client = new HttpClient();
        _client.BaseAddress = new System.Uri("https://localhost:7067/");
        _client.Timeout = TimeSpan.FromSeconds(5);

        GachaButton.onClick.AddListener(() => GachaTest());
        CharacterDeleteButton.onClick.AddListener(() => DeleteAll());
        RequestMissionStartButton.onClick.AddListener(() => RequestMissionStart());
        RequestMissionCompleteButton.onClick.AddListener(() => RequestMissionCompleteCheck());
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
                _characters = await response.Content.ReadFromJsonAsync<List<GameCharacterDTO>>();
                Debug.Log("Character Info---------------------------");
                foreach (var character in _characters)
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

    async Task DeleteAll()
    {
        Debug.Log("Starting DeleteAll Test");
        try
        {
            HttpResponseMessage response = await _client.DeleteAsync($"Character/DeleteAll");
            response.EnsureSuccessStatusCode();
            await CharacterCallTest();
        }
        catch (Exception ex)
        {
            Debug.LogError($"DeleteAll Call Failed: {ex.Message}");
        }
    }

    async Task RequestMissionStart()
    {
        Debug.Log("Starting RequestMissionStart Call...");
        try
        {
            var requestBody = new RequestMissionStartRequest()
            {
                MissionCode = "00-00-01",
                CharacterCode = new() { "00_01", "00_04" }
            };
            HttpResponseMessage response = await _client.PostAsJsonAsync($"RequestMission/StartMission", requestBody);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Debug.LogError($"RequestMissionStart Call Failed: {ex.Message}");
        }
    }


    async Task RequestMissionCompleteCheck()
    {
        Debug.Log("Starting RequestMissionCompleteCheck Call...");
        try
        {
            HttpResponseMessage response = await _client.PutAsync($"RequestMission/RequestMissionCompleteCheck", new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Debug.LogError($"RequestMissionCompleteCheck Call Failed: {ex.Message}");
        }
    }
}
