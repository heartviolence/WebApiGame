using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model.Characters;
using SampleWebApi.Service.Characters;
using ServerShared.DbContexts;
using ServerShared.Events;
using ServerShared.Shards;

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

        public async Task<UserAccountDetail> GetUserInfo(int userId)
        {
            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                return await context.UserDetails
                    .Where(u => u.UserId == userId)
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
            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                List<GameCharacter> characters = await context.UserDetails
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Characters)
                    .FirstOrDefaultAsync();

                return characters;
            }
        }

        public async Task Gacha(int userId)
        {
            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var userData = await context.UserDetails
                    .Include(u => u.Characters)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

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

        bool PayGachaCrystal(UserAccountDetail user)
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
            List<UserReward> rewards;
            using (var rewardContext = new UserAccountDbContext())
            {
                rewards = await rewardContext.UserRewards.Where(reward => reward.ExpireTime > DateTime.Now).ToListAsync();
                if (rewards.Count == 0)
                {
                    return;
                }
            }

            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails
                    .Where(u => u.UserId == userId)
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
            using (var context = new UserAccountDbContext())
            {
                var user = await context.UserAccounts
                    .Where(u => u.Username == username)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    return (true, user.UserId);
                }
            }
            return (false, -1);
        }

        public async Task<bool> RegisterNewUser(string username, string password)
        {
            using (var context = new UserAccountDbContext())
            {
                var userExist = await context.UserAccounts
                    .Where(u => u.Username == username)
                    .CountAsync();

                if (userExist > 0)
                {
                    return false;
                }

                var user = new UserAccount()
                {
                    Username = username,
                    Password = password
                };

                var userCreateEvent = Create_UserCreateEvent(username);
                context.UserAccounts.Add(user);
                context.GameEvents.Add(userCreateEvent.CovertToGameEvent());
                await context.SaveChangesAsync();
            }
            return true;
        }

        UserAccountCreatedEvent Create_UserCreateEvent(string username)
        {
            return new UserAccountCreatedEvent()
            {
                Username = username
            };
        }
    }
}
