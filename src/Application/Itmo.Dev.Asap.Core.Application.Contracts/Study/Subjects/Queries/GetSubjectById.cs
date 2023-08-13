using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Queries;

internal static class GetSubjectById
{
    public record Query(Guid Id) : IRequest<Response>;

    public record Response(SubjectDto Subject);
}