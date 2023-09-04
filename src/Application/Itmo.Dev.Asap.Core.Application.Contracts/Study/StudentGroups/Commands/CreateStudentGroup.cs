using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Commands;

internal static class CreateStudentGroup
{
    public record Command(string Name) : IRequest<Response>;

    public record Response(StudentGroupDto Group);
}