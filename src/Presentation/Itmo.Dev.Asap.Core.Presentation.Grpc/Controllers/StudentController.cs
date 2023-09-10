using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using Itmo.Dev.Asap.Core.Students;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class StudentController : StudentService.StudentServiceBase
{
    private readonly IMediator _mediator;

    public StudentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<CreateStudentsResponse> Create(CreateStudentsRequest request, ServerCallContext context)
    {
        CreateStudents.Command command = request.MapTo();
        CreateStudents.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<DismissFromGroupResponse> DismissFromGroup(DismissFromGroupRequest request, ServerCallContext context)
    {
        DismissStudentFromGroup.Command command = request.MapTo();
        DismissStudentFromGroup.Response response = await _mediator.Send(command);

        return response.MapFrom();
    }

    public override async Task<TransferStudentResponse> Transfer(
        TransferStudentRequest request,
        ServerCallContext context)
    {
        TransferStudent.Command command = request.MapTo();
        TransferStudent.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<QueryStudentResponse> Query(QueryStudentRequest request, ServerCallContext context)
    {
        FindStudentsByQuery.Query query = request.MapTo();
        FindStudentsByQuery.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }
}