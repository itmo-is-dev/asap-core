using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Queries;

internal static class GetStudentGroupById
{
    public record Query(Guid Id) : IRequest<Response>;

    public record Response(StudentGroupDto Group);
}