using Microsoft.EntityFrameworkCore;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;
using ServerShared.DbContexts;
using ServerShared.Events;
using ServerShared.Shards;
using System.Text.Json;

namespace AchievementWorker
{
    public class Worker(ILogger<Worker> logger) : BackgroundService
    {
        NatsClient nc;
        INatsJSContext js;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ConnectNats();
        }

        async Task ConnectNats()
        {
            nc = new NatsClient();
            js = nc.CreateJetStreamContext();

            _ = UserAccountWork();

            await js.CreateStreamAsync(new StreamConfig(name: nameof(GameEvent), subjects: [$"game.GameEvent"]));
            INatsJSConsumer consumer = await js.CreateOrUpdateConsumerAsync(stream: nameof(GameEvent), new ConsumerConfig("worker1"));

            await foreach (NatsJSMsg<GameEvent> msg in consumer.ConsumeAsync<GameEvent>())
            {
                GameEvent data = msg.Data;
                await ProcessEvent(data);
                await msg.AckAsync();
            }
        }

        #region useraccount

        async Task UserAccountWork()
        {
            try
            {
                await js.CreateStreamAsync(new StreamConfig(name: nameof(UserAccountCreatedEvent), subjects: [$"game.UserAccountCreatedEvent"]));
                INatsJSConsumer consumer = await js.CreateOrUpdateConsumerAsync(stream: nameof(UserAccountCreatedEvent), new ConsumerConfig("worker1"));

                await foreach (NatsJSMsg<GameEvent> msg in consumer.ConsumeAsync<GameEvent>())
                {
                    GameEvent data = msg.Data;
                    logger.LogInformation("{data}", data.EventType);
                    await ProcessUserCreateEvent(data);
                    await msg.AckAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in UserAccountWork");
            }
        }

        async Task ProcessUserCreateEvent(GameEvent e)
        {
            if (e.EventType != nameof(UserAccountCreatedEvent))
            {
                return;
            }

            var userCreateEvent = JsonSerializer.Deserialize<UserAccountCreatedEvent>(e.Payload);
            int userId;
            await using (var accountContext = new UserAccountDbContext())
            {
                var account = await accountContext.UserAccounts.Where(a => a.Username == userCreateEvent.Username).SingleOrDefaultAsync();
                if (account == null)
                {
                    logger.LogInformation("flush OldMessage");
                    return;
                }
                userId = account.UserId;
            }

            var freeDb = await GameDbUtil.FindAvailableDbShard();
            await using (var context = new GameDbContext(GameDbUtil.CreateConnectionString(freeDb)))
            {
                var user = new UserAccountDetail()
                {
                    UserId = userId,
                    Username = userCreateEvent.Username
                };
                var detailCreatedEvent = new UserAccountDetailCreatedEvent()
                {
                    UserId = userId,
                    Username = userCreateEvent.Username,
                    ShardNumber = freeDb
                };
                context.UserDetails.Add(user);
                context.GameEvents.Add(detailCreatedEvent.CovertToGameEvent());
                await context.SaveChangesAsync();
                logger.LogInformation("UserDetailCreated. Username: {Username}, UserId: {UserId},Shard: {freeDb}.", user.Username, user.UserId, freeDb);
            }

        }
        #endregion

        async Task ProcessEvent(GameEvent e)
        {
            logger.LogInformation("eventId:{EventId},eventType:{EventType}", e.Id, e.EventType);

            switch (e.EventType)
            {
                case nameof(CharacterGachaEvent):
                    var gachaEvent = JsonSerializer.Deserialize<CharacterGachaEvent>(e.Payload);
                    await OnCharacterGacha(gachaEvent, e.Shard);
                    break;
                case nameof(UserAccountDetailCreatedEvent):
                    var userAccountDetailCreatedEvent = JsonSerializer.Deserialize<UserAccountDetailCreatedEvent>(e.Payload);
                    await OnUserAccountDetailCreated(userAccountDetailCreatedEvent);
                    break;
                default:
                    break;
            }
        }

        async Task OnCharacterGacha(CharacterGachaEvent e, int shardNumber)
        {
            await using (var context = new GameDbContext(GameDbUtil.CreateConnectionString(shardNumber)))
            {
                var user = context.UserDetails.Where(u => u.UserId == e.UserId)
                    .Include(e => e.AchievementData)
                    .Include(e => e.CompletedAchievements)
                    .SingleOrDefault();

                user.AchievementData.GachaCount += 1;
                new GachaGameAchievement().Check(user);
                await context.SaveChangesAsync();
            }
        }

        async Task OnUserAccountDetailCreated(UserAccountDetailCreatedEvent e)
        {
            await using (var context = new UserAccountDbContext())
            {
                var account = await context.UserAccounts.Where(u => u.UserId == e.UserId).SingleAsync();
                account.ShardNumber = e.ShardNumber;
                await context.SaveChangesAsync();
            }
        }
    }
}
