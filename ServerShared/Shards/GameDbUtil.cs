using Microsoft.EntityFrameworkCore;
using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Shards
{
    public static class GameDbUtil
    {
        public static async Task<UserAccount> FindLookupRecordAsync(string username)
        {
            using (var context = new UserAccountDbContext())
            {
                return await context.UserAccounts.Where(u => u.Username == username).SingleAsync();
            }
        }

        public static async Task<UserAccount> FindLookupRecordAsync(int userId)
        {
            using (var context = new UserAccountDbContext())
            {
                return await context.UserAccounts.Where(u => u.UserId == userId).SingleAsync();
            }
        }

        public static GameDbContext CreateGameDbContext(UserAccount record)
        {
            return new GameDbContext(DefaultShardResolver.Resolve(record));
        }

        public static async Task<GameDbContext> CreateGameDbContext(int userId)
        {
            return CreateGameDbContext(await FindLookupRecordAsync(userId));
        }

        public static async Task<int> FindAvailableDbShard()
        {
            //임시구현
            var dbs = await GetAllGameDbConnectionStrings().ToListAsync();

            int min = int.MaxValue;
            int shard = 0;

            for (int i = 0; i < dbs.Count; i++)
            {
                await using (var context = new GameDbContext(dbs[i]))
                {
                    var count = await context.UserDetails.CountAsync();
                    if (count < min)
                    {
                        min = count;
                        shard = i;
                    }
                }
            }
            return shard;
        }

        public static string CreateConnectionString(int shardNumber)
        {
            return DefaultShardResolver.Resolve(shardNumber);
        }

        public static IAsyncEnumerable<string> GetAllGameDbConnectionStrings()
        {
            return DefaultShardResolver.GetConnectionStrings();
        }
    }
}
