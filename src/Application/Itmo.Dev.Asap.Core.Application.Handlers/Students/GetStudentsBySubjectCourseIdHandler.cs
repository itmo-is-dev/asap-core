using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Students;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries.GetStudentsBySubjectCourseId;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Students;

internal class GetStudentsBySubjectCourseIdHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public GetStudentsBySubjectCourseIdHandler(IPersistenceContext context)
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