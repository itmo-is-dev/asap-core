using Itmo.Dev.Asap.Core.Application.Contracts.Students.Notifications;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Domain.Study;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Students;

internal class StudentAssignmentPointsUpdatedDispatcher :
    INotificationHandler<SubmissionPointsUpdated.Notification>,
    INotificationHandler<SubmissionUpdated.Notification>
{
    private readonly IPublisher _publisher;
    private readonly IPersistenceContext _context;

    public StudentAssignmentPointsUpdatedDispatcher(IPublisher publisher, IPersistenceContext context)
    {
        _publisher = publisher;
        _context = context;
    }

    public async Task Handle(SubmissionPointsUpdated.Notification notification, CancellationToken cancellationToken)
    {
        StudentAssignment studentAssignment = await _context.StudentAssignments
            .GetByIdAsync(notification.Submission.StudentId, notification.Submission.AssignmentId, cancellationToken);

        StudentAssignmentPoints? points = studentAssignment.CalculatePoints();

        if (points is null)
            return;

        var n = new StudentAssignmentPointsUpdated.Notification(new[] { points.Value.ToDto() });
        await _publisher.Publish(n, cancellationToken);
    }

    public async Task Handle(SubmissionUpdated.Notification notification, CancellationToken cancellationToken)
    {
        StudentAssignment studentAssignment = await _context.StudentAssignments
            .GetByIdAsync(notification.Submission.StudentId, notification.Submission.AssignmentId, cancellationToken);

        StudentAssignmentPoints? points = studentAssignment.CalculatePoints();

        if (points is null)
            return;

        var n = new StudentAssignmentPointsUpdated.Notification(new[] { points.Value.ToDto() });
        await _publisher.Publish(n, cancellationToken);
    }
}