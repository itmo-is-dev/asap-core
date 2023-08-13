using Itmo.Dev.Asap.Core.Presentation.SignalR.Hubs;

namespace Itmo.Dev.Asap.Core.Presentation.SignalR.Extensions;

public static class WebApplicationExtensions
{
    public static void UseRpcPresentation(this IEndpointRouteBuilder builder)
    {
        builder.MapHub<QueueHub>("hubs/queue");
    }
}