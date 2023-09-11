using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Queries.GetSubjectCourseStudents;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;

internal class GetSubjectCourseStudentsHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public GetSubjectCourseStudentsHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        StudentDto[] dto = await _context.Students
            .GetStudentsBySubjectCourseIdAsync(request.SubjectCourseId, cancellationToken)
            .Select(x => x.ToDto())
            .ToArrayAsync(cancellationToken);

        return new Response(dto);
    }
}