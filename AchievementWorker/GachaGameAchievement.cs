using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AchievementWorker
{
    public class GachaGameAchievement
    {
        List<GachaGameAchievementCondition> conditions = new();

        public GachaGameAchievement()
        {
            conditions.Add(new GachaGameAchievementCondition()
            {
                Level = 1,
                GachaCount = 5,
            });

            conditions.Add(new GachaGameAchievementCondition()
            {
                Level = 2,
                GachaCount = 10,
            });
        }

        public void Check(UserAccountDetail user)
        {
            var thisAchievement = user.CompletedAchievements.Where(e => e.AchievementName == nameof(GachaGameAchievement)).SingleOrDefault();
            if (thisAchievement == null)
            {
                thisAchievement = new CompletedAchievement()
                {
                    AchievementName = nameof(GachaGameAchievement),
                    Level = 0
                };
                user.CompletedAchievements.Add(thisAchievement);
            }

            foreach (var condition in conditions)
            {
                if (thisAchievement.Level >= condition.Level)
                {
                    continue;
                }

                //조건체크
                if (user.AchievementData.GachaCount >= condition.GachaCount)
                {
                    thisAchievement.Level = condition.Level;
                }
            }
        }
    }

    public class GachaGameAchievementCondition
    {
        public int Level { get; set; }
        public int GachaCount { get; set; }
    }
}
