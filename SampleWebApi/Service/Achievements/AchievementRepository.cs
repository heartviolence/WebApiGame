using Assets.Scripts.Shared;
using Microsoft.EntityFrameworkCore;
using ServerShared.DbContexts;
using ServerShared.Shards;

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
            await using (var context = await GameDbUtil.CreateGameDbContext(userId))
            {
                var user = context.UserDetails
                    .Where(u => u.UserId == userId)
                    .Include(u => u.CompletedAchievements)
                    .SingleOrDefault();

                var completedAchievment = user.CompletedAchievements.Where(a => a.AchievementName == achievementName).SingleOrDefault();
                _service.GainAchievementRewards(user, completedAchievment);
                await context.SaveChangesAsync();
            }
        }
    }
}
