using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Abstractions.Submissions;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using Itmo.Dev.Asap.Core.SubmissionWorkflow;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class SubmissionWorkflowController : SubmissionWorkflowService.SubmissionWorkflowServiceBase
{
    private readonly ISubmissionWorkflowService _workflowService;

    public SubmissionWorkflowController(ISubmissionWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    public override async Task<ApprovedResponse> Approved(ApprovedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetSubmissionWorkflowAsync(request.SubmissionId.ToGuid(), context.CancellationToken);

        SubmissionActionMessageDto message = await workflow.SubmissionApprovedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            context.CancellationToken);

        return new ApprovedResponse { Message = message.Message };
    }

    public override async Task<ReactivatedResponse> Reactivated(ReactivatedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetSubmissionWorkflowAsync(request.SubmissionId.ToGuid(), context.CancellationToken);

        SubmissionActionMessageDto message = await workflow.SubmissionReactivatedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            context.CancellationToken);

        return new ReactivatedResponse { Message = message.Message };
    }

    public override async Task<UpdatedResponse> Updated(UpdatedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetAssignmentWorkflowAsync(request.AssignmentId.ToGuid(), context.CancellationToken);

        SubmissionUpdateResult result = await workflow.SubmissionUpdatedAsync(
            request.IssuerId.ToGuid(),
            request.UserId.ToGuid(),
            request.AssignmentId.ToGuid(),
            request.Payload,
            context.CancellationToken);

        return result.MapFrom();
    }

    public override async Task<AcceptedResponse> Accepted(AcceptedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetSubmissionWorkflowAsync(request.SubmissionId.ToGuid(), context.CancellationToken);

        SubmissionActionMessageDto message = await workflow.SubmissionAcceptedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            context.CancellationToken);

        return new AcceptedResponse { Message = message.Message };
    }

    public override async Task<RejectedResponse> Rejected(RejectedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetSubmissionWorkflowAsync(request.SubmissionId.ToGuid(), context.CancellationToken);

        SubmissionActionMessageDto message = await workflow.SubmissionRejectedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            context.CancellationToken);

        return new RejectedResponse { Message = message.Message };
    }

    public override async Task<AbandonedResponse> Abandoned(AbandonedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetSubmissionWorkflowAsync(request.SubmissionId.ToGuid(), context.CancellationToken);

        SubmissionActionMessageDto message = await workflow.SubmissionAbandonedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            request.IsTerminal,
            context.CancellationToken);

        return new AbandonedResponse { Message = message.Message };
    }

    public override async Task<NotAcceptedResponse> NotAccepted(NotAcceptedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetSubmissionWorkflowAsync(request.SubmissionId.ToGuid(), context.CancellationToken);

        SubmissionActionMessageDto message = await workflow.SubmissionNotAcceptedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            context.CancellationToken);

        return new NotAcceptedResponse { Message = message.Message };
    }
}