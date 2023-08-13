using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.SubmissionStateWorkflows;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands.CreateSubjectCourse;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;

internal class CreateSubjectCourseHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IPublisher _publisher;

    public CreateSubjectCourseHandler(IPersistenceContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Subject subject = await _context.Subjects.GetByIdAsync(request.SubjectId, cancellationToken);
        SubmissionStateWorkflowType workflowType = request.WorkflowType.AsValueObject();

        var subjectCourseBuilder = new SubjectCourseBuilder(
            Guid.NewGuid(),
            request.Title,
            workflowType);

        SubjectCourse subjectCourse = subject.AddCourse(subjectCourseBuilder);

        _context.SubjectCourses.Add(subjectCourse);
        await _context.SaveChangesAsync(cancellationToken);

        SubjectCourseDto dto = subjectCourse.ToDto();

        var notification = new SubjectCourseCreated.Notification(dto, request.CorrelationId);
        await _publisher.Publish(notification, default);

        return new Response(dto);
    }
}