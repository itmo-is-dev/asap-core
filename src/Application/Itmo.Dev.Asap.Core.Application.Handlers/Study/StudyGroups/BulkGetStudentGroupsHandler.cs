using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Queries.BulkGetStudyGroups;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.StudyGroups;

internal class BulkGetStudentGroupsHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public BulkGetStudentGroupsHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = StudentGroupQuery.Build(x => x.WithIds(request.Ids));

        IAsyncEnumerable<StudentGroup> groups = _context.StudentGroups.QueryAsync(query, cancellationToken);
        StudyGroupDto[] dto = await groups.Select(x => x.ToDto()).ToArrayAsync(cancellationToken);

        return new Response(dto);
    }
}