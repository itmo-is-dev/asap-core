using Itmo.Dev.Asap.Core.Application.Extensions;
using Itmo.Dev.Asap.Core.DataAccess.Extensions;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Seeding.Extensions;
using Itmo.Dev.Asap.Core.Seeding.Options;
using Itmo.Dev.Platform.Testing.Fixtures;
using Itmo.Dev.Platform.Testing.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Respawn;
using Respawn.Graph;
using Serilog;

namespace Itmo.Dev.Asap.Core.Tests.Fixtures;

public class CoreDatabaseFixture : DatabaseFixture
{
    public AsyncServiceScope CreateAsyncScope()
    {
        return Provider.CreateAsyncScope();
    }

    protected override void ConfigureServices(IServiceCollection collection)
    {
        collection.AddDataAccess(x => x
            .UseNpgsql(Container.GetConnectionString() + ";Include Error Detail = true;")
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .UseLoggerFactory(LoggerFactory.Create(b => b.AddSerilog(new StaticLogger()).AddConsole())));

        collection.AddDbContext<TestDatabaseContext>(builder => builder
            .UseNpgsql(Container.GetConnectionString() + ";Include Error Detail = true;")
            .UseLazyLoadingProxies()
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .UseLoggerFactory(LoggerFactory.Create(b => b.AddSerilog(new StaticLogger()).AddConsole()))
            .ConfigureWarnings(x => x.Ignore(CoreEventId.NavigationBaseIncludeIgnored)));

        collection.AddApplicationConfiguration();

        collection.AddEntityGenerators(x =>
        {
            x.ConfigureEntityGenerator<SubmissionModel>(xx => xx.Count = 1000);
            x.ConfigureEntityGenerator<SubjectCourseModel>(xx => xx.Count = 1);
            x.ConfigureEntityGenerator<StudentModel>(xx => xx.Count = 50);
            x.ConfigureEntityGenerator<UserModel>(xx => xx.Count = 100);

            ConfigureSeeding(x);
        });

        collection.AddDatabaseSeeders();
    }

    protected virtual void ConfigureSeeding(EntityGenerationOptions options) { }

    protected override async ValueTask UseProviderAsync(IServiceProvider provider)
    {
        await using AsyncServiceScope asyncScope = provider.CreateAsyncScope();
        await asyncScope.UseDatabaseContext();
    }

    protected override RespawnerOptions GetRespawnOptions()
    {
        return new RespawnerOptions
        {
            SchemasToInclude = new[] { "public" },
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = new[] { new Table("__EFMigrationsHistory") },
        };
    }
}