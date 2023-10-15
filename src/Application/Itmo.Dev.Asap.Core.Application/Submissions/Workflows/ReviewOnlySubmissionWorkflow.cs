using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.Common.Exceptions;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions.Workflow;
using Itmo.Dev.Asap.Core.Application.Factories;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Domain.ValueObject;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Submissions.Workflows;

public class ReviewOnlySubmissionWorkflow : SubmissionWorkflowBase
{
    public ReviewOnlySubmissionWorkflow(
        IPersistenceContext context,
        IPermissionValidator permissionValidator,
        IPublisher publisher) : base(permissionValidator, context, publisher) { }

    public override async Task<SubmissionApprovedResult> SubmissionApprovedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        await PermissionValidator.EnsureSubmissionMentorAsync(issuerId, submissionId, cancellationToken);

        ExecuteSubmissionCommandResult result = await ExecuteSubmissionCommandAsync(
            submissionId,
            cancellationToken,
            static x => x.Rating is null
                ? x.Rate(Fraction.FromDenormalizedValue(100), 0)
                : new SubmissionStateMoveResult.Success(x.State));

        if (result is ExecuteSubmissionCommandResult.InvalidMove invalidMove)
            return new SubmissionApprovedResult.InvalidState(invalidMove.Submission.State.Kind.AsDto());

        if (result is not ExecuteSubmissionCommandResult.Success success)
            throw new UnexpectedOperationResultException();

        Submission submission = success.Submission;

        SubjectCourse subjectCourse = await Context.SubjectCourses
            .GetByAssignmentId(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        Assignment assignment = await Context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(assignment, subjectCourse.DeadlinePolicy);

        SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
            ratedSubmission,
            assignment);

        return new SubmissionApprovedResult.Success(submissionRateDto);
    }
}