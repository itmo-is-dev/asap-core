using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Queries.GetSubjectCourseGroupsBySubjectCourseId;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourseGroups;

internal class GetSubjectCourseGroupsBySubjectCourseIdHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public GetSubjectCourseGroupsBySubjectCourseIdHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByIdAsync(request.SubjectCourseId, cancellationToken);

        SubjectCourseGroupDto[] subjectCourseGroups = subjectCourse.Groups
            .Select(group => new SubjectCourseGroupDto(subjectCourse.Id, group.Id, group.Name))
            .OrderBy(x => x.StudentGroupName)
            .ToArray();

        return new Response(subjectCourseGroups);
    }
}