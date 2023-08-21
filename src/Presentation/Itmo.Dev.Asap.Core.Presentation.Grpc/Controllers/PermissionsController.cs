using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries;
using Itmo.Dev.Asap.Core.Permissions;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using MediatR;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class PermissionsController : PermissionService.PermissionServiceBase
{
    private readonly IPermissionValidator _permissionValidator;
    private readonly IMediator _mediator;

    public PermissionsController(IPermissionValidator permissionValidator, IMediator mediator)
    {
        _permissionValidator = permissionValidator;
        _mediator = mediator;
    }

    public override async Task<IsSubmissionMentorResponse> IsSubmissionMentor(
        IsSubmissionMentorRequest request,
        ServerCallContext context)
    {
        return new IsSubmissionMentorResponse
        {
            IsMentor = await _permissionValidator.IsSubmissionMentorAsync(
                Guid.Parse(request.UserId),
                Guid.Parse(request.SubmissionId),
                context.CancellationToken),
        };
    }

    public override async Task<HasAccessToSubjectCourseResponse> HasAccessToSubjectCourse(
        HasAccessToSubjectCourseRequest request,
        ServerCallContext context)
    {
        HasAccessToSubjectCourse.Query query = request.MapTo();
        HasAccessToSubjectCourse.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapFrom();
    }
}