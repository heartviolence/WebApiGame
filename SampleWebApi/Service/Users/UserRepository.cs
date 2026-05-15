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
                    .AsSplitQuery()
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

        public async Task CharacterGacha(int userId)
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
                var beforeGachaCrystal = userData.Crystal;
                if (!PayGachaCrystal(userData))
                {
                    return;
                }
                var afterGachaCrystal = userData.Crystal;
                var characterCodes = userData.Characters.Select(c => c.Name).ToList();

                var otherOne = CharacterGachaOtherOne(characterCodes);
                if (string.IsNullOrEmpty(otherOne))
                {
                    return;
                }

                var gachaEvent = new CharacterGachaEvent()
                {
                    UserId = userId,
                    AddCharacterCode = otherOne,
                    BeforeCrystal = beforeGachaCrystal,
                    AfterCrystal = afterGachaCrystal,
                };

                userData.Characters.Add(DefaultGameCharacter.Create(otherOne));
                context.GameEvents.Add(gachaEvent.CovertToGameEvent());
                userData.RowVersion = Guid.NewGuid();
                await context.SaveChangesAsync();
            }
        }

        public async Task GrantItemToMailBox(int userId)
        {
            List<GrantItem> grantItems;
            using (var rewardContext = new UserAccountDbContext())
            {
                grantItems = await rewardContext.GrantItems.Where(reward => reward.ExpireTime > DateTime.Now).ToListAsync();
                if (grantItems.Count == 0)
                {
                    return;
                }
            }

            var grantItemIds = grantItems.Select(r => r.Id).ToList();
            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = await context.UserDetails
                    .Where(u => u.UserId == userId)
                    .Include(u => u.ReceievedGrantItem.Where(r => grantItemIds.Contains(r.GrantItemId)))
                    .SingleOrDefaultAsync();

                var excepts = grantItems.Where(r => !user.ReceievedGrantItem.Select(r => r.GrantItemId).Contains(r.Id));
                if (excepts.Count() == 0)
                {
                    return;
                }

                var gameEvent = new GrantItemToMailBoxEvent()
                {
                    UserId = userId
                };

                foreach (var item in excepts)
                {
                    user.MailBox.Add(new UserMail()
                    {
                        Description = item.Description,
                        ExpireTime = item.ExpireTime,
                        Items = item.Items,
                        Name = item.Name,
                    });
                    user.ReceievedGrantItem.Add(new ReceievedGrantItem() { GrantItemId = item.Id });
                    gameEvent.ReceievedItems.Add(item);
                }

                user.RowVersion = Guid.NewGuid();
                context.GameEvents.Add(gameEvent.CovertToGameEvent());
                await context.SaveChangesAsync();
            }
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

                var userCreateEvent = new UserAccountCreatedEvent()
                {
                    Username = username
                };
                context.UserAccounts.Add(user);
                context.GameEvents.Add(userCreateEvent.CovertToGameEvent());
                await context.SaveChangesAsync();
            }
            return true;
        }

        #region Helper

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
        #endregion
    }
}
