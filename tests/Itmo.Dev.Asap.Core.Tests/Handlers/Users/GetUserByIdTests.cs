using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Users.Queries;
using Itmo.Dev.Asap.Core.Application.Handlers.Users;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Users;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetUserByIdTests : CoreDatabaseTestBase
{
    public GetUserByIdTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldReturnUserDto_WhenUserExists()
    {
        // Arrange
        UserModel user = await Context.Users
            .OrderBy(x => x.Id)
            .FirstAsync();

        var query = new GetUserById.Query(user.Id);
        var handler = new GetUserByIdHandler(PersistenceContext);

        // Act
        Func<Task<GetUserById.Response>> action = () => handler.Handle(query, default);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenUserNotExist()
    {
        // Arrange
        var query = new GetUserById.Query(Guid.Empty);
        var handler = new GetUserByIdHandler(PersistenceContext);

        // Act
        Func<Task<GetUserById.Response>> action = () => handler.Handle(query, default);

        // Assert
        await action.Should().ThrowAsync<EntityNotFoundException>();
    }
}