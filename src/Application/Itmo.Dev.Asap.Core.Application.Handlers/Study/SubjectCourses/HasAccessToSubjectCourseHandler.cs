using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries.HasAccessToSubjectCourse;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;

internal class HasAccessToSubjectCourseHandler : IRequestHandler<Query, Response>
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersistenceContext _context;

    public HasAccessToSubjectCourseHandler(ICurrentUser currentUser, IPersistenceContext context)
    {
        _currentUser = currentUser;
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByIdAsync(request.SubjectCourseId, cancellationToken);

        bool hasAccess = _currentUser.HasAccessToSubjectCourse(subjectCourse);

        return new Response(hasAccess);
    }
}