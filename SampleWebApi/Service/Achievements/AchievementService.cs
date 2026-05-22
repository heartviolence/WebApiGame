using Assets.Scripts.Shared.GameDatas;
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

        public void PlayGainAcheivementRewardsEvent(UserAccountDetail user, GainAcheivementRewardsEvent e)
        {
            foreach(var reward in e.Rewards)
            {
                _itemService.AddItem(user, reward.Name, reward.Count);
            }
            var completedAchievement = user.CompletedAchievements.First(a => a.AchievementName == e.AchievementName);
            completedAchievement.RewardCheckPoint = e.AfterRewardCheckPoint;
        }


        List<GameItem> GetAchievementRewards(string achievementCode, int level)
        {
            return new() { new GameItem() { Id = -1, Name = ItemNames.Crystal, Count = 1 } };
        }
    }
}
