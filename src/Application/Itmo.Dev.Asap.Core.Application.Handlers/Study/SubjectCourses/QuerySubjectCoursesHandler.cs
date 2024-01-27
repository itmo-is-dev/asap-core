using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries.QuerySubjectCourses;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;

internal class QuerySubjectCoursesHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public QuerySubjectCoursesHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = SubjectCourseQuery.Build(x => x.WithIds(request.Ids));

        SubjectCourseDto[] subjectCourses = await _context.SubjectCourses
            .QueryAsync(query, cancellationToken)
            .Select(x => x.ToDto())
            .ToArrayAsync(cancellationToken);

        return new Response(subjectCourses);
    }
}