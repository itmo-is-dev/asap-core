using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Notifications;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourseGroups;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.SubjectCourseGroups;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class BulkCreateSubjectCourseGroupTests : CoreDatabaseTestBase
{
    public BulkCreateSubjectCourseGroupTests(CoreDatabaseFixture database, ITestOutputHelper output)
        : base(database, output) { }

    [Fact]
    public async Task HandleAsync_ShouldCreateMultipleGroups()
    {
        // Arrange
        SubjectCourseModel subjectCourse = await Context.SubjectCourses
            .OrderBy(x => x.Id)
            .Where(x => x.Assignments.Count != 0)
            .FirstAsync();

        StudentGroupModel[] studentGroups = Enumerable.Range(0, Faker.Random.Int(10, 15))
            .Select(_ => new StudentGroupModel(Faker.Random.Guid(), Faker.Commerce.ProductName()))
            .ToArray();

        Context.StudentGroups.AddRange(studentGroups);
        await Context.SaveChangesAsync();

        Guid[] studentGroupIds = studentGroups.Select(x => x.Id).ToArray();

        var publisher = new Mock<IPublisher>();

        var command = new BulkCreateSubjectCourseGroups.Command(subjectCourse.Id, studentGroupIds);
        var handler = new BulkCreateSubjectCourseGroupsHandler(PersistenceContext, publisher.Object);

        // Act
        await handler.Handle(command, default);

        Context.ChangeTracker.Clear();
        subjectCourse = await Context.SubjectCourses.SingleAsync(x => x.Id.Equals(subjectCourse.Id));

        // Assert
        foreach (StudentGroupModel studentGroup in studentGroups)
        {
            int studentGroupAssignmentCount = subjectCourse.Assignments
                .SelectMany(x => x.GroupAssignments)
                .Count(x => x.StudentGroupId.Equals(studentGroup.Id));

            studentGroupAssignmentCount.Should().Be(subjectCourse.Assignments.Count);
        }
    }

    [Fact]
    public async Task HandleAsync_ShouldNotifyMultipleGroups()
    {
        // Arrange
        SubjectCourseModel subjectCourse = await Context.SubjectCourses
            .OrderBy(x => x.Id)
            .Where(x => x.Assignments.Count != 0)
            .FirstAsync();

        int groupCount = Faker.Random.Int(10, 15);

        StudentGroupModel[] studentGroups = Enumerable.Range(0, groupCount)
            .Select(_ => new StudentGroupModel(Faker.Random.Guid(), Faker.Commerce.ProductName()))
            .ToArray();

        Context.StudentGroups.AddRange(studentGroups);
        await Context.SaveChangesAsync();

        Guid[] studentGroupIds = studentGroups.Select(x => x.Id).ToArray();

        var publisher = new Mock<IPublisher>();

        var command = new BulkCreateSubjectCourseGroups.Command(subjectCourse.Id, studentGroupIds);
        var handler = new BulkCreateSubjectCourseGroupsHandler(PersistenceContext, publisher.Object);

        // Act
        await handler.Handle(command, default);

        // Assert
        publisher.Verify(
            x => x.Publish(It.IsAny<SubjectCourseGroupCreated.Notification>(), default),
            Times.Exactly(groupCount));
    }
}