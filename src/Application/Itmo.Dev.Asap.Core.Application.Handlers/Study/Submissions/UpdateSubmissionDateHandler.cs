using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Application.Factories;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using Microsoft.Extensions.Logging;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands.UpdateSubmissionDate;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Submissions;

internal class UpdateSubmissionDateHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IPermissionValidator _permissionValidator;
    private readonly IPublisher _publisher;
    private readonly ILogger<UpdateSubmissionDateHandler> _logger;

    public UpdateSubmissionDateHandler(
        IPermissionValidator permissionValidator,
        IPersistenceContext context,
        IPublisher publisher,
        ILogger<UpdateSubmissionDateHandler> logger)
    {
        _permissionValidator = permissionValidator;
        _context = context;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Submission submission = await _context.Submissions
            .GetSubmissionForCodeOrLatestAsync(request.UserId, request.AssignmentId, request.Code, cancellationToken);

        await _permissionValidator.EnsureSubmissionMentorAsync(
            request.IssuerId,
            submission.Id,
            cancellationToken);

        var date = SpbDateTime.FromDateOnly(request.Date);
        submission.UpdateDate(date);

        _logger.LogInformation(
            "Updated submission date, Id = {Id}, Date = {Date}",
            submission.Id,
            submission.SubmissionDateOnly);

        _context.Submissions.Update(submission);
        await _context.SaveChangesAsync(cancellationToken);

        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByAssignmentId(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        Assignment assignment = await _context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(assignment, subjectCourse.DeadlinePolicy);

        _logger.LogInformation("Calculated rated submission = {RatedSubmission}", ratedSubmission);

        SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
            ratedSubmission,
            assignment);

        _logger.LogInformation("Calculated submission rate dto = {Submission}", submissionRateDto);

        var notification = new SubmissionUpdated.Notification(ratedSubmission.ToDto());
        await _publisher.PublishAsync(notification, cancellationToken);

        return new Response(submissionRateDto);
    }
}