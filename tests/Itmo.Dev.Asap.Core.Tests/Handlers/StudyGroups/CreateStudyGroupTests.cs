using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.StudyGroups;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using MediatR;
using Moq;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.StudyGroups;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CreateStudyGroupTests : CoreDatabaseTestBase
{
    public CreateStudyGroupTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldCreateGroup()
    {
        // Arrange
        string name = Fixture.Faker.Commerce.ProductName();

        var publisher = new Mock<IPublisher>();

        var command = new CreateStudyGroup.Command(name);
        var handler = new CreateStudyGroupHandler(PersistenceContext, publisher.Object);

        // Act
        CreateStudyGroup.Response response = await handler.Handle(command, default);

        // Assert
        response.Group.Name.Should().Be(name);
    }
}