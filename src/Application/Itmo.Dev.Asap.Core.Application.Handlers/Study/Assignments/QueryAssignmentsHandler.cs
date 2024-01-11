using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Queries.QueryAssignments;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Assignments;

internal class QueryAssignmentsHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public QueryAssignmentsHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = AssignmentQuery.Build(x => x
            .WithIds(request.Ids)
            .WithNames(request.Names));

        AssignmentDto[] assignments = await _context.Assignments
            .QueryAsync(query, cancellationToken)
            .Select(x => x.ToDto())
            .ToArrayAsync(cancellationToken);

        return new Response(assignments);
    }
}