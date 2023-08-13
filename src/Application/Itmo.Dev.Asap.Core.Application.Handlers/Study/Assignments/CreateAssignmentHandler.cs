using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;
using Itmo.Dev.Asap.Core.Domain.ValueObject;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Commands.CreateAssignment;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Assignments;

internal class CreateAssignmentHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IPublisher _publisher;

    public CreateAssignmentHandler(IPersistenceContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByIdAsync(request.SubjectCourseId, cancellationToken);

        var assignmentBuilder = new Assignment(
            Guid.NewGuid(),
            request.Title,
            request.ShortName,
            request.Order,
            new Points(request.MinPoints),
            new Points(request.MaxPoints),
            subjectCourse.Id);

        (ISubjectCourseEvent evt, Assignment assignment) = subjectCourse.AddAssignment(assignmentBuilder);

        await _context.SubjectCourses.ApplyAsync(evt, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        AssignmentDto dto = assignment.ToDto();

        var notification = new AssignmentCreated.Notification(dto);
        await _publisher.PublishAsync(notification, cancellationToken);

        return new Response(dto);
    }
}