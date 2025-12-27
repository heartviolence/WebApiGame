using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model.DbContexts;

namespace SampleWebApi.Service
{
    public class UserService
    {
        public UserService()
        {
        }

        public async Task<(bool isExist, int userId)> GetUserIdFromUsername(string username)
        {
            using (var context = new UserInfoContext())
            {
                var user = await context.UserInfos
                    .Where(u => u.Username == username)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    return (true, user.Id);
                }
            }
            return (false, -1);
        }

        public async Task<bool> RegisterNewUser(string username, string password)
        {
            using (var context = new UserInfoContext())
            {
                var userExist = await context.UserInfos
                    .Where(u => u.Username == username)
                    .CountAsync();

                if (userExist > 0)
                {
                    return false;
                }

                var user = new UserInfo()
                {
                    Username = username,
                    Password = password
                };

                user.Characters.Add(new GameCharacter() { CharacterID = CharacterId.Character_Sora });
                context.UserInfos.Add(user);
                await context.SaveChangesAsync();
            }
            return true;
        }
    }
}
