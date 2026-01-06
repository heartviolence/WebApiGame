using Microsoft.EntityFrameworkCore; 
using ServerShared.DbContexts;
using ServerShared.Events;

namespace SampleWebApi.Service
{
    public class LoginRepository
    {
        public LoginRepository()
        {
        }

        public async Task<(bool isExist, int userId)> GetUserIdFromUsername(string username)
        {
            using (var context = new GameDbContext())
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
            using (var context = new GameDbContext())
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

                user.Characters.Add(DefaultGameCharacter.Sora());
                user.Characters.Add(DefaultGameCharacter.Sia());
                user.Characters.Add(DefaultGameCharacter.Nora());
                user.Characters.Add(DefaultGameCharacter.Flora());
                var userCreateEvent = Create_UserCreateEvent(username);
                context.UserInfos.Add(user);
                context.GameEvents.Add(userCreateEvent.CovertToGameEvent());
                await context.SaveChangesAsync();
            }
            return true;
        }

        UserCreateEvent Create_UserCreateEvent(string username)
        {
            return new UserCreateEvent()
            {
                Username = username
            };
        }
    }
}
