using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Queries;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.Assignments;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Assignments;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetAssignmentsBySubjectCourseTests : CoreDatabaseTestBase
{
    public GetAssignmentsBySubjectCourseTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldReturnCorrectNumberOfAssignments()
    {
        // Arrange
        SubjectCourseModel subjectCourse = await Context.SubjectCourses
            .OrderBy(x => x.Id)
            .Where(x => x.Assignments.Count != 0)
            .FirstAsync();

        var query = new GetAssignmentsBySubjectCourse.Query(subjectCourse.Id);
        var handler = new GetAssignmentsBySubjectCourseHandler(PersistenceContext);

        // Act
        GetAssignmentsBySubjectCourse.Response response = await handler.Handle(query, default);

        // Assert
        response.Assignments.Should().HaveSameCount(subjectCourse.Assignments);
    }
}