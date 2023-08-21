using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Notifications;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using System.Reactive.Subjects;

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

        collection.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<IAssemblyMarker>());

        collection.AddSingleton<Subject<QueueUpdated.Notification>>();

        collection.AddSingleton<IObservable<QueueUpdated.Notification>>(
            p => p.GetRequiredService<Subject<QueueUpdated.Notification>>());

        collection.AddSingleton<IObserver<QueueUpdated.Notification>>(
            p => p.GetRequiredService<Subject<QueueUpdated.Notification>>());

        return collection;
    }
}