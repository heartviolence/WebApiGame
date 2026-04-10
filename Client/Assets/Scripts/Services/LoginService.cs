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

            return await ApiCallHelper.PostAsync<LoginRequest, RegisterResponse>("Login/Register", loginRequest);
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            var loginRequest = new LoginRequest()
            {
                Username = username,
                Password = password
            };
            var loginResponse = await ApiCallHelper.PostAsync<LoginRequest, LoginResponse>("Login/Login", loginRequest);
            GameApiClient.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
            return loginResponse;
        }
        public async Task Logout()
        {
            GameApiClient.Client.DefaultRequestHeaders.Authorization = null;
            await Task.CompletedTask;
        }
    }
}
