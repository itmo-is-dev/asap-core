using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.SubjectCourses;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CreateSubjectCourseTests : CoreDatabaseTestBase
{
    public CreateSubjectCourseTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldCreateSubjectCourse()
    {
        // Arrange
        SubjectModel subject = await Context.Subjects
            .OrderBy(x => x.Id)
            .FirstAsync();

        var command = new CreateSubjectCourse.Command(
            string.Empty,
            subject.Id,
            Faker.Commerce.ProductName(),
            SubmissionStateWorkflowTypeDto.ReviewWithDefense);

        var handler = new CreateSubjectCourseHandler(PersistenceContext, Mock.Of<IPublisher>());

        // Act
        CreateSubjectCourse.Response response = await handler.Handle(command, default);

        // Assert
        int subjectCourseCount = await Context.SubjectCourses
            .Where(x => x.Id.Equals(response.SubjectCourse.Id))
            .CountAsync();

        subjectCourseCount.Should().Be(1);
    }

    [Fact]
    public async Task HandleAsync_ShouldPublishSubjectCourseWithoutCancellation()
    {
        // Arrange
        SubjectModel subject = await Context.Subjects
            .OrderBy(x => x.Id)
            .FirstAsync();

        var publisher = new Mock<IPublisher>();

        var command = new CreateSubjectCourse.Command(
            string.Empty,
            subject.Id,
            Faker.Commerce.ProductName(),
            SubmissionStateWorkflowTypeDto.ReviewWithDefense);

        var handler = new CreateSubjectCourseHandler(PersistenceContext, publisher.Object);

        using var cts = new CancellationTokenSource(TimeSpan.FromHours(10));

        // Act
        await handler.Handle(command, cts.Token);

        // Assert
        publisher.Verify(
            x => x.Publish(It.IsAny<SubjectCourseCreated.Notification>(), default),
            Times.Once);
    }
}