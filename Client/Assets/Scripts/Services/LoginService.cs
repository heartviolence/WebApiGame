using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Assets.Scripts.Services
{
    internal class LoginService
    {
        public LoginResponse LoginData;
        public LoginService()
        {
        }

        public async Task<RegisterResponse> Register(string username, string password)
        {
            var loginRequest = new LoginRequest()
            {
                Username = username,
                Password = password
            };
            HttpResponseMessage response = await GameApiClient.Client.PostAsJsonAsync("Login/Register", loginRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RegisterResponse>();
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            var loginRequest = new LoginRequest()
            {
                Username = username,
                Password = password
            };

            var loginResponse = await GameApiClient.Client.PostAsJsonAsync($"Login/Login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();
            var result = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
            GameApiClient.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
            return result;
        }
    }
}
