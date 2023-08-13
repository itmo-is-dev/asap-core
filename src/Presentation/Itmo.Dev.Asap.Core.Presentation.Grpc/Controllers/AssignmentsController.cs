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

        return response.MapFrom();
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
        UpdateGroupAssignmentDeadline.Command command = request.MapTo();
        UpdateGroupAssignmentDeadline.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }
}