using Assets.Scripts.Shared;
using Microsoft.EntityFrameworkCore;
using ServerShared.DbContexts;

namespace SampleWebApi.Service.Achievements
{
    public class AchievementRepository
    {
        AchievementService _service;
        public AchievementRepository(AchievementService service)
        {
            this._service = service;
        }

        public async Task GainAcheivementRewards(int userId, string achievementName)
        {
            using (var context = new GameDbContext())
            {
                var user = context.UserInfos
                    .Where(u => u.Id == userId)
                    .Include(u => u.CompletedAchievements)
                    .SingleOrDefault();

                var completedAchievment = user.CompletedAchievements.Where(a => a.AchievementName == achievementName).SingleOrDefault();
                _service.GainAchievementRewards(user, completedAchievment);
                await context.SaveChangesAsync();
            }
        }
    }
}
