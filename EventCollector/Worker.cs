using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.EntityFrameworkCore;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;
using ServerShared.DbContexts;
using ServerShared.Events;
using ServerShared.Events.SandBox;
using ServerShared.Shards;
using System.Text.Json;


namespace EventCollector
{
    public class Worker : BackgroundService
    {
        NatsClient nc;
        INatsJSContext js;
        ElasticsearchClient _esClient;
        List<string> _connectionStrings = new();
        bool _connectionStringDirty = true;
        ILogger _logger;

        public Worker(ILogger<Worker> logger)
        {
            this._logger = logger;
            //로컬환경 하드코딩
            var esSettings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
                .CertificateFingerprint("7c3679058b70f4c6623d46fea518179eeb0e7a89eea87cca7aa7ef452ae245f1")
                .Authentication(new BasicAuthentication("elastic", "qwer1234"));
            _esClient = new ElasticsearchClient(esSettings);
        }


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
            _logger.LogInformation("{DateTime} : Event Collect", DateTimeOffset.Now);

            using (var accountContext = new UserAccountDbContext())
            {
                var events = await accountContext.GameEvents.Where(e => e.EventType == nameof(UserAccountCreatedEvent)).Take(100).ToListAsync();
                foreach (var e in events)
                {
                    await SendGameEvent(e, $"useraccountdb");
                    PubAckResponse ack = await js.PublishAsync($"game.UserAccountCreatedEvent", e);
                    ack.EnsureSuccess();
                    _logger.LogInformation("{DateTime} : UserAccountCreatedEvent", DateTimeOffset.Now);
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
                        await SendGameEvent(e, $"gamedbshard{i}");
                        PubAckResponse ack = await js.PublishAsync($"game.GameEvent", e);
                        ack.EnsureSuccess();
                        _logger.LogInformation("{DateTime} : {EventType}", DateTimeOffset.Now, e.EventType);
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


        async Task SendGameEvent(GameEvent e, string dbName)
        {
            switch (e.EventType)
            {
                case nameof(DeleteAllEvent):
                    await IndexGameEventAsync<DeleteAllEvent>(e, dbName);
                    break;
                case nameof(ShowMetheMoneyEvent):
                    await IndexGameEventAsync<ShowMetheMoneyEvent>(e, dbName);
                    break;
                case nameof(CharacterGachaEvent):
                    await IndexGameEventAsync<CharacterGachaEvent>(e, dbName);
                    break;
                case nameof(CharacterRankUpEvent):
                    await IndexGameEventAsync<CharacterRankUpEvent>(e, dbName);
                    break;
                case nameof(GainAcheivementRewardsEvent):
                    await IndexGameEventAsync<GainAcheivementRewardsEvent>(e, dbName);
                    break;
                case nameof(GetMissionRewardEvent):
                    await IndexGameEventAsync<GetMissionRewardEvent>(e, dbName);
                    break;
                case nameof(GrantItemToMailBoxEvent):
                    await IndexGameEventAsync<GrantItemToMailBoxEvent>(e, dbName);
                    break;
                case nameof(RequestMissionStartEvent):
                    await IndexGameEventAsync<RequestMissionStartEvent>(e, dbName);
                    break;
                case nameof(UseLevelUpItemEvent):
                    await IndexGameEventAsync<UseLevelUpItemEvent>(e, dbName);
                    break;
                case nameof(UserAccountCreatedEvent):
                    await IndexGameEventAsync<UserAccountCreatedEvent>(e, dbName);
                    break;
                case nameof(UserAccountDetailCreatedEvent):
                    await IndexGameEventAsync<UserAccountDetailCreatedEvent>(e, dbName);
                    break;
                case nameof(UserSnapshotEvent):
                    await IndexGameEventAsync<UserSnapshotEvent>(e, dbName);
                    break;
                default:
                    break;
            }
        }

        T GameEventDeserialize<T>(GameEvent e)
        {
            var d = JsonSerializer.Deserialize<T>(e.Payload);
            if (d == null)
            {
                throw new Exception($"gameEvent payload Deserialize Error, Id:{e.Id}, eventType:{e.EventType}, payload is null");
            }
            return d;
        }

        async Task IndexGameEventAsync<T>(GameEvent e, string dbName)
        {
            var typename = typeof(T).Name.ToLower();
            var d = GameEventDeserialize<T>(e);
            var response = await _esClient.IndexAsync(d, x => x.Index($"gameevent_{typename}_{dbName}"));
            if (!response.IsSuccess())
            {
                throw new Exception($"elasticsearch IndexAsync Response is fail, Id:{e.Id}, eventType:{e.EventType}");
            }
        }
    }
}
