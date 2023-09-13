using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.SubjectCourses;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetSubjectCourseStudentsTests : CoreDatabaseTestBase
{
    public GetSubjectCourseStudentsTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldReturnSubjectCourseStudents()
    {
        // Arrange
        var subjectCourse = await Context.SubjectCourses
            .OrderBy(x => x.Id)
            .Select(subjectCourse => new
            {
                id = subjectCourse.Id,
                studentsCount = subjectCourse.SubjectCourseGroups
                    .SelectMany(x => x.StudentGroup.Students)
                    .Count(),
            })
            .Where(x => x.studentsCount != 0)
            .FirstAsync();

        var query = new GetSubjectCourseStudents.Query(subjectCourse.id, null, int.MaxValue);
        var handler = new GetSubjectCourseStudentsHandler(PersistenceContext);

        // Act
        GetSubjectCourseStudents.Response response = await handler.Handle(query, default);

        // Assert
        response.Students.Should().HaveCount(subjectCourse.studentsCount);
    }
}