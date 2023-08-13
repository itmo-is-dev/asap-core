using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

namespace Itmo.Dev.Asap.Core.Seeding.DatabaseSeeders;

public class SubjectCourseSeeder : IDatabaseSeeder
{
    private readonly IEntityGenerator<SubjectCourseModel> _generator;

    public SubjectCourseSeeder(IEntityGenerator<SubjectCourseModel> generator)
    {
        _generator = generator;
    }

    public void Seed(DatabaseContext context)
    {
        context.SubjectCourses.AddRange(_generator.GeneratedEntities);
    }
}