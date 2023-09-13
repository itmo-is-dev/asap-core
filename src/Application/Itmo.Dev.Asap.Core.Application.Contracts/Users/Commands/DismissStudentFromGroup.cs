using Itmo.Dev.Asap.Core.Application.Dto.Users;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands;

internal static class DismissStudentFromGroup
{
    public record Command(Guid StudentId) : IRequest<Response>;

    public record Response(StudentDto Student);
}