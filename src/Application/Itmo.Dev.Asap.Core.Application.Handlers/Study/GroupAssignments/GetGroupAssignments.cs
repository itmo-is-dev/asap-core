using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Queries.GetGroupAssignments;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.GroupAssignments;

internal class GetGroupAssignments : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public GetGroupAssignments(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = GroupAssignmentQuery.Build(x => x.WithAssignmentId(request.AssignmentId));

        IAsyncEnumerable<GroupAssignment> assignments = _context.GroupAssignments
            .QueryAsync(query, cancellationToken);

        GroupAssignmentDto[] dto = await assignments
            .Select(x => x.ToDto())
            .OrderBy(x => x.GroupName)
            .ToArrayAsync(cancellationToken);

        return new Response(dto);
    }
}