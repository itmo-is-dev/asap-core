using Itmo.Dev.Asap.Core.Application.Contracts.Students.Notifications;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Students;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
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
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByAssignmentId(notification.Submission.AssignmentId, cancellationToken);

        StudentAssignmentPointsDto dto = notification.Submission.MapToStudentAssignmentPoints(subjectCourse.Id);
        var n = new StudentAssignmentPointsUpdated.Notification(new[] { dto });

        await _publisher.Publish(n, cancellationToken);
    }

    public async Task Handle(SubmissionUpdated.Notification notification, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByAssignmentId(notification.Submission.AssignmentId, cancellationToken);

        StudentAssignmentPointsDto dto = notification.Submission.MapToStudentAssignmentPoints(subjectCourse.Id);
        var n = new StudentAssignmentPointsUpdated.Notification(new[] { dto });

        await _publisher.Publish(n, cancellationToken);
    }
}