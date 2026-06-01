using NATS.Net;

namespace SampleWebApi.UserHealthPings
{
    public class UserAllocationWorker : BackgroundService
    {
        UserHealthPing _users;
        NatsClient _nc;
        string serverNumber = "1";
        ILogger _logger;
        public UserAllocationWorker(UserHealthPing users, ILogger<UserAllocationWorker> logger)
        {
            this._users = users;
            _nc = new NatsClient();
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            _logger.LogInformation("UserAllocationWorker Start");
            while (!ct.IsCancellationRequested)
            {
                if (!IsServerFull())
                {
                    await UserAllocationAccept(ct);
                }
                await Task.Delay(1000);
            }
        }

        async Task UserAllocationAccept(CancellationToken ct)
        {
            _logger.LogInformation("userAllocationAccept Start");
            await foreach (var msg in _nc.SubscribeAsync<string>(subject: "game.server.userAllocation", queueGroup: "gameserverUserAllocation", cancellationToken: ct))
            {
                _users.Lives[msg.Data] = DateTime.Now + TimeSpan.FromSeconds(30);
                await msg.ReplyAsync(data: serverNumber, cancellationToken: ct);
                _logger.LogInformation("user Accepted,username :{Username},CurrentUserCount :{CurrentUserCount}", msg.Data, _users.Lives.Count);
                if (IsServerFull())
                {
                    return;
                }
            }
        }

        bool IsServerFull()
        {
            return _users.Lives.Count >= 50;
        }

    }
}
