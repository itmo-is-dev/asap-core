using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourseGroups;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.SubjectCourseGroups;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CreateSubjectCourseGroupTests : CoreDatabaseTestBase
{
    public CreateSubjectCourseGroupTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldCreateSubjectCourseGroupCorrectly()
    {
        // Arrange
        SubjectCourseModel subjectCourse = await Context.SubjectCourses
            .OrderBy(x => x.Id)
            .Where(x => x.Assignments.Count != 0)
            .FirstAsync();

        var studentGroup = new StudentGroupModel(Faker.Random.Guid(), Faker.Commerce.ProductName());

        Context.StudentGroups.Add(studentGroup);
        await Context.SaveChangesAsync();

        var publisher = new Mock<IPublisher>();

        var command = new CreateSubjectCourseGroup.Command(subjectCourse.Id, studentGroup.Id);
        var handler = new CreateSubjectCourseGroupHandler(PersistenceContext, publisher.Object);

        // Act
        await handler.Handle(command, default);

        Context.ChangeTracker.Clear();
        subjectCourse = await Context.SubjectCourses.SingleAsync(x => x.Id.Equals(subjectCourse.Id));

        // Assert
        int studentGroupAssignmentCount = subjectCourse.Assignments
            .SelectMany(x => x.GroupAssignments)
            .Count(x => x.StudentGroupId.Equals(studentGroup.Id));

        studentGroupAssignmentCount.Should().Be(subjectCourse.Assignments.Count);
    }
}