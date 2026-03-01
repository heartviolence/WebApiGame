using Assets.Scripts.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Assets.Scripts.Services
{
    internal class UserService
    {
        public UserService() { }

        public async Task<List<GameCharacterDTO>> GetCharacters()
        {
            HttpResponseMessage response = await GameApiClient.Client.GetAsync($"User/GetCharacters");
            response.EnsureSuccessStatusCode();
            if (response.Content.Headers.ContentLength > 0)
            {
                return await response.Content.ReadFromJsonAsync<List<GameCharacterDTO>>();
            }
            return new List<GameCharacterDTO>();
        }

        public async Task<UserInfoDTO> GetUserInfo()
        {
            return await ApiCallHelper.GetAsync<UserInfoDTO>($"User/GetUserInfo");
        }

        public async Task Gacha()
        {
            await ApiCallHelper.PostAsync($"User/Gacha");
        }

        public async Task DeleteAll()
        {
            await ApiCallHelper.DeleteAsync($"User/DeleteAll");
        }

    }
}
