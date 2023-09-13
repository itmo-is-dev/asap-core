using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Students;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Itmo.Dev.Platform.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Students;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CreateStudentsHandlerTest : CoreDatabaseTestBase, IAsyncDisposeLifetime
{
    public CreateStudentsHandlerTest(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task Handle_Should_NotThrow()
    {
        Guid groupId = await Context.StudentGroups
            .Select(x => x.Id)
            .FirstAsync();

        var model = new CreateStudents.Command.Model("A", "B", "C", groupId, 0);
        var command = new CreateStudents.Command(new[] { model });
        var handler = new CreateStudentHandler(PersistenceContext);

        CreateStudents.Response response = await handler.Handle(command, default);

        response.Should().BeOfType<CreateStudents.Response.Success>().Which.Students.Should().NotBeEmpty();
    }
}