using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Seeding.Options;

namespace Itmo.Dev.Asap.Core.Seeding.EntityGenerators;

public class StudentGenerator : EntityGeneratorBase<StudentModel>
{
    private readonly IEntityGenerator<StudentGroupModel> _studentGroupGenerator;
    private readonly IEntityGenerator<UserModel> _userGenerator;

    public StudentGenerator(
        EntityGeneratorOptions<StudentModel> options,
        IEntityGenerator<StudentGroupModel> studentGroupGenerator,
        IEntityGenerator<UserModel> userGenerator)
        : base(options)
    {
        _studentGroupGenerator = studentGroupGenerator;
        _userGenerator = userGenerator;
    }

    protected override StudentModel Generate(int index)
    {
        if (index >= _userGenerator.GeneratedEntities.Count)
            throw new IndexOutOfRangeException("Student count is greater than count of users.");

        UserModel user = _userGenerator.GeneratedEntities[index];

        StudentGroupModel[] groups = _studentGroupGenerator.GeneratedEntities
            .Where(x => x.Students.Any(student => student.User.Equals(user)) is false)
            .ToArray();

        int groupNumber = index % groups.Length;
        StudentGroupModel group = groups[groupNumber];

        return new StudentModel(user.Id, group.Id)
        {
            Submissions = new List<SubmissionModel>(),
            User = user,
            StudentGroup = group,
        };
    }
}