using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Students;
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
        IAsyncEnumerable<Student> students = _context.Students
            .GetStudentsBySubjectCourseIdAsync(request.SubjectCourseId, cancellationToken);

        Guid[] ids = await students.Select(x => x.UserId).ToArrayAsync(cancellationToken);

        return new Response(ids);
    }
}