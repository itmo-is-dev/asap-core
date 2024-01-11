using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Queries.QuerySubjectCourseGroups;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourseGroups;

internal class QuerySubjectCourseGroupsHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public QuerySubjectCourseGroupsHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = StudentGroupQuery.Build(x => x
            .WithSubjectCourseId(request.SubjectCourseId)
            .WithIds(request.Ids)
            .WithNamePatterns(request.Names));

        SubjectCourseGroupDto[] groups = await _context.StudentGroups
            .QueryAsync(query, cancellationToken)
            .Select(x => new SubjectCourseGroupDto(request.SubjectCourseId, x.Id, x.Name))
            .ToArrayAsync(cancellationToken);

        return new Response(groups);
    }
}