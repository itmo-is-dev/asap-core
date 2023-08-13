using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.Submissions;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Submissions;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class ActivateSubmissionTests : CoreDatabaseTestBase
{
    public ActivateSubmissionTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldSetSubmissionStateActive()
    {
        // Arrange
        SubmissionModel submission = await Context.Submissions
            .OrderBy(x => x.Id)
            .Where(x => x.State == SubmissionStateKind.Inactive)
            .FirstAsync();

        var command = new ActivateSubmission.Command(submission.Id);
        var handler = new ActivateSubmissionHandler(PersistenceContext, Mock.Of<IPublisher>());

        // Act
        await handler.Handle(command, default);

        // Assert
        Context.ChangeTracker.Clear();
        submission = await Context.Submissions.SingleAsync(x => x.Id.Equals(submission.Id));

        submission.State.Should().Be(SubmissionStateKind.Active);
    }

    [Fact]
    public async Task HandleAsync_ShouldPublishUpdatedSubmissionWithoutCancellation()
    {
        // Arrange
        SubmissionModel submission = await Context.Submissions
            .OrderBy(x => x.Id)
            .Where(x => x.State == SubmissionStateKind.Inactive)
            .FirstAsync();

        var publisher = new Mock<IPublisher>();

        var command = new ActivateSubmission.Command(submission.Id);
        var handler = new ActivateSubmissionHandler(PersistenceContext, publisher.Object);

        using var cts = new CancellationTokenSource(TimeSpan.FromHours(10));

        // Act
        await handler.Handle(command, cts.Token);

        // Assert
        publisher.Verify(
            x => x.Publish(It.IsAny<SubmissionStateUpdated.Notification>(), default),
            Times.Once);
    }
}