using Itmo.Dev.Asap.Core.Application.Abstractions.Formatters;
using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Application.SubjectCourses;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.SubjectCourses;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CalculatePointsOfTransferredStudentTest : CoreDatabaseTestBase
{
    public CalculatePointsOfTransferredStudentTest(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task TransferStudent_CalculateSubmissions_Should_BeEqual()
    {
        AssignmentModel assignmentModel = await Context.Assignments
            .Select(x => x)
            .Include(assignmentModel => assignmentModel.GroupAssignments)
            .ThenInclude(groupAssignmentModel => groupAssignmentModel.Submissions)
            .ThenInclude(submissionModel => submissionModel.Student)
            .Include(assignmentModel => assignmentModel.SubjectCourse)
            .Include(assignmentModel => assignmentModel.GroupAssignments)
            .ThenInclude(groupAssignmentModel => groupAssignmentModel.StudentGroup)
            .ThenInclude(studentGroupModel => studentGroupModel.Students)
            .ToAsyncEnumerable()
            .Where(x =>
                x.GroupAssignments.Count > 1
                && x.GroupAssignments.Any(xx => xx.Submissions.Any(xxx => xxx.Rating != null)))
            .FirstAsync();

        StudentModel studentModel = assignmentModel.GroupAssignments
            .SelectMany(x => x.Submissions)
            .First(x => x.Rating != null)
            .Student;

        StudentGroupModel newGroupModel = assignmentModel.GroupAssignments
            .Select(x => x.StudentGroup)
            .First(xx => xx.Students.Contains(studentModel) is false);

        var subjectCourseService = new SubjectCourseService(PersistenceContext, new UserFullNameFormatter());

        SubjectCoursePointsDto pointsDto = await subjectCourseService
            .CalculatePointsAsync(assignmentModel.SubjectCourse.Id, CancellationToken.None);

        int ratedSubmissionCountBefore = pointsDto.StudentsPoints
            .First(x => x.Student.User.Id == studentModel.UserId)
            .Points.Count;

        Student student = await PersistenceContext.Students
            .GetByIdAsync(studentModel.UserId, default);

        StudentGroup? oldGroup = student.Group is null
            ? null
            : await PersistenceContext.StudentGroups.GetByIdAsync(student.Group.Id, default);

        StudentGroup newGroup = await PersistenceContext.StudentGroups
            .GetByIdAsync(newGroupModel.Id, default);

        student.TransferToAnotherGroup(oldGroup, newGroup);

        pointsDto = await subjectCourseService
            .CalculatePointsAsync(assignmentModel.SubjectCourse.Id, CancellationToken.None);

        int ratedSubmissionCountAfter = pointsDto.StudentsPoints
            .First(x => x.Student.User.Id == student.UserId)
            .Points.Count;

        Assert.Equal(ratedSubmissionCountBefore, ratedSubmissionCountAfter);
    }
}