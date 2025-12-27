using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model.DbContexts;
using System;

namespace SampleWebApi.Service
{
    public class CharacterService
    {
        List<string> allCharacters = new();
        Random random = new();
        public CharacterService()
        {
            allCharacters.Add(CharacterId.Character_Sora);
            allCharacters.Add(CharacterId.Character_Sia);
            allCharacters.Add(CharacterId.Character_Nora);
            allCharacters.Add(CharacterId.Character_Flora);
        }

        public async Task<List<GameCharacter>> GetCharacters(int userId)
        {
            using (var context = new UserInfoContext())
            {
                List<GameCharacter> characters = await context.UserInfos
                    .Where(u => u.Id == userId)
                    .Select(u => u.Characters)
                    .FirstOrDefaultAsync();

                return characters;
            }
        }

        public async Task Gacha(int userId)
        {
            using (var context = new UserInfoContext())
            {
                var userData = await context.UserInfos
                    .Include(u => u.Characters)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (userData == null)
                {
                    throw new Exception("User not found");
                }
                var characterCodes = userData.Characters.Select(c => c.CharacterID).ToList();
                //가진캐릭의 여집합
                var complement = allCharacters.Except(characterCodes).ToList();
                if (complement.Count == 0)
                {
                    return;
                }
                var gachaNumber = (int)random.NextInt64(0, complement.Count - 1);
                userData.Characters.Add(new GameCharacter() { CharacterID = complement[gachaNumber] });
                await context.SaveChangesAsync();
            }
        }
    }
}
