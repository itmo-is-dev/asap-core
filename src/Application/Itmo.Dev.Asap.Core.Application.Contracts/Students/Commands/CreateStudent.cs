using Itmo.Dev.Asap.Core.Application.Dto.Users;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands;

internal static class CreateStudent
{
    public record Command(
        string FirstName,
        string MiddleName,
        string LastName,
        Guid GroupId,
        int UniversityId) : IRequest<Response>;

    public record Response(StudentDto Student);
}