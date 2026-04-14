using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.DbContexts
{
    public class UserAccountDbContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<GrantItem> GrantItems { get; set; } //나중에 db분리
        public DbSet<GameEvent> GameEvents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=UserAccountDB;Trusted_Connection=True")
                .LogTo(Log.Logger.Information, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }
    }
}
