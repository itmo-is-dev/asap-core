using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Queries;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using Itmo.Dev.Asap.Core.StudentGroups;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class StudentGroupController : StudentGroupService.StudentGroupServiceBase
{
    private readonly IMediator _mediator;

    public StudentGroupController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        CreateStudentGroup.Command command = request.MapTo();
        CreateStudentGroup.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<FindByIdsResponse> FindByIds(FindByIdsRequest request, ServerCallContext context)
    {
        BulkGetStudentGroups.Query query = request.MapTo();
        BulkGetStudentGroups.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
    {
        UpdateStudentGroup.Command command = request.MapTo();
        UpdateStudentGroup.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<GetStudentsResponse> GetStudents(GetStudentsRequest request, ServerCallContext context)
    {
        GetStudentsByGroupId.Query query = request.MapTo();
        GetStudentsByGroupId.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<QueryStudentGroupResponse> Query(QueryStudentGroupRequest request, ServerCallContext context)
    {
        FindStudentGroupsByQuery.Query query = request.MapTo();
        FindStudentGroupsByQuery.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }
}