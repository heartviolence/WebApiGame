using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;
using ServerShared.DbContexts;


namespace EventCollector
{
    public class Worker(ILogger<Worker> logger) : BackgroundService
    {
        NatsClient nc;
        INatsJSContext js;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            nc = new NatsClient();
            js = nc.CreateJetStreamContext();

            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishEvent();
                await Task.Delay(3000, stoppingToken);
            }
        }

        async Task PublishEvent()
        {
            logger.LogInformation("{DateTime} : Event Collect", DateTimeOffset.Now);
            using (var context = new GameDbContext())
            {
                var events = context.GameEvents.Take(100).ToList();

                foreach (var e in events)
                {
                    PubAckResponse ack = await js.PublishAsync($"game.GameEvent", e);
                    ack.EnsureSuccess();
                }
                context.GameEvents.RemoveRange(events);
                await context.SaveChangesAsync();
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await nc.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
