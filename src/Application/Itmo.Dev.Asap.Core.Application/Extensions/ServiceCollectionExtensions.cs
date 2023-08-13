using FluentChaining;
using FluentChaining.Configurators;
using Itmo.Dev.Asap.Core.Application.Abstractions.Formatters;
using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.Abstractions.Queue;
using Itmo.Dev.Asap.Core.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Abstractions.Submissions;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Querying;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Application.Queries;
using Itmo.Dev.Asap.Core.Application.Queries.Adapters;
using Itmo.Dev.Asap.Core.Application.Queries.Requests;
using Itmo.Dev.Asap.Core.Application.Queue;
using Itmo.Dev.Asap.Core.Application.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Submissions.Workflows;
using Itmo.Dev.Asap.Core.Application.Users;
using Itmo.Dev.Asap.Core.Application.Validators;
using Itmo.Dev.Asap.Core.Application.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Core.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection collection)
    {
        collection.AddScoped<IPermissionValidator, PermissionValidator>();
        collection.AddScoped<ISubjectCourseService, SubjectCourseService>();
        collection.AddScoped<ISubmissionWorkflowService, SubmissionWorkflowService>();
        collection.AddScoped<IQueueService, QueueService>();

        collection.AddSingleton<IUserFullNameFormatter, UserFullNameFormatter>();

        collection.AddQueryChains();
        collection.AddFilterChains();

        collection.AddCurrentUser();

        collection.AddUpdateWorkers();

        return collection;
    }

    private static void AddCurrentUser(this IServiceCollection collection)
    {
        collection.AddScoped<CurrentUserProxy>();
        collection.AddScoped<ICurrentUser>(x => x.GetRequiredService<CurrentUserProxy>());
        collection.AddScoped<ICurrentUserManager>(x => x.GetRequiredService<CurrentUserProxy>());
    }

    private static void AddUpdateWorkers(this IServiceCollection collection)
    {
        collection.AddSingleton<QueueUpdater>();
        collection.AddSingleton<IQueueUpdateService>(x => x.GetRequiredService<QueueUpdater>());

        collection.AddSingleton<SubjectCourseUpdater>();
        collection.AddSingleton<ISubjectCourseUpdateService>(x => x.GetRequiredService<SubjectCourseUpdater>());

        collection.AddHostedService<QueueUpdateWorker>();
        collection.AddHostedService<SubjectCoursePointsUpdateWorker>();
    }

    private static void AddQueryChains(this IServiceCollection collection)
    {
        collection.AddEntityQuery<StudentQuery.Builder, StudentQueryParameter>();
        collection.AddEntityQuery<StudentGroupQuery.Builder, GroupQueryParameter>();
        collection.AddEntityQuery<UserQuery.Builder, UserQueryParameter>();

        collection
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddQueryChain<StudentQuery.Builder, StudentQueryParameter>()
            .AddQueryChain<StudentGroupQuery.Builder, GroupQueryParameter>()
            .AddQueryChain<UserQuery.Builder, UserQueryParameter>();
    }

    private static IChainConfigurator AddQueryChain<TValue, TParameter>(this IChainConfigurator configurator)
    {
        return configurator.AddChain<EntityQueryRequest<TValue, TParameter>, TValue>(x => x
            .ThenFromAssemblies(typeof(IAssemblyMarker))
            .FinishWith((r, _) => r.QueryBuilder));
    }

    private static void AddEntityQuery<TValue, TParameter>(this IServiceCollection collection)
    {
        collection.AddSingleton<IEntityQuery<TValue, TParameter>, EntityQueryAdapter<TValue, TParameter>>();
    }

    private static void AddFilterChains(this IServiceCollection collection)
    {
        collection.AddEntityFilter<StudentDto, StudentQueryParameter>();

        collection
            .AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddFilterChain<StudentDto, StudentQueryParameter>();
    }

    private static IChainConfigurator AddFilterChain<TValue, TParameter>(this IChainConfigurator configurator)
    {
        return configurator.AddChain<EntityFilterRequest<TValue, TParameter>, IEnumerable<TValue>>(x => x
            .ThenFromAssemblies(typeof(IAssemblyMarker))
            .FinishWith((r, _) => r.Data));
    }

    private static void AddEntityFilter<TValue, TParameter>(this IServiceCollection collection)
    {
        collection.AddSingleton<IEntityFilter<TValue, TParameter>, EntityFilterAdapter<TValue, TParameter>>();
    }
}