using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Domain.Study;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Commands.CreateSubject;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Subjects;

internal class CreateSubjectHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;

    public CreateSubjectHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        var subject = new Subject(Guid.NewGuid(), request.Title);

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response(subject.ToDto());
    }
}