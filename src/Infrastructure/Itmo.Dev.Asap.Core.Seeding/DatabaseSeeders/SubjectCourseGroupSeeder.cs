using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

namespace Itmo.Dev.Asap.Core.Seeding.DatabaseSeeders;

public class SubjectCourseGroupSeeder : IDatabaseSeeder
{
    private readonly IEntityGenerator<SubjectCourseGroupModel> _generator;

    public int Priority => 1;

    public SubjectCourseGroupSeeder(IEntityGenerator<SubjectCourseGroupModel> generator)
    {
        _generator = generator;
    }

    public void Seed(DatabaseContext context)
    {
        context.SubjectCourseGroups.AddRange(_generator.GeneratedEntities);
    }
}