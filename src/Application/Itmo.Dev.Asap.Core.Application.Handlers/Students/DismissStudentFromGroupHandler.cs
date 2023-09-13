using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Users.Commands.DismissStudentFromGroup;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Students;

internal class DismissStudentFromGroupHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;

    public DismissStudentFromGroupHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Student student = await _context.Students
            .GetByIdAsync(request.StudentId, cancellationToken);

        if (student.Group is null)
            return new Response(student.ToDto());

        StudentGroup group = await _context.StudentGroups.GetByIdAsync(student.Group.Id, cancellationToken);

        student.DismissFromStudyGroup(group);

        _context.Students.Update(student);
        _context.StudentGroups.Update(group);

        await _context.SaveChangesAsync(cancellationToken);

        return new Response(student.ToDto());
    }
}