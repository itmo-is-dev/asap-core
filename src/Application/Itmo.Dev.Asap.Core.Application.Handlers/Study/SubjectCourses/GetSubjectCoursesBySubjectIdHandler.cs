using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.Common.Exceptions;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Study;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries.GetSubjectCoursesBySubjectId;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;

internal class GetSubjectCoursesBySubjectIdHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;
    private readonly ICurrentUser _currentUser;

    public GetSubjectCoursesBySubjectIdHandler(
        IPersistenceContext context,
        ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var subjectQuery = SubjectQuery.Build(x => _currentUser.FilterAvailableSubjects(x).WithId(request.SubjectId));

        Subject? subject = await _context.Subjects
            .QueryAsync(subjectQuery, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (subject is null)
            throw UserHasNotAccessException.AccessViolation(_currentUser.Id);

        var subjectCourseQuery = SubjectCourseQuery.Build(x => x.WithSubjectId(request.SubjectId));

        SubjectCourse[] courses = await _context.SubjectCourses
            .QueryAsync(subjectCourseQuery, cancellationToken)
            .ToArrayAsync(cancellationToken);

        SubjectCourseDto[] availableSubjectCourses = courses
            .Where(_currentUser.HasAccessToSubjectCourse)
            .Select(x => x.ToDto())
            .ToArray();

        return new Response(availableSubjectCourses);
    }
}