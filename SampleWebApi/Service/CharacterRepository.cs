using Microsoft.EntityFrameworkCore;
using ServerShared.DbContexts;
using System.Threading.Tasks;

namespace SampleWebApi.Service
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
            using (var context = new GameDbContext())
            {
                var user = context.UserInfos
                    .Where(u => u.Id == userId)
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
            using (var context = new GameDbContext())
            {
                var user = context.UserInfos
                    .Where(u => u.Id == userId)
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
