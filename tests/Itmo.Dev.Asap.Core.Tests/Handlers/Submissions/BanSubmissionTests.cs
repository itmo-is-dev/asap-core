using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.Submissions;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Submissions;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class BanSubmissionTests : CoreDatabaseTestBase
{
    public BanSubmissionTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldSetSubmissionStateBanned_WhenIssuedByMentor()
    {
        // Arrange
        SubmissionModel submission = await Context.Submissions
            .OrderBy(x => x.Id)
            .Where(x => x.State == SubmissionStateKind.Active)
            .FirstAsync();

        Guid issuerId = submission.GroupAssignment.Assignment.SubjectCourse.Mentors.First().UserId;

        var command = new BanSubmission.Command(
            issuerId,
            submission.StudentId,
            submission.AssignmentId,
            submission.Code);

        var handler = new BanSubmissionHandler(
            GetRequiredService<IPermissionValidator>(),
            PersistenceContext,
            Mock.Of<IPublisher>());

        // Act
        await handler.Handle(command, default);

        // Assert
        Context.ChangeTracker.Clear();
        submission = await Context.Submissions.SingleAsync(x => x.Id.Equals(submission.Id));

        submission.State.Should().Be(SubmissionStateKind.Banned);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenIssuedNotByMentor()
    {
        // Arrange
        SubmissionModel submission = await Context.Submissions
            .OrderBy(x => x.Id)
            .Where(x => x.State == SubmissionStateKind.Active)
            .FirstAsync();

        IEnumerable<Guid> mentorIds = submission.GroupAssignment.Assignment.SubjectCourse.Mentors.Select(x => x.UserId);

        UserModel user = await Context.Users
            .OrderBy(x => x.Id)
            .Where(u => mentorIds.Contains(u.Id) == false)
            .FirstAsync();

        var command = new BanSubmission.Command(
            user.Id,
            submission.StudentId,
            submission.AssignmentId,
            submission.Code);

        var handler = new BanSubmissionHandler(
            GetRequiredService<IPermissionValidator>(),
            PersistenceContext,
            Mock.Of<IPublisher>());

        // Act
        Func<Task<BanSubmission.Response>> action = () => handler.Handle(command, default);

        // Assert
        await action.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task HandleAsync_ShouldPublishUpdatedSubmissionWithoutCancellation()
    {
        // Arrange
        SubmissionModel submission = await Context.Submissions
            .OrderBy(x => x.Id)
            .Where(x => x.State == SubmissionStateKind.Active)
            .FirstAsync();

        var publisher = new Mock<IPublisher>();

        var command = new BanSubmission.Command(
            Guid.Empty,
            submission.StudentId,
            submission.AssignmentId,
            submission.Code);

        var handler = new BanSubmissionHandler(
            Mock.Of<IPermissionValidator>(),
            PersistenceContext,
            publisher.Object);

        using var cts = new CancellationTokenSource(TimeSpan.FromHours(10));

        // Act
        await handler.Handle(command, cts.Token);

        // Assert
        publisher.Verify(
            x => x.Publish(It.IsAny<SubmissionUpdated.Notification>(), default),
            Times.Once);
    }
}