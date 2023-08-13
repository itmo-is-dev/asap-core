using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Students;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Itmo.Dev.Platform.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Students;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CreateStudentTest : CoreDatabaseTestBase, IAsyncDisposeLifetime
{
    public CreateStudentTest(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task Handle_Should_NotThrow()
    {
        Guid groupId = await Context.StudentGroups
            .Select(x => x.Id)
            .FirstAsync();

        var command = new CreateStudent.Command("A", "B", "C", groupId);
        var handler = new CreateStudentHandler(PersistenceContext);

        CreateStudent.Response response = await handler.Handle(command, default);

        response.Student.Should().NotBeNull();
    }
}