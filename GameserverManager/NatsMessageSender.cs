using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Net;

namespace GameserverManager
{
    public class NatsMessageSender
    {
        NatsClient _nc;
        ILogger _logger;

        public NatsMessageSender(ILogger<NatsMessageSender> logger)
        {
            _nc = new NatsClient();
            this._logger = logger;
        }

        public async Task<string?> FindNiceServer(string username)
        {
            try
            {
                var response = await _nc.RequestAsync<string, string>(subject: "game.server.userAllocation", data: username, replyOpts: new NatsSubOpts() { Timeout = TimeSpan.FromSeconds(5) });
                return response.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return "";
        }
    }
}
