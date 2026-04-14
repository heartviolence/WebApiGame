using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ServerShared.DbContexts
{
    public class GameDbContext : DbContext
    {
        private readonly string _connectionString;

        public GameDbContext() : base()
        {

        }

        public GameDbContext(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public DbSet<UserAccountDetail> UserDetails { get; set; }
        public DbSet<GameCharacter> GameCharacters { get; set; }
        public DbSet<GameEvent> GameEvents { get; set; }
        public DbSet<RequestMission> RequestMissions { get; set; }
        // DbSet<UserReward> UserRewards { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=UserDB;Trusted_Connection=True");
            optionsBuilder.UseSqlServer(_connectionString)
                .LogTo(Log.Logger.Information, LogLevel.Information)
                .EnableSensitiveDataLogging(); ;
        }
    }
}
