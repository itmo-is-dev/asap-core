using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Students;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries.FindStudentsByQuery;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Students;

internal class FindStudentsByQueryHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public FindStudentsByQueryHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = StudentQuery.Build(x => x
            .WithFullNamePatterns(request.NamePatterns)
            .WithGroupNamePatterns(request.GroupNamePatterns)
            .WithUniversityIds(request.UniversityIds)
            .WithCursor(request.PageToken?.StudentId)
            .WithLimit(request.PageSize));

        StudentDto[] dto = await _context.Students
            .QueryAsync(query, cancellationToken)
            .Select(x => x.ToDto())
            .ToArrayAsync(cancellationToken);

        return new Response(dto);
    }
}