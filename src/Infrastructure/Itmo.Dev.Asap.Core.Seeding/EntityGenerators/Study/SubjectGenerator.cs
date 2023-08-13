using Bogus;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Seeding.Options;

namespace Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

public class SubjectGenerator : EntityGeneratorBase<SubjectModel>
{
    private readonly Faker _faker;

    public SubjectGenerator(EntityGeneratorOptions<SubjectModel> options, Faker faker)
        : base(options)
    {
        _faker = faker;
    }

    protected override SubjectModel Generate(int index)
    {
        return new SubjectModel(_faker.Random.Guid(), _faker.Commerce.Product())
        {
            SubjectCourses = new List<SubjectCourseModel>(),
        };
    }
}