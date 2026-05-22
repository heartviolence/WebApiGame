using Assets.Scripts.Shared.GameDatas;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Model.Characters;
using SampleWebApi.Service.Characters;
using ServerShared.DbContexts;
using ServerShared.Events;
using ServerShared.Shards;
using ServerShared.Util;

namespace SampleWebApi.Service.Users
{
    public class UserRepository
    {
        Random random = new();
        IGameCharacterDataProvider _gameCharacterData;
        ILogger _logger;
        public UserRepository(IGameCharacterDataProvider gameCharacterData, ILogger<UserRepository> logger)
        {
            this._gameCharacterData = gameCharacterData;
            _logger = logger;
        }

        public async Task<UserAccountDetail> GetUserInfo(int userId)
        {
            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                return await context.UserDetails
                    .Where(u => u.UserId == userId)
                    .Include(u => u.Characters)
                    .Include(u => u.AchievementData)
                    .Include(u => u.RequestMissions)
                    .Include(u => u.GameItems)
                    .Include(u => u.Records)
                    .Include(u => u.CompletedAchievements)
                    .Include(u => u.MailBox)
                    .ThenInclude(m => m.Items)
                    .Include(u => u.ReceievedGrantItem)
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

        public async Task<int> CharacterGacha(int userId)
        {
            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var userData = await context.UserDetails
                    .Include(u => u.Characters)
                    .Include(u => u.GameItems.Where(i => i.Name == ItemNames.Crystal))
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (userData == null)
                {
                    throw new Exception("User not found");
                }
                var beforeGachaCrystal = userData.Crystal();
                if (!PayGachaCrystal(userData))
                {
                    _logger.LogInformation("Gacha처리 실패,크리스탈부족");
                    return 1;
                }
                var afterGachaCrystal = userData.Crystal();
                var characterCodes = userData.Characters.Select(c => c.Name).ToList();

                var otherOne = CharacterGachaOtherOne(characterCodes);
                if (string.IsNullOrEmpty(otherOne))
                {
                    _logger.LogInformation("Gacha처리 실패,모든캐릭보유중");
                    return 2;
                }

                var gachaEvent = new CharacterGachaEvent()
                {
                    UserId = userId,
                    AddCharacterCode = otherOne,
                    BeforeCrystal = beforeGachaCrystal.Count,
                    AfterCrystal = afterGachaCrystal.Count,
                };

                userData.Characters.Add(DefaultGameCharacter.Create(otherOne));
                context.GameEvents.Add(gachaEvent.CovertToGameEvent());
                userData.RowVersion = Guid.NewGuid();
                await context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task GrantItemToMailBox(int userId)
        {
            List<GrantItem> grantItems;
            using (var rewardContext = new UserAccountDbContext())
            {
                grantItems = await rewardContext.GrantItems
                    .Where(i => i.ExpireTime > DateTime.Now)
                    .Include(i => i.Items)
                    .ToListAsync();
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

                var excepts = grantItems.Where(r => !user.ReceievedGrantItem.Select(r => r.GrantItemId).Contains(r.Id)).ToList();
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
                    item.Items.ForEach(i => i.Id = 0);
                    gameEvent.ReceievedItems.Add(item);
                }

                PlayGrantItemToMailBoxEvent(user, gameEvent);

                user.RowVersion = Guid.NewGuid();
                context.GameEvents.Add(gameEvent.CovertToGameEvent());
                await context.SaveChangesAsync();
            }
        }

        public void PlayGrantItemToMailBoxEvent(UserAccountDetail user, GrantItemToMailBoxEvent e)
        {
            foreach (var item in e.ReceievedItems)
            {
                user.ReceievedGrantItem.Add(new ReceievedGrantItem() { GrantItemId = item.Id });
                user.MailBox.Add(new UserMail()
                {
                    Description = item.Description,
                    ExpireTime = item.ExpireTime,
                    Items = item.Items,
                    Name = item.Name,
                });
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

        public async Task<UserAccountDetail> SnapShotData(int userId)
        {
            using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var userData = await context.UserDetails
                    .Where(u => u.UserId == userId)
                    .Include(u => u.Characters)
                    .Include(u => u.RequestMissions)
                    .Include(u => u.CompletedAchievements)
                    .Include(u => u.AchievementData)
                    .Include(u => u.GameItems)
                    .Include(u => u.Records)
                    .Include(u => u.MailBox)
                    .ThenInclude(m => m.Items)
                    .Include(u => u.ReceievedGrantItem)
                    .AsSplitQuery()
                    .SingleOrDefaultAsync();

                if (userData == null)
                {
                    throw new Exception("User not found");
                }

                var snapShotEvent = new UserSnapshotEvent()
                {
                    UserData = userData
                };
                context.GameEvents.Add(snapShotEvent.CovertToGameEvent());
                userData.RowVersion = Guid.NewGuid();
                await context.SaveChangesAsync();
                return userData;
            }
        }

        #region Helper

        bool PayGachaCrystal(UserAccountDetail user)
        {
            var crystal = user.Crystal();
            int gachaPay = 10;
            if (crystal.Count < gachaPay)
            {
                return false;
            }
            crystal.Count -= gachaPay;
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
