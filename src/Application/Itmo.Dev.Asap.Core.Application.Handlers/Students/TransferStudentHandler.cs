using Itmo.Dev.Asap.Core.Application.Contracts.Students.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Students.Commands.TransferStudent;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Students;

internal class TransferStudentHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IPublisher _publisher;

    public TransferStudentHandler(
        IPersistenceContext context,
        IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Student student = await _context.Students.GetByIdAsync(request.StudentId, cancellationToken);
        StudentGroup group = await _context.StudentGroups.GetByIdAsync(request.GroupId, cancellationToken);

        StudentGroup? oldGroup = student.Group is null
            ? null
            : await _context.StudentGroups.GetByIdAsync(student.Group.Id, cancellationToken);

        (student, IStudentEvent evt) = student.TransferToAnotherGroup(oldGroup, group);

        await _context.Students.ApplyAsync(evt, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        StudentDto dto = student.ToDto();

        var notification = new StudentTransferred.Notification(
            dto.User.Id,
            request.GroupId,
            oldGroup?.Id);

        await _publisher.PublishAsync(notification, cancellationToken);

        return new Response(dto);
    }
}