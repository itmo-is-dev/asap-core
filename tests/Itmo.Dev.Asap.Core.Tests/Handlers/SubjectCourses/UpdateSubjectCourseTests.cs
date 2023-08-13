using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.SubjectCourses;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class UpdateSubjectCourseTests : CoreDatabaseTestBase
{
    public UpdateSubjectCourseTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldUpdateSubjectCourse()
    {
        // Arrange
        SubjectCourseModel subjectCourse = await Context.SubjectCourses
            .OrderBy(x => x.Id)
            .FirstAsync();

        string newTitle = subjectCourse.Title + "_new";

        var command = new UpdateSubjectCourse.Command(subjectCourse.Id, newTitle);
        var handler = new UpdateSubjectCourseHandler(PersistenceContext);

        // Act
        await handler.Handle(command, default);

        // Assert
        Context.ChangeTracker.Clear();
        subjectCourse = await Context.SubjectCourses.SingleAsync(x => x.Id.Equals(subjectCourse.Id));

        subjectCourse.Title.Should().Be(newTitle);
    }
}