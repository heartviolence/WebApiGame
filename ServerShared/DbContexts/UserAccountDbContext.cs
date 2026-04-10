using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.DbContexts
{
    public class UserAccountDbContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserReward> UserRewards { get; set; } //나중에 db분리
        public DbSet<GameEvent> GameEvents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=UserAccountDB;Trusted_Connection=True");
        }
    }
}
