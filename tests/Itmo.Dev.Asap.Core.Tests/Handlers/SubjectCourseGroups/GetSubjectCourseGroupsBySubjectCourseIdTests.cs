using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Queries;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourseGroups;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.SubjectCourseGroups;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetSubjectCourseGroupsBySubjectCourseIdTests : CoreDatabaseTestBase
{
    public GetSubjectCourseGroupsBySubjectCourseIdTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldReturnSubjectCourseGroups()
    {
        // Arrange
        SubjectCourseModel subjectCourse = await Context.SubjectCourses
            .OrderBy(x => x.Id)
            .Where(x => x.SubjectCourseGroups.Count != 0)
            .FirstAsync();

        var query = new GetSubjectCourseGroupsBySubjectCourseId.Query(subjectCourse.Id);
        var handler = new GetSubjectCourseGroupsBySubjectCourseIdHandler(PersistenceContext);

        // Act
        GetSubjectCourseGroupsBySubjectCourseId.Response response = await handler.Handle(query, default);

        // Assert
        response.Groups.Should().HaveSameCount(subjectCourse.SubjectCourseGroups);
    }
}