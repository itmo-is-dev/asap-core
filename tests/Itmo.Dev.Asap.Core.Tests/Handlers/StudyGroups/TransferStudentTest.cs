using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Students;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.StudyGroups;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class TransferStudentTest : CoreDatabaseTestBase
{
    public TransferStudentTest(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldTransferStudentCorrectly()
    {
        // Arrange
        List<StudentGroupModel> groups = await Context.StudentGroups
            .OrderBy(x => x.Id)
            .Where(x => x.Students.Count != 0)
            .Take(2)
            .ToListAsync();

        Guid studentId = groups[0].Students.First().UserId;
        StudentGroupModel group = groups[1];

        var publisher = new Mock<IPublisher>();

        var command = new TransferStudent.Command(studentId, group.Id);
        var handler = new TransferStudentHandler(PersistenceContext, publisher.Object);

        // Act
        TransferStudent.Response response = await handler.Handle(command, default);

        // Assert
        response.Student.GroupName.Should().Be(group.Name);
    }
}