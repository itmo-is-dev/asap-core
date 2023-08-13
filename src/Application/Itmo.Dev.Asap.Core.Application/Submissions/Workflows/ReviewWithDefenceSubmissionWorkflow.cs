using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Common.Resources;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Submissions.Workflows;

public class ReviewWithDefenceSubmissionWorkflow : SubmissionWorkflowBase
{
    public ReviewWithDefenceSubmissionWorkflow(
        IPermissionValidator permissionValidator,
        IPersistenceContext context,
        IPublisher publisher) : base(permissionValidator, context, publisher) { }

    public override async Task<SubmissionActionMessageDto> SubmissionApprovedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        await PermissionValidator.EnsureSubmissionMentorAsync(issuerId, submissionId, cancellationToken);
        await ExecuteSubmissionCommandAsync(submissionId, cancellationToken, static x => x.MarkAsReviewed());

        string message = UserCommandProcessingMessage.SubmissionMarkAsReviewedAndNeedDefense();
        return new SubmissionActionMessageDto(message);
    }
}