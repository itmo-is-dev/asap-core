using Itmo.Dev.Asap.Core.Presentation.Grpc.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcPresentation(this IServiceCollection collection)
    {
        collection.AddGrpc(options =>
        {
            options.Interceptors.Add<AuthenticationInterceptor>();
        });

        collection.AddGrpcReflection();

        return collection;
    }
}