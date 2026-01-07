using Microsoft.EntityFrameworkCore;
using ServerShared.DbContexts;
using ServerShared.Events;
using System;
using System.Runtime.CompilerServices;

namespace SampleWebApi.Service
{
    public class UserRepository
    {
        Random random = new();
        IGameCharacterDataProvider _gameCharacterData;
        public UserRepository(IGameCharacterDataProvider gameCharacterData)
        {
            this._gameCharacterData = gameCharacterData;
        }

        public async Task<UserInfo> GetUserInfo(int userId)
        {
            using (var context = new GameDbContext())
            {
                return await context.UserInfos
                    .Where(u => u.Id == userId)
                    .Include(u => u.Characters)
                    .Include(u => u.RequestMissions)
                    .Include(u => u.GameItems)
                    .Include(u => u.Records)
                    .FirstOrDefaultAsync();
            }
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

                if (!PayGachaCrystal(userData))
                {
                    return;
                }
                var characterCodes = userData.Characters.Select(c => c.Name).ToList();

                var gachaEvent = CreateGachaEvent(userId, characterCodes);
                if (string.IsNullOrEmpty(gachaEvent.AddCharacterCode))
                {
                    return;
                }
                userData.Characters.Add(DefaultGameCharacter.Create(gachaEvent.AddCharacterCode));
                context.GameEvents.Add(gachaEvent.CovertToGameEvent());
                await context.SaveChangesAsync();
            }
        }

        bool PayGachaCrystal(UserInfo user)
        {
            int gachaPay = 10;
            if (user.Crystal < gachaPay)
            {
                return false;
            }
            user.Crystal -= gachaPay;
            return true;
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
