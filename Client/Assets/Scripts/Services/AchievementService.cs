using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public class AchievementService
    {
        public async Task GainAchievementRewards(string achievementName)
        {
            HttpResponseMessage response = await GameApiClient.Client.PostAsync($"Achievement/GainAcheivementRewards?achievementName={achievementName}", new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
    }
}
