using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Application.Factories;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Common.Resources;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Domain.ValueObject;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Submissions.Workflows;

public class ReviewOnlySubmissionWorkflow : SubmissionWorkflowBase
{
    public ReviewOnlySubmissionWorkflow(
        IPersistenceContext context,
        IPermissionValidator permissionValidator,
        IPublisher publisher) : base(permissionValidator, context, publisher) { }

    public override async Task<SubmissionActionMessageDto> SubmissionApprovedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        await PermissionValidator.EnsureSubmissionMentorAsync(issuerId, submissionId, cancellationToken);

        Submission submission = await ExecuteSubmissionCommandAsync(
            submissionId,
            cancellationToken,
            static x =>
            {
                if (x.Rating is null)
                    x.Rate(Fraction.FromDenormalizedValue(100), 0);
            });

        SubjectCourse subjectCourse = await Context.SubjectCourses
            .GetByAssignmentId(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        Assignment assignment = await Context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(assignment, subjectCourse.DeadlinePolicy);

        SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
            ratedSubmission,
            assignment);

        string message = UserCommandProcessingMessage.ReviewRatedSubmission(submissionRateDto.TotalPoints ?? 0);

        return new SubmissionActionMessageDto(message);
    }
}