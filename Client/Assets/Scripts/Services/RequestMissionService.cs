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
                CharacterCode = new() { "Sora", "Flora" }
            };

            return await ApiCallHelper.PostAsync<RequestMissionStartRequest, bool>($"RequestMission/StartMission", requestBody);
        }

        public async Task RequestMissionCompleteCheck()
        {
            await ApiCallHelper.PutAsync($"RequestMission/RequestMissionCompleteCheck");
        }
    }
}
