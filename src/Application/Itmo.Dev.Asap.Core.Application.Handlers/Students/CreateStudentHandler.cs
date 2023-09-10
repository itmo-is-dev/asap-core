using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.UserAssociations;
using Itmo.Dev.Asap.Core.Domain.Users;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands.CreateStudents;

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
        IEnumerable<Guid> groupIds = request.Students.Select(x => x.GroupId);
        var groupsQuery = StudentGroupQuery.Build(x => x.WithIds(groupIds));

        Dictionary<Guid, StudentGroup> groups = await _context.StudentGroups
            .QueryAsync(groupsQuery, cancellationToken)
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        var students = new List<Student>(request.Students.Count);

        foreach (Command.Model model in request.Students)
        {
            if (groups.TryGetValue(model.GroupId, out StudentGroup? group) is false)
                return new Response.GroupNotFound(model.GroupId);

            var user = new User(Guid.NewGuid(), model.FirstName, model.MiddleName, model.LastName);
            IsuUserAssociation.CreateAndAttach(Guid.NewGuid(), user, model.UniversityId);

            var student = new Student(user, group.Info);

            _context.Users.Add(user);
            _context.Students.Add(student);

            students.Add(student);
        }

        await _context.SaveChangesAsync(cancellationToken);

        IEnumerable<StudentDto> dto = students.Select(x => x.ToDto());

        return new Response.Success(dto);
    }
}