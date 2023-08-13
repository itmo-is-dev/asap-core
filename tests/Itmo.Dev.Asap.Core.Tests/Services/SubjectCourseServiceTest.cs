using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Abstractions.Formatters;
using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Dto.Tables;
using Itmo.Dev.Asap.Core.Application.SubjectCourses;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Services;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class SubjectCourseServiceTest : CoreDatabaseTestBase
{
    public SubjectCourseServiceTest(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task CalculatePointsAsync_Should_ReturnPoints()
    {
        // Arrange
        SubjectCourseModel course = await Context.SubjectCourses
            .Where(x => x.Assignments
                .SelectMany(xx => xx.GroupAssignments)
                .SelectMany(xx => xx.Submissions)
                .Any())
            .FirstAsync();

        SubjectCourseService service = CreateService();

        // Act
        SubjectCoursePointsDto points = await service.CalculatePointsAsync(course.Id, default);

        // Assert
        points.StudentsPoints.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CalculatePointsAsync_Should_ReturnUniqueAssignmentIds()
    {
        // Arrange
        SubjectCourseModel course = await Context.SubjectCourses
            .Where(x => x.Assignments
                .SelectMany(xx => xx.GroupAssignments)
                .SelectMany(xx => xx.Submissions)
                .Any())
            .FirstAsync();

        SubjectCourseService service = CreateService();

        // Act
        SubjectCoursePointsDto points = await service.CalculatePointsAsync(course.Id, default);

        // Assert
        IReadOnlyList<StudentPointsDto> studentPoints = points.StudentsPoints;

        int uniqueStudentPoints = studentPoints
            .Select(x => x.Points)
            .Count(x => x.Select(xx => xx.AssignmentId).Distinct().Count() == x.Count);

        Assert.Equal(uniqueStudentPoints, studentPoints.Count);
    }

    public SubjectCourseService CreateService()
    {
        return new SubjectCourseService(PersistenceContext, new UserFullNameFormatter());
    }
}