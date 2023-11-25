using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries.GetSubjectCourseMentors;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;

internal class GetSubjectCourseMentorsHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public GetSubjectCourseMentorsHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        Guid[] mentors = await _context.Mentors
            .QueryAsync(MentorQuery.Build(x => x.WithSubjectCourseId(request.SubjectCourseId)), cancellationToken)
            .Select(x => x.UserId)
            .ToArrayAsync(cancellationToken);

        return new Response(mentors);
    }
}