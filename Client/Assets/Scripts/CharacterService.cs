using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class CharacterService
    {
        public CharacterService()
        {
        }

        public async Task<GameCharacterDTO> UseLevelUpItem(string characterId, int itemCount)
        {
            HttpResponseMessage response = await GameApiClient.Client.PostAsync($"Character/UseLevelUpItem?characterName={characterId}&itemCount={itemCount}", new StringContent(""));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GameCharacterDTO>();
        }

        public async Task<GameCharacterDTO> RankUp(string characterId)
        {
            HttpResponseMessage response = await GameApiClient.Client.PostAsync($"Character/RankUp?characterName={characterId}", new StringContent(""));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GameCharacterDTO>();
        }
    }
}
