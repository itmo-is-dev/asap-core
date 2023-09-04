using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Queries;

internal static class BulkGetStudentGroups
{
    public record Query(IReadOnlyCollection<Guid> Ids) : IRequest<Response>;

    public record Response(IReadOnlyCollection<StudentGroupDto> Groups);
}