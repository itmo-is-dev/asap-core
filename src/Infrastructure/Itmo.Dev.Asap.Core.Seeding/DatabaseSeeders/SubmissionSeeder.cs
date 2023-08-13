using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

namespace Itmo.Dev.Asap.Core.Seeding.DatabaseSeeders;

public class SubmissionSeeder : IDatabaseSeeder
{
    private readonly IEntityGenerator<SubmissionModel> _generator;

    public SubmissionSeeder(IEntityGenerator<SubmissionModel> generator)
    {
        _generator = generator;
    }

    public void Seed(DatabaseContext context)
    {
        context.Submissions.AddRange(_generator.GeneratedEntities);
    }
}