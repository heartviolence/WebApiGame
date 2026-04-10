using ServerShared.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerShared.Shards
{
    public class DefaultShardResolver
    {
        public static List<string> cash = new();
        public static string Resolve(UserAccount record)
        {
            return Resolve(record.ShardNumber);
        }
        public static string Resolve(int shardNumber)
        {
            if (shardNumber == 0)
            {
                return @"Server=(localdb)\mssqllocaldb;Database=UserDB0;Trusted_Connection=True";
            }
            else
            {
                return @"Server=(localdb)\mssqllocaldb;Database=UserDB1;Trusted_Connection=True";
            }
        }

        public static async IAsyncEnumerable<string> GetConnectionStrings()
        {
            //await 
            yield return @"Server=(localdb)\mssqllocaldb;Database=UserDB0;Trusted_Connection=True";
            yield return @"Server=(localdb)\mssqllocaldb;Database=UserDB1;Trusted_Connection=True";
        }

        
    }
}
