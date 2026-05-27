using ReverseProxy;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<DynamicConfigProvider>();
builder.Services.AddSingleton<IProxyConfigProvider>(sp => sp.GetRequiredService<DynamicConfigProvider>());
builder.Services.AddReverseProxy();
builder.Services.AddHostedService<ServerListUpdater>();
var app = builder.Build();

app.MapReverseProxy(pipeline =>
{
    pipeline.Use((context, next) =>
    {
        if (context.Request.Headers.TryGetValue("ServerNumber", out var serverNumber))
        {
            var proxyFeature = context.Features.Get<IReverseProxyFeature>();

            var destinations = proxyFeature.AvailableDestinations;
            var target = destinations.FirstOrDefault(d => d.DestinationId.Equals($"srv_{serverNumber}"));

            if (target != null)
            {
                proxyFeature.AvailableDestinations = new[] { target };
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status502BadGateway;
                return Task.CompletedTask;
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return Task.CompletedTask;
        }

        return next();
    });
});
app.MapGet("/debug/proxy-config", (IProxyConfigProvider provider) =>
{
    var config = provider.GetConfig();
    return Results.Ok(new
    {
        Routes = config.Routes,
        Clusters = config.Clusters
    });
});

app.Run();

