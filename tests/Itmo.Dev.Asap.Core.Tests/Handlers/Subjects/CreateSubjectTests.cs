using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.Subjects;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Subjects;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CreateSubjectTests : CoreDatabaseTestBase
{
    public CreateSubjectTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldCreateSubject()
    {
        // Arrange
        var command = new CreateSubject.Command(Faker.Commerce.ProductName());
        var handler = new CreateSubjectHandler(PersistenceContext);

        // Act
        CreateSubject.Response response = await handler.Handle(command, default);

        // Assert
        int subjectCount = await Context.Subjects
            .Where(x => x.Id.Equals(response.Subject.Id))
            .CountAsync();

        subjectCount.Should().Be(1);
    }
}