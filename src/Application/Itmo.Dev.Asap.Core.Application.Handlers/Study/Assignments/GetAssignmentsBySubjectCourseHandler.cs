using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Models;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Queries.GetAssignmentsBySubjectCourse;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Assignments;

internal class GetAssignmentsBySubjectCourseHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public GetAssignmentsBySubjectCourseHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = AssignmentQuery.Build(x => x
            .WithSubjectCourseId(request.SubjectCourseId)
            .WithOrderByOrder(OrderDirection.Ascending));

        IAsyncEnumerable<Assignment> assignments = _context.Assignments
            .QueryAsync(query, cancellationToken);

        AssignmentDto[] dto = await assignments
            .Select(x => x.ToDto())
            .ToArrayAsync(cancellationToken);

        return new Response(dto);
    }
}