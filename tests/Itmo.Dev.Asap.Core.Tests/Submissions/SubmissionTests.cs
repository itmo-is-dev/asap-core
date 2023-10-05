using FluentAssertions;
using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePenalties;
using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePolicies;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Domain.Submissions.States;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Domain.ValueObject;
using Itmo.Dev.Asap.Core.Tests.Extensions;
using Itmo.Dev.Platform.Testing;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Submissions;

public class SubmissionTests : TestBase
{
    [Fact]
    public void CalculateRatedSubmission_ShouldReturnZeroPoints_WhenDeadlineIsUnixEpoch()
    {
        // Arrange
        Guid groupId = Faker.Random.Guid();
        Guid assignmentId = Faker.Random.Guid();

        var groupInfo = new StudentGroupInfo(Faker.Random.Guid(), Faker.Internet.UserName());
        var assignmentInfo = new AssignmentInfo(assignmentId, "lab-1", "lab-1");

        var assignment = new Assignment(
            assignmentInfo.Id,
            assignmentInfo.Title,
            assignmentInfo.ShortName,
            1,
            0,
            10,
            Faker.Random.Guid());

        var penalties = Enumerable
            .Range(0, 5)
            .Select<int, DeadlinePenalty>(
                i => new FractionDeadlinePenalty(TimeSpan.FromDays(7) * i, 1 - (0.2 * (i + 1))))
            .ToHashSet();

        var policy = new DeadlinePolicy(penalties);

        var student = new Student(Faker.User(), groupInfo);

        var groupAssignment = new GroupAssignment(
            new GroupAssignmentId(groupId, assignmentId),
            DateOnly.FromDateTime(DateTime.UnixEpoch),
            groupInfo,
            assignmentInfo);

        var submission = new Submission(
            Faker.Random.Guid(),
            1,
            student,
            SpbDateTime.FromDateOnly(Faker.Date.FutureDateOnly()),
            string.Empty,
            groupAssignment,
            Fraction.FromDenormalizedValue(100),
            0,
            new CompletedSubmissionState());

        // Act
        RatedSubmission rated = submission.CalculateRatedSubmission(assignment, policy);

        // Assert
        rated.TotalPoints.Should().Be(0);
    }
}