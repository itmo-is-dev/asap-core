using Bogus;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Seeding.Extensions;
using Itmo.Dev.Asap.Core.Seeding.Options;

namespace Itmo.Dev.Asap.Core.Seeding.EntityGenerators.Submissions;

public class SubmissionGenerator : EntityGeneratorBase<SubmissionModel>
{
    private const double MaxExtraPoints = 15;
    private const float ExtraPointsPresenceProbability = 0.1f;
    private readonly IEntityGenerator<AssignmentModel> _assignmentGenerator;

    private readonly Faker _faker;

    public SubmissionGenerator(
        EntityGeneratorOptions<SubmissionModel> options,
        Faker faker,
        IEntityGenerator<AssignmentModel> assignmentGenerator)
        : base(options)
    {
        _faker = faker;
        _assignmentGenerator = assignmentGenerator;
    }

    protected override SubmissionModel Generate(int index)
    {
        IEnumerable<AssignmentModel> assignments = _assignmentGenerator.GeneratedEntities
            .Where(x => x.GroupAssignments
                .SelectMany(xx => xx.StudentGroup.Students)
                .Any());

        AssignmentModel assignment = _faker.PickRandom(assignments);

        IEnumerable<StudentModel> students = assignment.GroupAssignments
            .SelectMany(x => x.StudentGroup.Students)
            .Where(student => assignment.SubjectCourse.Mentors
                .Any(mentor => mentor.User.Equals(student.User)) is false);

        StudentModel student = _faker.PickRandom(students);

        GroupAssignmentModel groupAssignment = assignment.GroupAssignments
            .Single(x => x.StudentGroup.Equals(student.StudentGroup));

        int submissionCount = groupAssignment.Submissions.Count(x => x.Student.Equals(student));
        SubmissionStateKind stateKind = _faker.PickRandom(Enum.GetValues<SubmissionStateKind>());

        double rating = stateKind is SubmissionStateKind.Completed
            ? _faker.Random.Fraction().Value
            : 0;

        double extraPoints = stateKind is SubmissionStateKind.Completed && _faker.Random.Bool(ExtraPointsPresenceProbability)
            ? _faker.Random.Points(0, MaxExtraPoints).Value
            : 0;

        var submission = new SubmissionModel(
            _faker.Random.Guid(),
            submissionCount + 1,
            _faker.Internet.Url(),
            rating,
            extraPoints,
            Calendar.FromLocal(_faker.Date.Future()),
            student.UserId,
            groupAssignment.StudentGroupId,
            groupAssignment.AssignmentId,
            stateKind)
        {
            Student = student,
            GroupAssignment = groupAssignment,
        };

        groupAssignment.Submissions.Add(submission);

        return submission;
    }
}