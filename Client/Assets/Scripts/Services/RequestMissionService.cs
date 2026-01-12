using Assets.Scripts.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Assets.Scripts.Services
{
    internal class RequestMissionService
    {
        public async Task<bool> RequestMissionStart()
        {
            var requestBody = new RequestMissionStartRequest()
            {
                MissionCode = "00-00-01",
                CharacterCode = new() { "00_01", "00_04" }
            };
            HttpResponseMessage response = await GameApiClient.Client.PostAsJsonAsync($"RequestMission/StartMission", requestBody);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task RequestMissionCompleteCheck()
        {
            HttpResponseMessage response = await GameApiClient.Client.PutAsync($"RequestMission/RequestMissionCompleteCheck", new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
    }
}
