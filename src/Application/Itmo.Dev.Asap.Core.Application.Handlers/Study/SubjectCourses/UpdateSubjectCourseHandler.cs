using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands.UpdateSubjectCourse;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;

internal class UpdateSubjectCourseHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;

    public UpdateSubjectCourseHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses.GetByIdAsync(request.Id, cancellationToken);
        TitleUpdatedEvent evt = subjectCourse.UpdateTitle(request.Title);

        await _context.SubjectCourses.ApplyAsync(evt, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response(subjectCourse.ToDto());
    }
}