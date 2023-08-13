using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries.GetStudentsByGroupId;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Students;

internal class GetStudentsByGroupIdHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public GetStudentsByGroupIdHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = StudentQuery.Build(x => x.WithGroupId(request.GroupId));

        StudentDto[] dto = await _context.Students
            .QueryAsync(query, cancellationToken)
            .Select(x => x.ToDto())
            .ToArrayAsync(cancellationToken);

        return new Response(dto);
    }
}