using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Commands.UpdateStudentGroup;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.StudyGroups;

internal class UpdateStudyGroupHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IPublisher _publisher;

    public UpdateStudyGroupHandler(IPersistenceContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        StudentGroup studentGroup = await _context.StudentGroups.GetByIdAsync(request.Id, cancellationToken);
        studentGroup.Name = request.Name;

        _context.StudentGroups.Update(studentGroup);
        await _context.SaveChangesAsync(cancellationToken);

        StudentGroupDto dto = studentGroup.ToDto();

        var notification = new StudentGroupUpdated.Notification(dto);
        await _publisher.PublishAsync(notification, cancellationToken);

        return new Response(dto);
    }
}