using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

namespace Itmo.Dev.Asap.Core.Seeding.DatabaseSeeders;

public class StudentSeeder : IDatabaseSeeder
{
    private readonly IEntityGenerator<StudentModel> _generator;

    public StudentSeeder(IEntityGenerator<StudentModel> generator)
    {
        _generator = generator;
    }

    public int Priority => 1;

    public void Seed(DatabaseContext context)
    {
        context.Students.AddRange(_generator.GeneratedEntities);
    }
}