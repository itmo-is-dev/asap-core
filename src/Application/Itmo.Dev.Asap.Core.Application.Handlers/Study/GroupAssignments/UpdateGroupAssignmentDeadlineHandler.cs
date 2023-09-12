using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.Common.Exceptions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Commands.UpdateGroupAssignmentDeadline;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.GroupAssignments;

internal class UpdateGroupAssignmentDeadlineHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IPublisher _publisher;
    private readonly ICurrentUser _currentUser;

    public UpdateGroupAssignmentDeadlineHandler(
        IPersistenceContext context,
        IPublisher publisher,
        ICurrentUser currentUser)
    {
        _context = context;
        _publisher = publisher;
        _currentUser = currentUser;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        GroupAssignment groupAssignment = await _context.GroupAssignments
            .GetByIdAsync(request.GroupId, request.AssignmentId, cancellationToken);

        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByAssignmentId(groupAssignment.Id.AssignmentId, cancellationToken);

        if (_currentUser.CanUpdateAllDeadlines is false)
        {
            var mentorQuery = MentorQuery.Build(x => x
                .WithUserId(_currentUser.Id)
                .WithSubjectCourseId(subjectCourse.Id));

            bool isMentor = await _context.Mentors
                .QueryAsync(mentorQuery, cancellationToken)
                .AnyAsync(cancellationToken);

            if (isMentor is false)
                throw new AccessDeniedException();
        }

        groupAssignment.Deadline = request.Deadline;

        _context.GroupAssignments.Update(groupAssignment);
        await _context.SaveChangesAsync(cancellationToken);

        GroupAssignmentDto dto = groupAssignment.ToDto();

        var notification = new GroupAssignmentDeadlineUpdated.Notification(dto);
        await _publisher.PublishAsync(notification, cancellationToken);

        return new Response(dto);
    }
}