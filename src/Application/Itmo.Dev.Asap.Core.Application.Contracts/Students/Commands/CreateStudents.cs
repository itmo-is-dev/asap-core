using Itmo.Dev.Asap.Core.Application.Dto.Students;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands;

internal static class CreateStudents
{
    public record Command(IReadOnlyCollection<Command.Model> Students) : IRequest<Response>
    {
        public sealed record Model(
            string FirstName,
            string MiddleName,
            string LastName,
            Guid GroupId,
            int UniversityId);
    }

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(IEnumerable<StudentDto> Students) : Response;

        public sealed record GroupsNotFound(IEnumerable<Guid> GroupIds) : Response;
    }
}