using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourseGroups;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.SubjectCourseGroups;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class DeleteSubjectCourseGroupTests : CoreDatabaseTestBase
{
    public DeleteSubjectCourseGroupTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldRemoveGroupCorrectly()
    {
        // Arrange
        SubjectCourseModel subjectCourse = await Context.SubjectCourses
            .OrderBy(x => x.Id)
            .Where(x => x.SubjectCourseGroups.Count != 0)
            .FirstAsync();

        Guid groupId = subjectCourse.SubjectCourseGroups.First().StudentGroupId;

        var command = new DeleteSubjectCourseGroup.Command(subjectCourse.Id, groupId);
        var handler = new DeleteSubjectCourseGroupHandler(PersistenceContext);

        // Act
        await handler.Handle(command, default);

        // Assert
        Context.ChangeTracker.Clear();
        subjectCourse = await Context.SubjectCourses.SingleAsync(x => x.Id.Equals(subjectCourse.Id));

        subjectCourse.SubjectCourseGroups.Should().NotContain(x => x.StudentGroupId.Equals(groupId));
    }
}