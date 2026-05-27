using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace ReverseProxy
{
    public class ProxyConfig : IProxyConfig
    {
        private readonly CancellationChangeToken _ctsToken;
        private readonly CancellationTokenSource _cts = new();

        public IReadOnlyList<RouteConfig> Routes { get; }

        public IReadOnlyList<ClusterConfig> Clusters { get; }

        public IChangeToken ChangeToken => _ctsToken;

        public ProxyConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            Routes = routes;
            Clusters = clusters;
            _ctsToken = new(_cts.Token);
        }

        public void Cancel()
        {
            _cts.Cancel();
        }
    }
}
