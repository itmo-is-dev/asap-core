using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.UserAssociations;
using Itmo.Dev.Asap.Core.Domain.Users;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands.CreateStudent;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Students;

internal class CreateStudentHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;

    public CreateStudentHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        StudentGroup group = await _context.StudentGroups
            .GetByIdAsync(request.GroupId, cancellationToken);

        var user = new User(Guid.NewGuid(), request.FirstName, request.MiddleName, request.LastName);
        IsuUserAssociation.CreateAndAttach(Guid.NewGuid(), user, request.UniversityId);

        var student = new Student(user, group.Info);

        _context.Users.Add(user);
        _context.Students.Add(student);

        await _context.SaveChangesAsync(cancellationToken);

        StudentDto dto = student.ToDto();

        return new Response(dto);
    }
}