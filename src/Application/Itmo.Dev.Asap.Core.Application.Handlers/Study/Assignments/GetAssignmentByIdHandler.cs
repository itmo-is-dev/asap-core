using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Queries.GetAssignmentById;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Assignments;

internal class GetAssignmentByIdHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public GetAssignmentByIdHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        Assignment assignment = await _context.Assignments.GetByIdAsync(request.Id, cancellationToken);
        AssignmentDto dto = assignment.ToDto();

        return new Response(dto);
    }
}