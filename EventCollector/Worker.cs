using Microsoft.EntityFrameworkCore;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;
using ServerShared.DbContexts;
using ServerShared.Events;
using ServerShared.Shards;


namespace EventCollector
{
    public class Worker(ILogger<Worker> logger) : BackgroundService
    {
        NatsClient nc;
        INatsJSContext js;
        List<string> _connectionStrings = new();
        bool _connectionStringDirty = true;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            nc = new NatsClient();
            js = nc.CreateJetStreamContext();

            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateConnectionStrings();
                await PublishEvent();
                await Task.Delay(1000, stoppingToken);
            }
        }

        async Task UpdateConnectionStrings()
        {
            if (_connectionStringDirty == false)
            {
                return;
            }
            _connectionStrings = await GameDbUtil.GetAllGameDbConnectionStrings().ToListAsync();
        }

        async Task PublishEvent()
        {
            logger.LogInformation("{DateTime} : Event Collect", DateTimeOffset.Now);

            using (var accountContext = new UserAccountDbContext())
            {
                var events = await accountContext.GameEvents.Where(e => e.EventType == nameof(UserAccountCreatedEvent)).Take(100).ToListAsync();
                foreach (var e in events)
                {
                    PubAckResponse ack = await js.PublishAsync($"game.UserAccountCreatedEvent", e);
                    ack.EnsureSuccess();
                    logger.LogInformation("{DateTime} : UserAccountCreatedEvent", DateTimeOffset.Now);
                }
                accountContext.GameEvents.RemoveRange(events);
                await accountContext.SaveChangesAsync();
            }

            for (int i = 0; i < _connectionStrings.Count; i++)
            {
                var connectionString = _connectionStrings[i];

                using (var context = new GameDbContext(connectionString))
                {
                    var events = context.GameEvents.Take(100).ToList();

                    foreach (var e in events)
                    {
                        e.Shard = i;
                        PubAckResponse ack = await js.PublishAsync($"game.GameEvent", e);
                        ack.EnsureSuccess();
                        logger.LogInformation("{DateTime} : {EventType}", DateTimeOffset.Now, e.EventType);
                    }
                    context.GameEvents.RemoveRange(events);
                    await context.SaveChangesAsync();
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await nc.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
