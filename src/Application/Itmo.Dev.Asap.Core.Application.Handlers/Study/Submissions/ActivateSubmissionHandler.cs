using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands.ActivateSubmission;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Submissions;

internal class ActivateSubmissionHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IPublisher _publisher;

    public ActivateSubmissionHandler(IPersistenceContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Submission submission = await _context.Submissions.GetByIdAsync(request.SubmissionId, cancellationToken);
        submission.Activate();

        _context.Submissions.Update(submission);
        await _context.SaveChangesAsync(cancellationToken);

        Assignment assignment = await _context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Assignment.Id, cancellationToken);

        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByAssignmentId(assignment.Id, cancellationToken);

        RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(assignment, subjectCourse.DeadlinePolicy);
        SubmissionDto dto = ratedSubmission.ToDto();

        var notification = new SubmissionStateUpdated.Notification(dto);
        await _publisher.PublishAsync(notification, default);

        return new Response(dto);
    }
}