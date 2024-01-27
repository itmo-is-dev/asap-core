using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Queries;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.StudyGroups;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.StudyGroups;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class BulkGetStudyGroupsTests : CoreDatabaseTestBase
{
    public BulkGetStudyGroupsTests(CoreDatabaseFixture fixture) : base(fixture) { }

    [Fact]
    public async Task HandleAsync_ShouldReturnGroups()
    {
        // Arrange
        List<Guid> ids = await Context.StudentGroups
            .OrderBy(x => x.Id)
            .Take(Faker.Random.Int(10, 20))
            .Select(x => x.Id)
            .ToListAsync();

        var query = new BulkGetStudentGroups.Query(ids);
        var handler = new BulkGetStudentGroupsHandler(PersistenceContext);

        // Act
        BulkGetStudentGroups.Response response = await handler.Handle(query, default);

        // Assert
        response.Groups.Count.Should().Be(ids.Count);
    }
}