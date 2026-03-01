using Assets.Scripts.Shared.Games;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace Assets.Scripts.Services
{
    public class GameService
    {

        async Task<GameState> PostAsync(string url)
        {
            HttpResponseMessage response = await GameApiClient.Client.PostAsync(url, new StringContent(""));
            response.EnsureSuccessStatusCode();
            var gamestate = await response.Content.ReadAsStringAsync();
            return MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(gamestate));
        }

        public async Task<GameState> Start()
        {
            return await PostAsync("Game/Start");
        }

        public async Task<GameState> SelectNPC(int index)
        {
            return await PostAsync($"Game/SelectNPC?index={index}");
        }

        public async Task<GameState> SelectCard(int index)
        {
            return await PostAsync($"Game/SelectCard?index={index}");
        }

        public async Task<GameState> PowerUp()
        {
            return await PostAsync("Game/PowerUp");
        }

        public async Task<GameState> NextFloor()
        {
            return await PostAsync("Game/NextFloor");
        }

        public async Task<GameState> BattleEnd()
        {
            return await PostAsync("Game/BattleEnd");
        }
    }
}
