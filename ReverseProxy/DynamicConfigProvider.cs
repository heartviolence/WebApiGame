using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;

namespace ReverseProxy
{
    public class DynamicConfigProvider : IProxyConfigProvider
    {
        private ProxyConfig _config;
        public DynamicConfigProvider()
        {
            _config = new ProxyConfig(new List<RouteConfig>(), new List<ClusterConfig>());
        }

        public IProxyConfig GetConfig()
        {
            return _config;
        }

        public void UpdateConfig(List<(string key, string address)> serverLists)
        {
            var destinations = new Dictionary<string, DestinationConfig>();
            for (int i = 0; i < serverLists.Count; i++)
            {
                destinations.Add($"srv_{serverLists[i].key}", new DestinationConfig { Address = serverLists[i].address });
            }

            var cluster = new ClusterConfig()
            {
                ClusterId = "cluster1",
                Destinations = destinations
            };

            var route = new RouteConfig()
            {
                RouteId = "route1",
                ClusterId = "cluster1",
                Match = new RouteMatch { Path = "{**catch-all}" }
            };

            var newConfig = new ProxyConfig(new[] { route }, new[] { cluster });
            var oldConfig = Interlocked.Exchange(ref _config, newConfig);
            oldConfig.Cancel();
        }
    }
}
