using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.StudyGroups.Queries.FindGroupsByQuery;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.StudyGroups;

internal class FindGroupsByQueryHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public FindGroupsByQueryHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = StudentGroupQuery.Build(x => x
            .WithNamePatterns(request.NamePatterns)
            .WithCursor(request.PageToken?.Id)
            .WithLimit(request.PageSize));

        StudyGroupDto[] dto = await _context.StudentGroups
            .QueryAsync(query, cancellationToken)
            .Select(x => x.ToDto())
            .ToArrayAsync(cancellationToken);

        return new Response(dto);
    }
}