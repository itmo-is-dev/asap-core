using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.Common.Exceptions;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Study;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Subjects.Queries.GetSubjectById;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Subjects;

internal class GetSubjectByIdHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;
    private readonly ICurrentUser _currentUser;

    public GetSubjectByIdHandler(IPersistenceContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = SubjectQuery.Build(x => _currentUser.FilterAvailableSubjects(x).WithId(request.Id));

        Subject? subject = await _context.Subjects
            .QueryAsync(query, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (subject is null)
            throw UserHasNotAccessException.AccessViolation(_currentUser.Id);

        return new Response(subject.ToDto());
    }
}