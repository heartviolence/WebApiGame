using Microsoft.EntityFrameworkCore;

namespace SampleWebApi.Model.DbContexts
{
    public class UserInfoContext : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=UserDB;Trusted_Connection=True");
        } 
    }
}
