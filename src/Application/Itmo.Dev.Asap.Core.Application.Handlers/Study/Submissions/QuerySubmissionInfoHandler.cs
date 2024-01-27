using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Queries.QuerySubmissionInfo;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Submissions;

internal class QuerySubmissionInfoHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public QuerySubmissionInfoHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = SubmissionQuery.Build(x => x.WithIds(request.Ids));

        SubmissionInfoDto[] submissions = await _context.Submissions
            .QueryAsync(query, cancellationToken)
            .Select(x => x.ToInfoDto())
            .ToArrayAsync(cancellationToken);

        return new Response(submissions);
    }
}