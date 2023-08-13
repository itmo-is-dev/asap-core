using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

namespace Itmo.Dev.Asap.Core.Seeding.DatabaseSeeders;

public class SubjectSeeder : IDatabaseSeeder
{
    private readonly IEntityGenerator<SubjectModel> _generator;

    public SubjectSeeder(IEntityGenerator<SubjectModel> generator)
    {
        _generator = generator;
    }

    public void Seed(DatabaseContext context)
    {
        context.Subjects.AddRange(_generator.GeneratedEntities);
    }
}