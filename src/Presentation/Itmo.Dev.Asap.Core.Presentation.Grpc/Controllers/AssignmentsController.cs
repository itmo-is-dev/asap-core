using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Queries;
using Itmo.Dev.Asap.Core.Assignments;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class AssignmentsController : AssignmentsService.AssignmentsServiceBase
{
    private readonly IMediator _mediator;

    public AssignmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<CreateResponse> CreateAssignment(CreateRequest request, ServerCallContext context)
    {
        CreateAssignment.Command command = request.MapTo();
        CreateAssignment.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
    {
        GetAssignmentById.Query query = request.MapTo();
        GetAssignmentById.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<UpdatePointsResponse> UpdatePoints(
        UpdatePointsRequest request,
        ServerCallContext context)
    {
        UpdateAssignmentPoints.Command command = request.MapTo();
        UpdateAssignmentPoints.Response response = await _mediator.Send(command, context.CancellationToken);

        return response switch
        {
            UpdateAssignmentPoints.Response.Success s => s.MapFrom(),

            UpdateAssignmentPoints.Response.MaxPointsLessThanMinPoints
                => throw new RpcException(new Status(
                    StatusCode.InvalidArgument,
                    "Max points cannot be less than min points")),

            _ => throw new RpcException(new Status(StatusCode.Unknown, "Operation yielded unknown result")),
        };
    }

    public override async Task<GetGroupAssignmentsResponse> GetGroupAssignments(
        GetGroupAssignmentsRequest request,
        ServerCallContext context)
    {
        GetGroupAssignments.Query query = request.MapTo();
        GetGroupAssignments.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<UpdateGroupAssignmentResponse> UpdateGroupAssignment(
        UpdateGroupAssignmentRequest request,
        ServerCallContext context)
    {
        var command = new UpdateGroupAssignmentDeadlines.Command(
            request.AssignmentId.ToGuid(),
            request.Deadline.MapToDateOnly(),
            new[] { request.GroupId.ToGuid() });

        UpdateGroupAssignmentDeadlines.Response response = await _mediator.Send(command, context.CancellationToken);

        return response switch
        {
            UpdateGroupAssignmentDeadlines.Response.Success success => new UpdateGroupAssignmentResponse
            {
                GroupAssignment = success.GroupAssignments.Single().MapToProto(),
            },

            Application.Contracts.Study.GroupAssignments.Commands.UpdateGroupAssignmentDeadlines.Response.Unauthorized
                => throw new RpcException(new Status(
                    StatusCode.PermissionDenied,
                    "User is unauthorized to update this subject course deadlines")),

            _ => throw new RpcException(new Status(StatusCode.Internal, "Operation finished with unexpected result")),
        };
    }

    public override async Task<UpdateGroupAssignmentDeadlinesResponse> UpdateGroupAssignmentDeadlines(
        UpdateGroupAssignmentDeadlinesRequest request,
        ServerCallContext context)
    {
        UpdateGroupAssignmentDeadlines.Command command = request.MapTo();
        UpdateGroupAssignmentDeadlines.Response response = await _mediator.Send(command, context.CancellationToken);

        return response switch
        {
            UpdateGroupAssignmentDeadlines.Response.Success success => success.MapFrom(),

            Application.Contracts.Study.GroupAssignments.Commands.UpdateGroupAssignmentDeadlines.Response.Unauthorized
                => throw new RpcException(new Status(
                    StatusCode.PermissionDenied,
                    "User is unauthorized to update this subject course deadlines")),

            _ => throw new RpcException(new Status(StatusCode.Internal, "Operation finished with unexpected result")),
        };
    }

    public override async Task<QueryResponse> Query(QueryRequest request, ServerCallContext context)
    {
        var query = new QueryAssignments.Query(
            request.Ids.Select(x => x.ToGuid()),
            request.Names);

        QueryAssignments.Response response = await _mediator.Send(query, context.CancellationToken);

        return new QueryResponse
        {
            Assignments = { response.Assignments.Select(x => x.MapToProto()) },
        };
    }
}