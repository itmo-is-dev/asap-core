using Itmo.Dev.Asap.Core.Application.Abstractions.Queue;
using Itmo.Dev.Asap.Core.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Contracts.Students.Notifications;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Assignments.Notifications;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Notifications;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.StudentGroups.Notifications;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourseGroups.Notifications;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Tables;

internal class TableUpdateNotificationHandler :
    INotificationHandler<AssignmentCreated.Notification>,
    INotificationHandler<AssignmentPointsUpdated.Notification>,
    INotificationHandler<GroupAssignmentDeadlineUpdated.Notification>,
    INotificationHandler<StudentGroupUpdated.Notification>,
    INotificationHandler<SubjectCourseGroupCreated.Notification>,
    INotificationHandler<SubjectCourseGroupDeleted.Notification>,
    INotificationHandler<DeadlinePolicyAdded.Notification>,
    INotificationHandler<SubmissionPointsUpdated.Notification>,
    INotificationHandler<SubmissionStateUpdated.Notification>,
    INotificationHandler<SubmissionUpdated.Notification>,
    INotificationHandler<StudentTransferred.Notification>
{
    private readonly IPersistenceContext _context;
    private readonly IQueueUpdateService _queueUpdateService;
    private readonly ISubjectCourseUpdateService _subjectCourseUpdateService;

    public TableUpdateNotificationHandler(
        IPersistenceContext context,
        IQueueUpdateService queueUpdateService,
        ISubjectCourseUpdateService subjectCourseUpdateService)
    {
        _context = context;
        _queueUpdateService = queueUpdateService;
        _subjectCourseUpdateService = subjectCourseUpdateService;
    }

    public Task Handle(AssignmentCreated.Notification notification, CancellationToken cancellationToken)
    {
        _subjectCourseUpdateService.UpdatePoints(notification.Assignment.SubjectCourseId);
        return Task.CompletedTask;
    }

    public Task Handle(AssignmentPointsUpdated.Notification notification, CancellationToken cancellationToken)
    {
        _subjectCourseUpdateService.UpdatePoints(notification.Assignment.SubjectCourseId);
        return Task.CompletedTask;
    }

    public async Task Handle(
        GroupAssignmentDeadlineUpdated.Notification notification,
        CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses.GetByAssignmentId(
            notification.GroupAssignment.AssignmentId,
            cancellationToken);

        _subjectCourseUpdateService.UpdatePoints(subjectCourse.Id);
        _queueUpdateService.Update(subjectCourse.Id, notification.GroupAssignment.GroupId);
    }

    public async Task Handle(StudentGroupUpdated.Notification notification, CancellationToken cancellationToken)
    {
        var query = SubjectCourseQuery.Build(x => x.WithStudentGroupId(notification.Group.Id));

        SubjectCourse[] courses = await _context.SubjectCourses
            .QueryAsync(query, cancellationToken)
            .ToArrayAsync(cancellationToken);

        foreach (SubjectCourse course in courses)
        {
            _subjectCourseUpdateService.UpdatePoints(course.Id);
        }
    }

    public Task Handle(SubjectCourseGroupCreated.Notification notification, CancellationToken cancellationToken)
    {
        _queueUpdateService.Update(notification.Group.SubjectCourseId, notification.Group.StudentGroupId);

        return Task.CompletedTask;
    }

    public Task Handle(SubjectCourseGroupDeleted.Notification notification, CancellationToken cancellationToken)
    {
        _queueUpdateService.Update(notification.SubjectCourseId, notification.GroupId);
        return Task.CompletedTask;
    }

    public Task Handle(DeadlinePolicyAdded.Notification notification, CancellationToken cancellationToken)
    {
        _subjectCourseUpdateService.UpdatePoints(notification.SubjectCourseId);
        return Task.CompletedTask;
    }

    public async Task Handle(SubmissionPointsUpdated.Notification notification, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses.GetByAssignmentId(
            notification.Submission.AssignmentId,
            cancellationToken);

        _subjectCourseUpdateService.UpdatePoints(subjectCourse.Id);
    }

    public async Task Handle(SubmissionStateUpdated.Notification notification, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses.GetByAssignmentId(
            notification.Submission.AssignmentId,
            cancellationToken);

        StudentGroup group = await _context.StudentGroups.GetByStudentId(
            notification.Submission.StudentId,
            cancellationToken);

        _queueUpdateService.Update(subjectCourse.Id, group.Id);
    }

    public async Task Handle(SubmissionUpdated.Notification notification, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses.GetByAssignmentId(
            notification.Submission.AssignmentId,
            cancellationToken);

        StudentGroup group = await _context.StudentGroups.GetByStudentId(
            notification.Submission.StudentId,
            cancellationToken);

        _subjectCourseUpdateService.UpdatePoints(subjectCourse.Id);
        _queueUpdateService.Update(subjectCourse.Id, group.Id);
    }

    public async Task Handle(StudentTransferred.Notification notification, CancellationToken cancellationToken)
    {
        var queryBuilder = new SubjectCourseQuery.Builder();

        queryBuilder.WithStudentGroupId(notification.NewGroupId);

        if (notification.OldGroupId is not null)
        {
            queryBuilder.WithStudentGroupId(notification.OldGroupId.Value);
        }

        IAsyncEnumerable<SubjectCourse> subjectCourses = _context.SubjectCourses
            .QueryAsync(queryBuilder.Build(), cancellationToken);

        IAsyncEnumerable<(Guid SubjectCourseId, Guid GroupId)> pairsEnumerable = subjectCourses.SelectMany(
            x => x.Groups.ToAsyncEnumerable(),
            (x, group) => (subjectCourseId: x.Id, group.Id));

        if (notification.OldGroupId is null)
        {
            pairsEnumerable = pairsEnumerable.Where(x => x.GroupId.Equals(notification.NewGroupId));
        }
        else
        {
            pairsEnumerable = pairsEnumerable.Where(x =>
                x.GroupId.Equals(notification.NewGroupId)
                || x.GroupId.Equals(notification.OldGroupId.Value));
        }

        (Guid SubjectCourseId, Guid GroupId)[] pairs = await pairsEnumerable.ToArrayAsync(cancellationToken);

        IEnumerable<Guid> subjectCourseIds = pairs
            .Select(x => x.SubjectCourseId)
            .Distinct();

        foreach (Guid subjectCourse in subjectCourseIds)
        {
            _subjectCourseUpdateService.UpdatePoints(subjectCourse);
        }

        foreach ((Guid subjectCourseId, Guid groupId) in pairs)
        {
            _queueUpdateService.Update(subjectCourseId, groupId);
        }
    }
}