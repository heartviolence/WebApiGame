using Assets.Scripts.Shared.GameDatas;
using SampleWebApi.Model.Items;
using SampleWebApi.Service.Users.Items;
using ServerShared.DbContexts;

namespace SampleWebApi.Service.Achievements
{
    public class AchievementService
    {
        GameItemService _itemService;
        public AchievementService(GameItemService itemService)
        {
            this._itemService = itemService;
        }

        public void GainAchievementRewards(UserAccountDetail user, CompletedAchievement completedAchievement)
        {
            if (completedAchievement is null ||
                completedAchievement.Level <= completedAchievement.RewardCheckPoint)
            {
                return;
            }

            var rewards = GetAchievementRewards(completedAchievement.AchievementName, completedAchievement.RewardCheckPoint + 1);
            foreach (var reward in rewards)
            {
                _itemService.AddItem(user, reward.itemName, reward.count);
            }
            completedAchievement.RewardCheckPoint += 1;
        }

        List<(string itemName, int count)> GetAchievementRewards(string achievementCode, int level)
        {
            return new() { new(SpeicalItemNames.Crystal, 1) };
        }
    }
}
