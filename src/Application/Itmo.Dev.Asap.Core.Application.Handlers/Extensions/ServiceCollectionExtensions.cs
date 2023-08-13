using Itmo.Dev.Asap.Core.Application.Contracts.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection collection)
    {
        collection.AddOptions<PaginationConfiguration>().BindConfiguration("Pagination");
        collection.AddMediatR(x => x.RegisterServicesFromAssemblyContaining(typeof(IAssemblyMarker)));

        return collection;
    }
}