using Assets.Scripts.Shared.GameDatas;
using SampleWebApi.Model.Items;
using SampleWebApi.Service.Users.Items;
using ServerShared.DbContexts;
using ServerShared.Events;

namespace SampleWebApi.Service.Achievements
{
    public class AchievementService
    {
        GameItemService _itemService;
        public AchievementService(GameItemService itemService)
        {
            this._itemService = itemService;
        }

        public GainAcheivementRewardsEvent? GainAchievementRewards(UserAccountDetail user, CompletedAchievement completedAchievement)
        {
            if (completedAchievement is null ||
                completedAchievement.Level <= completedAchievement.RewardCheckPoint)
            {
                return null;
            }

            var rewards = GetAchievementRewards(completedAchievement.AchievementName, completedAchievement.RewardCheckPoint + 1);
            foreach (var reward in rewards)
            {
                _itemService.AddItem(user, reward.Name, reward.Count);
            }
            completedAchievement.RewardCheckPoint += 1;
            return new GainAcheivementRewardsEvent
            {
                UserId = user.UserId,
                AchievementName = completedAchievement.AchievementName,
                Level = completedAchievement.Level,
                BeforeRewardCheckPoint = completedAchievement.RewardCheckPoint - 1,
                AfterRewardCheckPoint = completedAchievement.RewardCheckPoint,
                Rewards = rewards
            };
        }

        List<GameItem> GetAchievementRewards(string achievementCode, int level)
        {
            return new() { new GameItem() { Id = -1, Name = SpecialItemNames.Crystal, Count = 1 } };
        }
    }
}
