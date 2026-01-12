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
        public async Task<GameState> Start()
        {
            HttpResponseMessage response = await GameApiClient.Client.PostAsync("Game/Start", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var gamestate = await response.Content.ReadAsStringAsync();
            return MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(gamestate));
        }

        public async Task<GameState> SelectNPC(int index)
        {
            HttpResponseMessage response = await GameApiClient.Client.PostAsync($"Game/SelectNPC?index={index}", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var gamestate = await response.Content.ReadAsStringAsync();
            return MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(gamestate));
        }

        public async Task<GameState> SelectCard(int index)
        {
            HttpResponseMessage response = await GameApiClient.Client.PostAsync($"Game/SelectCard?index={index}", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var gamestate = await response.Content.ReadAsStringAsync();
            return MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(gamestate));
        }

        public async Task<GameState> PowerUp()
        {
            HttpResponseMessage response = await GameApiClient.Client.PostAsync("Game/PowerUp", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var gamestate = await response.Content.ReadAsStringAsync();
            return MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(gamestate));
        }

        public async Task<GameState> NextFloor()
        {
            HttpResponseMessage response = await GameApiClient.Client.PostAsync("Game/NextFloor", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var gamestate = await response.Content.ReadAsStringAsync();
            return MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(gamestate));
        }

        public async Task<GameState> BattleEnd()
        {
            HttpResponseMessage response = await GameApiClient.Client.PostAsync("Game/BattleEnd", new StringContent(""));
            response.EnsureSuccessStatusCode();
            var gamestate = await response.Content.ReadAsStringAsync();
            return MessagePackSerializer.Deserialize<GameState>(Convert.FromBase64String(gamestate));
        }
    }
}
