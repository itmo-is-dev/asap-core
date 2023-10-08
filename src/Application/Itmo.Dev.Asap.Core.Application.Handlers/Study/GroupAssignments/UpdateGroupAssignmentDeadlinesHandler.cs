using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Commands.UpdateGroupAssignmentDeadlines;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.GroupAssignments;

internal class UpdateGroupAssignmentDeadlinesHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly IPublisher _publisher;

    public UpdateGroupAssignmentDeadlinesHandler(
        IPersistenceContext context,
        ICurrentUser currentUser,
        IPublisher publisher)
    {
        _context = context;
        _currentUser = currentUser;
        _publisher = publisher;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetByAssignmentId(request.AssignmentId, cancellationToken);

        if (_currentUser.CanUpdateAllDeadlines is false)
        {
            var mentorQuery = MentorQuery.Build(x => x
                .WithUserId(_currentUser.Id)
                .WithSubjectCourseId(subjectCourse.Id));

            bool isMentor = await _context.Mentors
                .QueryAsync(mentorQuery, cancellationToken)
                .AnyAsync(cancellationToken);

            if (isMentor is false)
                return new Response.Unauthorized();
        }

        var query = GroupAssignmentQuery.Build(x => x
            .WithAssignmentId(request.AssignmentId)
            .WithGroupIds(request.GroupIds));

        GroupAssignment[] groupAssignments = await _context.GroupAssignments
            .QueryAsync(query, cancellationToken)
            .ToArrayAsync(cancellationToken);

        foreach (GroupAssignment groupAssignment in groupAssignments)
        {
            groupAssignment.Deadline = request.Deadline;
            _context.GroupAssignments.Update(groupAssignment);
        }

        await _context.SaveChangesAsync(cancellationToken);

        GroupAssignmentDto[] dto = groupAssignments.Select(x => x.ToDto()).ToArray();

        foreach (GroupAssignmentDto groupAssignment in dto)
        {
            var notification = new GroupAssignmentDeadlineUpdated.Notification(groupAssignment);
            await _publisher.Publish(notification, default);
        }

        return new Response.Success(dto);
    }
}