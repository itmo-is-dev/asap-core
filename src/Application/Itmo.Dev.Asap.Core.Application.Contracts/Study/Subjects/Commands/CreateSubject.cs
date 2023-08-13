using Itmo.Dev.Asap.Core.Application.Dto.Study;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Commands;

internal static class CreateSubject
{
    public record Command(string Title) : IRequest<Response>;

    public record Response(SubjectDto Subject);
}