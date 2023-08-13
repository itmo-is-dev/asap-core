using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

namespace Itmo.Dev.Asap.Core.Seeding.DatabaseSeeders;

public class StudentGroupSeeder : IDatabaseSeeder
{
    private readonly IEntityGenerator<StudentGroupModel> _generator;

    public StudentGroupSeeder(IEntityGenerator<StudentGroupModel> generator)
    {
        _generator = generator;
    }

    public void Seed(DatabaseContext context)
    {
        context.StudentGroups.AddRange(_generator.GeneratedEntities);
    }
}