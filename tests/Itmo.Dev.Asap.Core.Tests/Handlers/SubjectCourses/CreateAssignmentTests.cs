using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.Assignments;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.SubjectCourses;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CreateAssignmentTests : CoreDatabaseTestBase
{
    public CreateAssignmentTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldCreateAssignmentCorrectly()
    {
        // Arrange
        SubjectCourseModel subjectCourse = await Context.SubjectCourses
            .OrderBy(x => x.Id)
            .FirstAsync();

        var publisher = new Mock<IPublisher>();

        var command = new CreateAssignment.Command(
            subjectCourse.Id,
            Faker.Commerce.ProductName(),
            Faker.Commerce.ProductName(),
            Faker.Random.Int(10, 20),
            Faker.Random.Double(0, 10),
            Faker.Random.Double(10, 20));

        var handler = new CreateAssignmentHandler(PersistenceContext, publisher.Object);

        // Act
        CreateAssignment.Response response = await handler.Handle(command, default);

        // Assert
        int assignmentExists = await Context.Assignments
            .CountAsync(x => x.Id.Equals(response.Assignment.Id));

        assignmentExists.Should().Be(1);
    }
}