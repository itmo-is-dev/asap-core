using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands;

internal static class DismissStudentFromGroup
{
    public record Command(Guid StudentId) : IRequest;
}