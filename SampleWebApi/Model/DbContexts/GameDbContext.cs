using Microsoft.EntityFrameworkCore;

namespace SampleWebApi.Model.DbContexts
{
    public class GameDbContext : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<GameCharacter> GameCharacters { get; set; }
        public DbSet<GameEvent> GameEvents { get; set; }
        public DbSet<RequestMission> RequestMissions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=UserDB;Trusted_Connection=True");
        }
    }
}
