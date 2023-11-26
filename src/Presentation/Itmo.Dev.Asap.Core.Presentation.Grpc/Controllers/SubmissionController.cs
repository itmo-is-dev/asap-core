using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Models;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using Itmo.Dev.Asap.Core.Submissions;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class SubmissionController : SubmissionService.SubmissionServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SubmissionController> _logger;

    public SubmissionController(IMediator mediator, ILogger<SubmissionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
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

    public override async Task<UnbanResponse> Unban(UnbanRequest request, ServerCallContext context)
    {
        UnbanSubmission.Command command = request.MapTo();
        UnbanSubmission.Response response = await _mediator.Send(command, context.CancellationToken);

        return response switch
        {
            UnbanSubmission.Response.Success success => new UnbanResponse
            {
                Success = new UnbanResponse.Types.Success
                {
                    Submission = success.Submission.MapToProtoSubmission(),
                },
            },

            UnbanSubmission.Response.Unauthorized => new UnbanResponse
            {
                Unauthorized = new UnbanResponse.Types.Unauthorized(),
            },

            UnbanSubmission.Response.InvalidMove invalidMove => new UnbanResponse
            {
                InvalidMove = new UnbanResponse.Types.InvalidMove
                {
                    SourceState = invalidMove.SourceState.MapToSubmissionState(),
                },
            },

            _ => throw new RpcException(new Status(StatusCode.Internal, "Operation yielded in unexpected result")),
        };
    }

    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        CreateSubmission.Command command = request.MapTo();
        CreateSubmission.Response response = await _mediator.Send(command, context.CancellationToken);

        return response switch
        {
            CreateSubmission.Response.Success s => new CreateResponse
            {
                Success = new CreateResponse.Types.Success { Submission = s.Submission.MapToProtoSubmission() },
            },

            CreateSubmission.Response.Unauthorized => new CreateResponse
            {
                Unauthorized = new CreateResponse.Types.Unauthorized(),
            },

            _ => throw new RpcException(new Status(StatusCode.Internal, "Operation produced unexpected result")),
        };
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
        try
        {
            RateSubmission.Command command = request.MapTo();
            RateSubmission.Response response = await _mediator.Send(command, context.CancellationToken);

            return new RateResponse
            {
                Submission = response.Submission.MapFrom(),
            };
        }
        catch (DomainInvalidOperationException e)
        {
            return new RateResponse
            {
                ErrorMessage = e.Message,
            };
        }
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

            try
            {
                UpdateSubmissionPoints.Response response = await _mediator.Send(command, context.CancellationToken);

                rateDto = response.Submission;
            }
            catch (DomainInvalidOperationException e)
            {
                return new UpdateResponse
                {
                    ErrorMessage = e.Message,
                };
            }
        }

        if (request.SubmissionDateCase is UpdateRequest.SubmissionDateOneofCase.SubmissionDateValue)
        {
            var command = new UpdateSubmissionDate.Command(
                request.IssuerId.ToGuid(),
                request.UserId.ToGuid(),
                request.AssignmentId.ToGuid(),
                request.Code,
                request.SubmissionDateValue.MapToDateOnly());

            try
            {
                UpdateSubmissionDate.Response response = await _mediator.Send(command, context.CancellationToken);

                rateDto = response.Submission;
            }
            catch (DomainInvalidOperationException e)
            {
                return new UpdateResponse
                {
                    ErrorMessage = e.Message,
                };
            }
        }

        if (rateDto is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "No update command was executed"));

        SubmissionRate dto = rateDto.MapFrom();

        _logger.LogInformation("Returning submission rate = {Rate}", dto);

        return new UpdateResponse { Submission = dto };
    }

    public override async Task<QueryFirstCompletedSubmissionResponse> QueryFirstCompletedSubmission(
        QueryFirstCompletedSubmissionRequest request,
        ServerCallContext context)
    {
        var query = new QueryFirstCompletedSubmissions.Query(
            request.SubjectCourseId.ToGuid(),
            JsonConvert.DeserializeObject<QueryFirstCompletedSubmissions.PageToken>(request.PageToken),
            request.PageSize);

        QueryFirstCompletedSubmissions.Response response = await _mediator.Send(query, context.CancellationToken);

        IEnumerable<QueryFirstCompletedSubmissionResponse.Types.FirstSubmission> submissions = response.Submissions
            .Select(submission => new QueryFirstCompletedSubmissionResponse.Types.FirstSubmission
            {
                SubmissionId = submission.Id.ToString(),
                UserId = submission.UserId.ToString(),
                AssignmentId = submission.AssignmentId.ToString(),
            });

        return new QueryFirstCompletedSubmissionResponse
        {
            Submission = { submissions },
            PageToken = JsonConvert.SerializeObject(response.PageToken),
        };
    }
}