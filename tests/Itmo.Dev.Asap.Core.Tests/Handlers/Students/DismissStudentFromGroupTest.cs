using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Students;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Itmo.Dev.Platform.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Students;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class DismissStudentFromGroupTest : CoreDatabaseTestBase, IAsyncDisposeLifetime
{
    public DismissStudentFromGroupTest(CoreDatabaseFixture database, ITestOutputHelper output)
        : base(database, output) { }

    [Fact]
    public async Task Handle_Should_NotThrow()
    {
        // Arrange
        Guid studentId = await Context.Students
            .Where(x => x.StudentGroup != null)
            .Select(x => x.UserId)
            .FirstAsync();

        var command = new DismissStudentFromGroup.Command(studentId);
        var handler = new DismissStudentFromGroupHandler(PersistenceContext);

        // Act
        Func<Task> action = () => handler.Handle(command, default);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task HandleAsync_ShouldSetStudentIdNull()
    {
        // Arrange
        Guid studentId = await Context.Students
            .Where(x => x.StudentGroup != null)
            .Select(x => x.UserId)
            .FirstAsync();

        var command = new DismissStudentFromGroup.Command(studentId);
        var handler = new DismissStudentFromGroupHandler(PersistenceContext);

        // Act
        await handler.Handle(command, default);

        // Assert
        Context.ChangeTracker.Clear();

        StudentModel student = await Context.Students
            .SingleAsync(x => x.UserId.Equals(studentId));

        student.StudentGroupId.Should().BeNull();
    }
}