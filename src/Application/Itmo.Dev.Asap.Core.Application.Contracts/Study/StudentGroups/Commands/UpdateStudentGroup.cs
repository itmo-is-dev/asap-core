using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Commands;

internal static class UpdateStudentGroup
{
    public record Command(Guid Id, string Name) : IRequest<Response>;

    public record Response(StudentGroupDto Group);
}