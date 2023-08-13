using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Itmo.Dev.Asap.Core.Tests.Fixtures;

public class TestDatabaseContext : DatabaseContext
{
    public TestDatabaseContext(DbContextOptions<TestDatabaseContext> options) : base(options) { }
}