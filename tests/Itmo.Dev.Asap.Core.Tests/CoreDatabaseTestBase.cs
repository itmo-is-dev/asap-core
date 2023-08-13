using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.Seeding.Extensions;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Itmo.Dev.Asap.Core.Tests;

public class CoreDatabaseTestBase : CoreTestBase, IAsyncLifetime
{
    private AsyncServiceScope _scope;

    protected CoreDatabaseTestBase(CoreDatabaseFixture fixture, ITestOutputHelper? output = null) : base(output)
    {
        Fixture = fixture;
    }

    protected DatabaseContext Context { get; private set; } = null!;

    protected IPersistenceContext PersistenceContext { get; private set; } = null!;

    protected CoreDatabaseFixture Fixture { get; }

    public async Task InitializeAsync()
    {
        _scope = Fixture.CreateAsyncScope();

        await _scope.UseDatabaseSeeders<TestDatabaseContext>();
        Context = _scope.ServiceProvider.GetRequiredService<TestDatabaseContext>();
        PersistenceContext = _scope.ServiceProvider.GetRequiredService<IPersistenceContext>();
    }

    public async Task DisposeAsync()
    {
        await Fixture.ResetAsync();
        await _scope.DisposeAsync();
    }

    protected T GetRequiredService<T>() where T : class
    {
        return _scope.ServiceProvider.GetRequiredService<T>();
    }
}