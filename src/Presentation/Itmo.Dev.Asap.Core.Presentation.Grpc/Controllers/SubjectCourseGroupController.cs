using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Commands;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using Itmo.Dev.Asap.Core.SubjectCourseGroups;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class SubjectCourseGroupController : SubjectCourseGroupService.SubjectCourseGroupServiceBase
{
    private readonly IMediator _mediator;

    public SubjectCourseGroupController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        BulkCreateSubjectCourseGroups.Command command = request.MapTo();
        BulkCreateSubjectCourseGroups.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<Empty> Delete(DeleteRequest request, ServerCallContext context)
    {
        DeleteSubjectCourseGroup.Command command = request.MapTo();
        await _mediator.Send(command, context.CancellationToken);

        return new Empty();
    }
}