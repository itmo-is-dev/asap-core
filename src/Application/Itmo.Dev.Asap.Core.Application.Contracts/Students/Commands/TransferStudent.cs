using Itmo.Dev.Asap.Core.Application.Dto.Users;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands;

internal static class TransferStudent
{
    public record Command(Guid StudentId, Guid GroupId) : IRequest<Response>;

    public record Response(StudentDto Student);
}