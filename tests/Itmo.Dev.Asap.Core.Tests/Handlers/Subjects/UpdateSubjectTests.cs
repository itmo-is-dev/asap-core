using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.Subjects;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Subjects;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class UpdateSubjectTests : CoreDatabaseTestBase
{
    public UpdateSubjectTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldUpdateSubject()
    {
        // Arrange
        SubjectModel subject = await Context.Subjects
            .OrderBy(x => x.Id)
            .FirstAsync();

        string newTitle = subject.Title + "_new";

        var command = new UpdateSubject.Command(subject.Id, newTitle);
        var handler = new UpdateSubjectHandler(PersistenceContext);

        // Act
        await handler.Handle(command, default);

        // Assert
        Context.ChangeTracker.Clear();
        subject = await Context.Subjects.SingleAsync(x => x.Id.Equals(subject.Id));

        subject.Title.Should().Be(newTitle);
    }
}