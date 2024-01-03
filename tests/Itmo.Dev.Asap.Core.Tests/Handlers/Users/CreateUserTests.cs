using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Users;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Users;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CreateUserTests : CoreDatabaseTestBase
{
    public CreateUserTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldCreateUser()
    {
        // Arrange
        var command = new CreateUser.Command(
            Faker.Name.FirstName(),
            Faker.Internet.UserName(),
            Faker.Name.LastName());

        var handler = new CreateUserHandler(PersistenceContext);

        // Act
        CreateUser.Response response = await handler.Handle(command, default);

        // Assert
        int userCount = await Context.Users
            .Where(x => x.Id.Equals(response.User.Id))
            .CountAsync();

        userCount.Should().Be(1);
    }
}