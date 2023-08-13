using Bogus;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Seeding.Options;

namespace Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

public class StudentGroupGenerator : EntityGeneratorBase<StudentGroupModel>
{
    private const int MinGroupNumber = 10000;
    private const int MaxGroupNumber = 100000;

    private readonly Faker _faker;

    public StudentGroupGenerator(EntityGeneratorOptions<StudentGroupModel> options, Faker faker)
        : base(options)
    {
        _faker = faker;
    }

    protected override StudentGroupModel Generate(int index)
    {
        int groupNumber = _faker.Random.Int(MinGroupNumber, MaxGroupNumber);

        return new StudentGroupModel(_faker.Random.Guid(), $"M{groupNumber}")
        {
            Students = new List<StudentModel>(),
            SubjectCourseGroups = new List<SubjectCourseGroupModel>(),
        };
    }
}