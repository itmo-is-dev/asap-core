using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Commands;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Queries;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using Itmo.Dev.Asap.Core.Subjects;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class SubjectController : SubjectService.SubjectServiceBase
{
    private readonly IMediator _mediator;

    public SubjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<CreateSubjectResponse> CreateSubject(
        CreateSubjectRequest request,
        ServerCallContext context)
    {
        CreateSubject.Command command = request.MapTo();
        CreateSubject.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
    {
        GetSubjects.Query query = request.MapTo();
        GetSubjects.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
    {
        GetSubjectById.Query query = request.MapTo();
        GetSubjectById.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
    {
        UpdateSubject.Command command = request.MapTo();
        UpdateSubject.Response response = await _mediator.Send(command, context.CancellationToken);

        return response.MapFrom();
    }

    public override async Task<GetCoursesResponse> GetCourses(GetCoursesRequest request, ServerCallContext context)
    {
        GetSubjectCoursesBySubjectId.Query query = request.MapTo();
        GetSubjectCoursesBySubjectId.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }
}