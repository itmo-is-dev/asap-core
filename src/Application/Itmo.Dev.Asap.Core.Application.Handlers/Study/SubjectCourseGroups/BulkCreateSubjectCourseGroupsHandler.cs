using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Study;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Commands.BulkCreateSubjectCourseGroups;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourseGroups;

internal class BulkCreateSubjectCourseGroupsHandler : IRequestHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IPublisher _publisher;

    public BulkCreateSubjectCourseGroupsHandler(IPersistenceContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        SubjectCourse course = await _context.SubjectCourses
            .GetByIdAsync(request.SubjectCourseId, cancellationToken);

        IEnumerable<Guid> groupsToCreateIds = request.StudentGroupIds.Except(course.Groups.Select(x => x.Id));

        var studentGroupsQuery = StudentGroupQuery.Build(x => x.WithIds(groupsToCreateIds));

        StudentGroup[] studentGroups = await _context.StudentGroups
            .QueryAsync(studentGroupsQuery, cancellationToken)
            .ToArrayAsync(cancellationToken);

        (IReadOnlyCollection<SubjectCourseGroup> groups, ISubjectCourseEvent evt) = course.AddGroups(studentGroups);

        await _context.SubjectCourses.ApplyAsync(evt, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        SubjectCourseGroupDto[] dtos = groups.Select(x => x.ToDto()).ToArray();

        IEnumerable<SubjectCourseGroupCreated.Notification> notifications = dtos
            .Select(g => new SubjectCourseGroupCreated.Notification(g));

        IEnumerable<Task> tasks = notifications.Select(x => _publisher.Publish(x, default));
        await Task.WhenAll(tasks);

        return new Response(dtos);
    }
}