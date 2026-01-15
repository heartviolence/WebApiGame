using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model.Characters;
using SampleWebApi.Service.Characters;
using ServerShared.DbContexts;
using ServerShared.Events;

namespace SampleWebApi.Service.Users
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
                    .Include(u => u.CompletedAchievements)
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
        public async Task UserRewardsToMailBox(int userId)
        {
            using (var context = new GameDbContext())
            {
                var rewards = (await context.UserRewards.ToListAsync())
                    .Where(reward => reward.ExpireTime > DateTime.Now);
                var user = context.UserInfos
                    .Where(u => u.Id == userId)
                    .SingleOrDefault();

                foreach (var reward in rewards)
                {
                    if (!user.ReceievedUserRewards.Contains(reward.Id))
                    {
                        user.MailBox.Add(new UserMail()
                        {
                            Description = reward.Description,
                            ExpireTime = reward.ExpireTime,
                            Items = reward.Items,
                            Name = reward.Name,
                        });

                        user.ReceievedUserRewards.Add(reward.Id);
                    }
                }
                await context.SaveChangesAsync();
            }
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
