using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Commands.UpdateSubject;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Subjects;

internal class UpdateSubjectHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;

    public UpdateSubjectHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Subject subject = await _context.Subjects.GetByIdAsync(request.Id, cancellationToken);
        subject.Title = request.Name;

        _context.Subjects.Update(subject);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response(subject.ToDto());
    }
}