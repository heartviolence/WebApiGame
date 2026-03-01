using Assets.Scripts.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public class ServerSandBoxService
    {
        public ServerSandBoxService()
        {
        }

        public async Task ShowMetheMoney()
        {
            await ApiCallHelper.PostAsync("SandBox/ShowMetheMoney");
        }
    }
}
