using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.StudyGroups;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.StudyGroups;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class UpdateStudyGroupTests : CoreDatabaseTestBase
{
    public UpdateStudyGroupTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldUpdateGroupCorrectly()
    {
        // Arrange
        Guid groupId = await Context.StudentGroups
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .FirstAsync();

        string name = Fixture.Faker.Commerce.ProductName();

        var publisher = new Mock<IPublisher>();

        var command = new UpdateStudyGroup.Command(groupId, name);
        var handler = new UpdateStudyGroupHandler(PersistenceContext, publisher.Object);

        // Act
        UpdateStudyGroup.Response response = await handler.Handle(command, default);

        // Assert
        response.Group.Name.Should().Be(name);
    }
}