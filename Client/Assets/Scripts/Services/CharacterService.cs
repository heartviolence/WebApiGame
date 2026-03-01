using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public class CharacterService
    {
        public CharacterService()
        {
        }

        public async Task<GameCharacterDTO> UseLevelUpItem(string characterId, int itemCount)
        {
            return await ApiCallHelper.PostAsync<GameCharacterDTO>($"Character/UseLevelUpItem?characterName={characterId}&itemCount={itemCount}");
        }

        public async Task<GameCharacterDTO> RankUp(string characterId)
        {
            return await ApiCallHelper.PostAsync<GameCharacterDTO>($"Character/RankUp?characterName={characterId}");
        }
    }
}
