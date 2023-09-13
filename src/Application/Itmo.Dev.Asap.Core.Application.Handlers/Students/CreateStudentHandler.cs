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

        if (groupsQuery.Ids.Length != groups.Count)
        {
            IEnumerable<Guid> notFoundGroupIds = groupsQuery.Ids.Except(groups.Keys);
            return new Response.GroupsNotFound(notFoundGroupIds);
        }

        Student[] students = request.Students
            .Select(model =>
            {
                StudentGroup group = groups[model.GroupId];

                var user = new User(Guid.NewGuid(), model.FirstName, model.MiddleName, model.LastName);
                IsuUserAssociation.CreateAndAttach(Guid.NewGuid(), user, model.UniversityId);

                var student = new Student(user, group.Info);

                _context.Users.Add(user);
                _context.Students.Add(student);

                return student;
            })
            .ToArray();

        await _context.SaveChangesAsync(cancellationToken);

        IEnumerable<StudentDto> dto = students.Select(x => x.ToDto());

        return new Response.Success(dto);
    }
}