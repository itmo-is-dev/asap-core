using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Permissions;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class PermissionsController : PermissionService.PermissionServiceBase
{
    private readonly IPermissionValidator _permissionValidator;

    public PermissionsController(IPermissionValidator permissionValidator)
    {
        _permissionValidator = permissionValidator;
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
}