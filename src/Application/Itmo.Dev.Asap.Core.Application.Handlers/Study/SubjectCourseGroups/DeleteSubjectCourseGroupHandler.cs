using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Commands.DeleteSubjectCourseGroup;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourseGroups;

internal class DeleteSubjectCourseGroupHandler : IRequestHandler<Command>
{
    private readonly IPersistenceContext _context;

    public DeleteSubjectCourseGroupHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByIdAsync(request.SubjectCourseId, cancellationToken);

        ISubjectCourseEvent evt = subjectCourse.RemoveGroup(request.StudentGroupId);

        await _context.SubjectCourses.ApplyAsync(evt, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}