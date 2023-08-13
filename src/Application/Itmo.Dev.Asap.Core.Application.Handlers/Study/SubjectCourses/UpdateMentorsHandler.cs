using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;
using Itmo.Dev.Asap.Core.Domain.Users;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands.UpdateMentors;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;

internal class UpdateMentorsHandler : IRequestHandler<Command>
{
    private readonly IPersistenceContext _context;

    public UpdateMentorsHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByIdAsync(request.SubjectCourseId, cancellationToken);

        var userQuery = UserQuery.Build(x => x.WithIds(request.UserIds));

        User[] users = await _context.Users
            .QueryAsync(userQuery, cancellationToken)
            .ToArrayAsync(cancellationToken);

        ISubjectCourseEvent evt = subjectCourse.UpdateMentors(users);

        await _context.SubjectCourses.ApplyAsync(evt, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}