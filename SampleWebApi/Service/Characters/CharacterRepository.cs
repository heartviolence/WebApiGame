using Microsoft.EntityFrameworkCore;
using ServerShared.DbContexts;
using ServerShared.Shards;

namespace SampleWebApi.Service.Characters
{
    public class CharacterRepository
    {
        CharacterService _service;
        public CharacterRepository(CharacterService service)
        {
            _service = service;
        }

        public async Task<GameCharacter> UseLevelUpItem(int userId, string characterName, int itemCount)
        {
            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails
                    .Where(u => u.UserId == userId)
                    .Include(u => u.Characters)
                    .Include(u => u.GameItems)
                    .SingleOrDefault();

                var character = user.Characters.Where(c => c.Name == characterName).SingleOrDefault();
                if (user == null || character == null)
                {
                    return null;
                }

                _service.UseLevelUpItem(user, character, itemCount);
                await context.SaveChangesAsync();
                return character;
            }
        }

        public async Task<GameCharacter> RankUp(int userId, string characterName)
        {
            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails
                    .Where(u => u.UserId == userId)
                    .Include(u => u.Characters)
                    .Include(u => u.GameItems)
                    .SingleOrDefault();

                var character = user.Characters.Where(c => c.Name == characterName).SingleOrDefault();
                if (user == null || character == null)
                {
                    return null;
                }

                _service.RankUp(user, character);
                await context.SaveChangesAsync();
                return character;
            }
        }

    }
}
