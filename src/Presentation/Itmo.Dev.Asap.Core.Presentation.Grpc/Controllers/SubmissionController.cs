using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using Itmo.Dev.Asap.Core.Submissions;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class SubmissionController : SubmissionService.SubmissionServiceBase
{
    private readonly IMediator _mediator;

    public SubmissionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<ActivateResponse> Activate(ActivateRequest request, ServerCallContext context)
    {
        ActivateSubmission.Command command = request.MapTo();
        ActivateSubmission.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<BanResponse> Ban(BanRequest request, ServerCallContext context)
    {
        BanSubmission.Command command = request.MapTo();
        BanSubmission.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        CreateSubmission.Command command = request.MapTo();
        CreateSubmission.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<DeactivateResponse> Deactivate(DeactivateRequest request, ServerCallContext context)
    {
        DeactivateSubmission.Command command = request.MapTo();
        DeactivateSubmission.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
    {
        DeleteSubmission.Command command = request.MapTo();
        DeleteSubmission.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<MarkReviewedResponse> MarkReviewed(
        MarkReviewedRequest request,
        ServerCallContext context)
    {
        MarkSubmissionReviewed.Command command = request.MapTo();
        MarkSubmissionReviewed.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<RateResponse> Rate(RateRequest request, ServerCallContext context)
    {
        RateSubmission.Command command = request.MapTo();
        RateSubmission.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
    {
        SubmissionRateDto? rateDto = null;

        if (request.RatingPercent is not null || request.ExtraPoints is not null)
        {
            var command = new UpdateSubmissionPoints.Command(
                request.IssuerId.ToGuid(),
                request.UserId.ToGuid(),
                request.AssignmentId.ToGuid(),
                request.Code,
                request.RatingPercent,
                request.ExtraPoints);

            UpdateSubmissionPoints.Response response = await _mediator.Send(command, context.CancellationToken);

            rateDto = response.Submission;
        }

        if (request.SubmissionDateCase is UpdateRequest.SubmissionDateOneofCase.SubmissionDateValue)
        {
            var command = new UpdateSubmissionDate.Command(
                request.IssuerId.ToGuid(),
                request.UserId.ToGuid(),
                request.AssignmentId.ToGuid(),
                request.Code,
                request.SubmissionDateValue.MapToDateOnly());

            UpdateSubmissionDate.Response response = await _mediator.Send(command, context.CancellationToken);

            rateDto = response.Submission;
        }

        if (rateDto is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "No update command was executed"));

        return new UpdateResponse { Submission = rateDto.MapFrom() };
    }
}