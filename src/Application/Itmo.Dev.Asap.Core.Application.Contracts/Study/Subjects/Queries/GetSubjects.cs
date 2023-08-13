using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Queries;

internal static class GetSubjects
{
    public record Query : IRequest<Response>;

    public record Response(IReadOnlyCollection<SubjectDto> Subjects);
}