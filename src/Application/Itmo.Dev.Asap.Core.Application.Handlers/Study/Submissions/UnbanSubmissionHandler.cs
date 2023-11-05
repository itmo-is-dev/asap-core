using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands.UnbanSubmission;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Submissions;

internal class UnbanSubmissionHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IPermissionValidator _permissionValidator;
    private readonly IPublisher _publisher;

    public UnbanSubmissionHandler(
        IPersistenceContext context,
        IPermissionValidator permissionValidator,
        IPublisher publisher)
    {
        _context = context;
        _permissionValidator = permissionValidator;
        _publisher = publisher;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Submission submission = await _context.Submissions
            .GetSubmissionForCodeOrLatestAsync(
                request.StudentId,
                request.AssignmentId,
                request.Code,
                cancellationToken);

        bool isAuthorized = await _permissionValidator.IsSubmissionMentorAsync(
            request.IssuerId,
            submission.Id,
            cancellationToken);

        if (isAuthorized is false)
            return new Response.Unauthorized();

        SubmissionStateMoveResult result = submission.Unban();

        if (result is SubmissionStateMoveResult.InvalidMove)
            return new Response.InvalidMove(submission.State.Kind.AsDto());

        if (result is not SubmissionStateMoveResult.Success)
            throw new InvalidOperationException("Operation yielded unexpected result");

        _context.Submissions.Update(submission);
        await _context.SaveChangesAsync(cancellationToken);

        Assignment assignment = await _context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Assignment.Id, cancellationToken);

        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByAssignmentId(assignment.Id, cancellationToken);

        RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(assignment, subjectCourse.DeadlinePolicy);
        SubmissionDto dto = ratedSubmission.ToDto();

        var notification = new SubmissionUpdated.Notification(dto);
        await _publisher.PublishAsync(notification, default);

        return new Response.Success(dto);
    }
}