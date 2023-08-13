using Itmo.Dev.Asap.Core.DataAccess.Contexts;

namespace Itmo.Dev.Asap.Core.Seeding.DatabaseSeeders;

public interface IDatabaseSeeder
{
    int Priority => 0;

    void Seed(DatabaseContext context);
}