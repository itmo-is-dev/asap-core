using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Commands;

internal static class UpdateSubject
{
    public record Command(Guid Id, string Name) : IRequest<Response>;

    public record Response(SubjectDto Subject);
}