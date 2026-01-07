using Assets.Scripts.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class ServerSandBoxService
    {
        public ServerSandBoxService()
        {
        }

        public async Task ShowMetheMoney()
        {
            HttpResponseMessage response = await GameApiClient.Client.PostAsync("SandBox/ShowMetheMoney", new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
    }
}
