using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Mapping;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.DataAccess.Models.DeadlinePenalties;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.DataAccess.Tools;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses.Events;
using Itmo.Dev.Asap.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Itmo.Dev.Asap.Core.DataAccess.Repositories.SubjectCourses;

public class SubjectCourseRepositoryEventVisitor : ISubjectCourseEventVisitor
{
    private readonly DatabaseContext _context;

    public SubjectCourseRepositoryEventVisitor(DatabaseContext context)
    {
        _context = context;
    }

    public ValueTask VisitAsync(SubjectCourseGroupCreatedEvent evt, CancellationToken cancellationToken)
    {
        SubjectCourseGroupModel model = SubjectCourseGroupMapper.MapFrom(evt.SubjectCourseGroup);
        _context.SubjectCourseGroups.Add(model);

        return ValueTask.CompletedTask;
    }

    public ValueTask VisitAsync(GroupAssignmentCreatedEvent evt, CancellationToken cancellationToken)
    {
        GroupAssignmentModel model = GroupAssignmentMapper.MapFrom(evt.GroupAssignment);
        _context.GroupAssignments.Add(model);

        return ValueTask.CompletedTask;
    }

    public ValueTask VisitAsync(SubjectCourseGroupRemovedEvent evt, CancellationToken cancellationToken)
    {
        EntityEntry<SubjectCourseGroupModel> entry = RepositoryTools.GetEntry(
            _context,
            x => x.SubjectCourseId.Equals(evt.SubjectCourse.Id) && x.StudentGroupId.Equals(evt.StudentGroupId),
            () => new SubjectCourseGroupModel(evt.SubjectCourse.Id, evt.StudentGroupId));

        entry.State = EntityState.Deleted;

        return ValueTask.CompletedTask;
    }

    public ValueTask VisitAsync(SubjectCourseAssignmentCreatedEvent evt, CancellationToken cancellationToken)
    {
        AssignmentModel model = AssignmentMapper.MapFrom(evt.Assignment, evt.SubjectCourse.Id);
        _context.Assignments.Add(model);

        return ValueTask.CompletedTask;
    }

    public ValueTask VisitAsync(DeadlinePenaltyAddedEvent evt, CancellationToken cancellationToken)
    {
        DeadlinePenaltyModel model = DeadlinePenaltyMapper.MapFrom(evt.Penalty, id: null, evt.SubjectCourse.Id);
        _context.DeadlinePenalties.Add(model);

        return ValueTask.CompletedTask;
    }

    public ValueTask VisitAsync(MentorAddedEvent evt, CancellationToken cancellationToken)
    {
        MentorModel model = MentorMapper.MapFrom(evt.Mentor);
        _context.Mentors.Add(model);

        return ValueTask.CompletedTask;
    }

    public ValueTask VisitAsync(MentorRemovedEvent evt, CancellationToken cancellationToken)
    {
        Mentor entity = evt.Mentor;

        EntityEntry<MentorModel> entry = RepositoryTools.GetEntry(
            _context,
            x => x.UserId.Equals(entity.UserId) && x.SubjectCourseId.Equals(entity.SubjectCourseId),
            () => MentorMapper.MapFrom(entity));

        entry.State = EntityState.Deleted;

        return ValueTask.CompletedTask;
    }

    public ValueTask VisitAsync(TitleUpdatedEvent evt, CancellationToken cancellationToken)
    {
        EntityEntry<SubjectCourseModel> entry = RepositoryTools.GetEntry(
            _context,
            x => x.Id.Equals(evt.SubjectCourse.Id),
            () => SubjectCourseMapper.MapFrom(evt.SubjectCourse));

        entry.Entity.Title = evt.Title;
        entry.State = EntityState.Modified;

        return ValueTask.CompletedTask;
    }
}