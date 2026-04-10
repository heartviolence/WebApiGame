using Microsoft.EntityFrameworkCore;
using ServerShared.DbContexts;
using ServerShared.Shards;

var connectionStrings = await GameDbUtil.GetAllGameDbConnectionStrings().ToListAsync();

foreach (var conn in connectionStrings)
{
    using (var context = new GameDbContext(conn))
    {
        await context.Database.MigrateAsync();
    }
}

using(var accountContext = new UserAccountDbContext())
{
    await accountContext.Database.MigrateAsync();
}