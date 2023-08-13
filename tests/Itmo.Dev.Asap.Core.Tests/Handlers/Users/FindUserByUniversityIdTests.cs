using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Users.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Application.Handlers.Users;
using Itmo.Dev.Asap.Core.DataAccess.Models.UserAssociations;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Users;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class FindUserByUniversityIdTests : CoreDatabaseTestBase
{
    public FindUserByUniversityIdTests(CoreDatabaseFixture database, ITestOutputHelper output)
        : base(database, output) { }

    [Fact]
    public async Task HandleAsync_ShouldReturnCorrectUser_WhenUniversityIdExists()
    {
        // Arrange
        var user = await Context.UserAssociations
            .OfType<IsuUserAssociationModel>()
            .Select(x => new { x.UserId, x.UniversityId })
            .OrderBy(x => x.UserId)
            .FirstAsync();

        var query = new FindUserByUniversityId.Query(user.UniversityId);
        var handler = new FindUserByUniversityIdHandler(PersistenceContext);

        // Act
        FindUserByUniversityId.Response response = await handler.Handle(query, default);

        // Assert
        response.User.Should().NotBeNull().And.Subject.As<UserDto>().Id.Should().Be(user.UserId);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNull_WhenUniversityIdDoesNotExist()
    {
        // Arrange
        var query = new FindUserByUniversityId.Query(int.MinValue);
        var handler = new FindUserByUniversityIdHandler(PersistenceContext);

        // Act
        FindUserByUniversityId.Response response = await handler.Handle(query, default);

        // Assert
        response.User.Should().BeNull();
    }
}