using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.Common.Exceptions;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries.GetSubjectCourseById;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;

internal class GetSubjectCourseByIdHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;
    private readonly ICurrentUser _currentUser;

    public GetSubjectCourseByIdHandler(
        IPersistenceContext context,
        ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByIdAsync(request.Id, cancellationToken);

        if (_currentUser.HasAccessToSubjectCourse(subjectCourse) is false)
            throw UserHasNotAccessException.AccessViolation(_currentUser.Id);

        return new Response(subjectCourse.ToDto());
    }
}