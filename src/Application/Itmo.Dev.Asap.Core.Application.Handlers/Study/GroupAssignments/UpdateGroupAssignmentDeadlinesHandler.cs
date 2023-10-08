using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Commands.UpdateGroupAssignmentDeadlines;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.GroupAssignments;

internal class UpdateGroupAssignmentDeadlinesHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;

    public UpdateGroupAssignmentDeadlinesHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        var query = GroupAssignmentQuery.Build(x => x
            .WithAssignmentId(request.AssignmentId)
            .WithGroupIds(request.GroupIds));

        GroupAssignment[] groupAssignments = await _context.GroupAssignments
            .QueryAsync(query, cancellationToken)
            .ToArrayAsync(cancellationToken);

        foreach (GroupAssignment groupAssignment in groupAssignments)
        {
            groupAssignment.Deadline = request.Deadline;
            _context.GroupAssignments.Update(groupAssignment);
        }

        await _context.SaveChangesAsync(cancellationToken);

        IEnumerable<GroupAssignmentDto> dto = groupAssignments.Select(x => x.ToDto());

        return new Response(dto);
    }
}