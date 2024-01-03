using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Commands;
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
        string name = Faker.Commerce.ProductName();

        var publisher = new Mock<IPublisher>();

        var command = new CreateStudentGroup.Command(name);
        var handler = new CreateStudyGroupHandler(PersistenceContext, publisher.Object);

        // Act
        CreateStudentGroup.Response response = await handler.Handle(command, default);

        // Assert
        response.Group.Name.Should().Be(name);
    }
}