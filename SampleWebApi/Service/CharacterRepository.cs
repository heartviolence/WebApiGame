using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model.DbContexts;
using SampleWebApi.Model.Events;
using System;
using System.Runtime.CompilerServices;

namespace SampleWebApi.Service
{
    public class CharacterRepository
    {
        Random random = new();
        IGameCharacterDataProvider _gameCharacterData;
        public CharacterRepository(IGameCharacterDataProvider gameCharacterData)
        {
            this._gameCharacterData = gameCharacterData;
        }

        public async Task<List<GameCharacter>> GetCharacters(int userId)
        {
            using (var context = new GameDbContext())
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
            using (var context = new GameDbContext())
            {
                var userData = await context.UserInfos
                    .Include(u => u.Characters)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (userData == null)
                {
                    throw new Exception("User not found");
                }
                var characterCodes = userData.Characters.Select(c => c.CharacterID).ToList();

                var gachaEvent = CreateGachaEvent(userId, characterCodes);
                if (string.IsNullOrEmpty(gachaEvent.AddCharacterCode))
                {
                    return;
                }
                userData.Characters.Add(new GameCharacter() { CharacterID = gachaEvent.AddCharacterCode });
                context.GameEvents.Add(gachaEvent.CovertToGameEvent());
                await context.SaveChangesAsync();
            }
        }

        string CharacterGachaOtherOne(IEnumerable<string> characterCodes)
        {
            //가진캐릭의 여집합
            var complement = _gameCharacterData.GameCharacterData.Select(e => e.Key)
                                                .Except(characterCodes)
                                                .ToList();
            if (complement.Count == 0)
            {
                return string.Empty;
            }
            var gachaNumber = (int)random.NextInt64(0, complement.Count - 1);
            return complement[gachaNumber];
        }

        CharacterGachaEvent CreateGachaEvent(int userId, IEnumerable<string> characterCodes)
        {
            return new CharacterGachaEvent()
            {
                UserId = userId,
                AddCharacterCode = CharacterGachaOtherOne(characterCodes)
            };
        }
    }
}
