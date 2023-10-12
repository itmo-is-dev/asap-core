using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Abstractions.Submissions;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions.Workflow;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Tools;
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

        SubmissionApprovedResult result = await workflow.SubmissionApprovedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            context.CancellationToken);

        return result switch
        {
            SubmissionApprovedResult.Success success => new ApprovedResponse
            {
                Success = new ApprovedResponse.Types.Success
                {
                    SubmissionRate = success.SubmissionRate.MapToSubmissionRate(),
                },
            },

            SubmissionApprovedResult.InvalidState invalidState => new ApprovedResponse
            {
                InvalidState = new ApprovedResponse.Types.InvalidState
                {
                    SubmissionState = invalidState.State.MapToSubmissionState(),
                },
            },

            _ => throw RpcExceptionFactory.UnexpectedOperationResult,
        };
    }

    public override async Task<ReactivatedResponse> Reactivated(ReactivatedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetSubmissionWorkflowAsync(request.SubmissionId.ToGuid(), context.CancellationToken);

        SubmissionReactivatedResult result = await workflow.SubmissionReactivatedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            context.CancellationToken);

        return result switch
        {
            SubmissionReactivatedResult.Success => new ReactivatedResponse
            {
                Success = new ReactivatedResponse.Types.Success(),
            },

            SubmissionReactivatedResult.InvalidState invalidState => new ReactivatedResponse
            {
                InvalidState = new ReactivatedResponse.Types.InvalidState
                {
                    SubmissionState = invalidState.SubmissionState.MapToSubmissionState(),
                },
            },

            _ => throw RpcExceptionFactory.UnexpectedOperationResult,
        };
    }

    public override async Task<UpdatedResponse> Updated(UpdatedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetAssignmentWorkflowAsync(request.AssignmentId.ToGuid(), context.CancellationToken);

        SubmissionUpdatedResult result = await workflow.SubmissionUpdatedAsync(
            request.IssuerId.ToGuid(),
            request.UserId.ToGuid(),
            request.AssignmentId.ToGuid(),
            request.Payload,
            context.CancellationToken);

        return result switch
        {
            SubmissionUpdatedResult.Success success => new UpdatedResponse
            {
                Success = new UpdatedResponse.Types.Success
                {
                    SubmissionRate = success.SubmissionRate.MapToSubmissionRate(),
                    IsCreated = success.IsCreated,
                },
            },

            _ => throw RpcExceptionFactory.UnexpectedOperationResult,
        };
    }

    public override async Task<AcceptedResponse> Accepted(AcceptedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetSubmissionWorkflowAsync(request.SubmissionId.ToGuid(), context.CancellationToken);

        SubmissionAcceptedResult result = await workflow.SubmissionAcceptedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            context.CancellationToken);

        return result switch
        {
            SubmissionAcceptedResult.Success success => new AcceptedResponse
            {
                Success = new AcceptedResponse.Types.Success
                {
                    SubmissionRate = success.SubmissionRate.MapToSubmissionRate(),
                },
            },

            SubmissionAcceptedResult.InvalidState invalidState => new AcceptedResponse
            {
                InvalidState = new AcceptedResponse.Types.InvalidState
                {
                    SubmissionState = invalidState.SubmissionState.MapToSubmissionState(),
                },
            },

            _ => throw RpcExceptionFactory.UnexpectedOperationResult,
        };
    }

    public override async Task<RejectedResponse> Rejected(RejectedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetSubmissionWorkflowAsync(request.SubmissionId.ToGuid(), context.CancellationToken);

        SubmissionRejectedResult result = await workflow.SubmissionRejectedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            context.CancellationToken);

        return result switch
        {
            SubmissionRejectedResult.Success success => new RejectedResponse
            {
                Success = new RejectedResponse.Types.Success
                {
                    SubmissionCode = success.SubmissionCode,
                },
            },

            SubmissionRejectedResult.InvalidState invalidState => new RejectedResponse
            {
                InvalidState = new RejectedResponse.Types.InvalidState
                {
                    SubmissionState = invalidState.SubmissionState.MapToSubmissionState(),
                },
            },

            _ => throw RpcExceptionFactory.UnexpectedOperationResult,
        };
    }

    public override async Task<AbandonedResponse> Abandoned(AbandonedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetSubmissionWorkflowAsync(request.SubmissionId.ToGuid(), context.CancellationToken);

        SubmissionAbandonedResult result = await workflow.SubmissionAbandonedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            request.IsTerminal,
            context.CancellationToken);

        return result switch
        {
            SubmissionAbandonedResult.Success success => new AbandonedResponse
            {
                Success = new AbandonedResponse.Types.Success
                {
                    SubmissionCode = success.SubmissionCode,
                },
            },

            SubmissionAbandonedResult.InvalidState invalidState => new AbandonedResponse
            {
                InvalidState = new AbandonedResponse.Types.InvalidState
                {
                    SubmissionState = invalidState.SubmissionState.MapToSubmissionState(),
                },
            },

            _ => throw RpcExceptionFactory.UnexpectedOperationResult,
        };
    }

    public override async Task<NotAcceptedResponse> NotAccepted(NotAcceptedRequest request, ServerCallContext context)
    {
        ISubmissionWorkflow workflow = await _workflowService
            .GetSubmissionWorkflowAsync(request.SubmissionId.ToGuid(), context.CancellationToken);

        SubmissionNotAcceptedResult result = await workflow.SubmissionNotAcceptedAsync(
            request.IssuerId.ToGuid(),
            request.SubmissionId.ToGuid(),
            context.CancellationToken);

        return result switch
        {
            SubmissionNotAcceptedResult.Success success => new NotAcceptedResponse
            {
                Success = new NotAcceptedResponse.Types.Success
                {
                    SubmissionRate = success.SubmissionRate.MapToSubmissionRate(),
                },
            },

            SubmissionNotAcceptedResult.InvalidState invalidState => new NotAcceptedResponse
            {
                InvalidState = new NotAcceptedResponse.Types.InvalidState
                {
                    SubmissionState = invalidState.SubmissionState.MapToSubmissionState(),
                },
            },

            _ => throw RpcExceptionFactory.UnexpectedOperationResult,
        };
    }
}