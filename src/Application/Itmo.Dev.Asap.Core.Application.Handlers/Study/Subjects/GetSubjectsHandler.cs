using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Study;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Queries.GetSubjects;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Subjects;

internal class GetSubjectsHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;
    private readonly ICurrentUser _currentUser;

    public GetSubjectsHandler(IPersistenceContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = SubjectQuery.Build(x => _currentUser.FilterAvailableSubjects(x));

        IAsyncEnumerable<Subject> subjects = _context.Subjects.QueryAsync(query, cancellationToken);

        SubjectDto[] dto = await subjects
            .Select(x => x.ToDto())
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new Response(dto);
    }
}