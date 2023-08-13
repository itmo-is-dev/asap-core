using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

namespace Itmo.Dev.Asap.Core.Seeding.DatabaseSeeders;

public class AssignmentDatabaseSeeder : IDatabaseSeeder
{
    private readonly IEntityGenerator<AssignmentModel> _generator;

    public AssignmentDatabaseSeeder(IEntityGenerator<AssignmentModel> generator)
    {
        _generator = generator;
    }

    public void Seed(DatabaseContext context)
    {
        context.Assignments.AddRange(_generator.GeneratedEntities);
    }
}