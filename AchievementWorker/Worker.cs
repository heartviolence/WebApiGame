using Microsoft.EntityFrameworkCore;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;
using ServerShared.DbContexts;
using ServerShared.Events;
using System.Text.Json;

namespace AchievementWorker
{
    public class Worker(ILogger<Worker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ConnectNats();
        }

        async Task ConnectNats()
        {
            NatsClient nc = new NatsClient();
            INatsJSContext js = nc.CreateJetStreamContext();

            await js.CreateStreamAsync(new StreamConfig(name: nameof(GameEvent), subjects: [$"game.GameEvent"]));
            INatsJSConsumer consumer = await js.CreateOrUpdateConsumerAsync(stream: nameof(GameEvent), new ConsumerConfig("worker1"));

            await foreach (NatsJSMsg<GameEvent> msg in consumer.ConsumeAsync<GameEvent>())
            {
                GameEvent data = msg.Data;
                await ProcessEvent(data);
                await msg.AckAsync();
            }
        }

        async Task ProcessEvent(GameEvent e)
        {
            Console.WriteLine($"{e.Id},type:{e.EventType}");

            switch (e.EventType)
            {
                case nameof(CharacterGachaEvent):
                    var gachaEvent = JsonSerializer.Deserialize<CharacterGachaEvent>(e.Payload);
                    await GachaAchievement(gachaEvent);
                    break;
                default:
                    break;
            }
        }

        async Task GachaAchievement(CharacterGachaEvent e)
        {
            using (var context = new GameDbContext())
            {
                var user = context.UserInfos.Where(u => u.Id == e.UserId)
                    .Include(e => e.AchievementData)
                    .Include(e => e.CompletedAchievements)
                    .SingleOrDefault();

                user.AchievementData.GachaCount += 1;
                new GachaGameAchievement().Check(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
