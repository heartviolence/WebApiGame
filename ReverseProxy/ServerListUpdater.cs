using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;
using Yarp.ReverseProxy.Configuration;

namespace ReverseProxy
{
    public class ServerListUpdater : BackgroundService
    {
        private readonly DynamicConfigProvider _provider;
        public ServerListUpdater(IProxyConfigProvider provider)
        {
            _provider = (DynamicConfigProvider)provider;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var servers = new List<(string key, string address)> { new("0", "https://localhost:7067"), new("1", "https://localhost:7067") };

                _provider.UpdateConfig(servers);
                Console.WriteLine($"ServerList Updated");
                await Task.Delay(TimeSpan.FromSeconds(5), ct);
            }
        }
    }
}
