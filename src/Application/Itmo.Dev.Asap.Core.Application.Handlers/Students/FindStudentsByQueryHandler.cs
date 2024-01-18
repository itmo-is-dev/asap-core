using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Students;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using Microsoft.Extensions.Logging;
using static Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries.FindStudentsByQuery;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Students;

internal class FindStudentsByQueryHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;
    private readonly ILogger<FindStudentsByQueryHandler> _logger;

    public FindStudentsByQueryHandler(IPersistenceContext context, ILogger<FindStudentsByQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = StudentQuery.Build(x => x
            .WithIds(request.Ids)
            .WithFullNamePatterns(request.NamePatterns)
            .WithGroupNamePatterns(request.GroupNamePatterns)
            .WithUniversityIds(request.UniversityIds)
            .WithCursor(request.PageToken?.StudentId)
            .WithLimit(request.PageSize));

        StudentDto[] dto = await _context.Students
            .QueryAsync(query, cancellationToken)
            .Select(x => x.ToDto())
            .ToArrayAsync(cancellationToken);

        _logger.LogTrace(
            "Finished students query = {Query}, found {Count} records",
            query,
            dto.Length);

        return new Response(dto);
    }
}