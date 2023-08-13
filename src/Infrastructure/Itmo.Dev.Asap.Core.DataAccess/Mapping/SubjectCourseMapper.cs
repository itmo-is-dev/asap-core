using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePenalties;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Users;

namespace Itmo.Dev.Asap.Core.DataAccess.Mapping;

public static class SubjectCourseMapper
{
    public static SubjectCourse MapTo(
        SubjectCourseModel model,
        HashSet<StudentGroupInfo> groups,
        HashSet<DeadlinePenalty> penalties,
        HashSet<SubjectCourseAssignment> assignments,
        HashSet<Mentor> mentors)
    {
        return new SubjectCourse(
            model.Id,
            model.SubjectId,
            model.Title,
            model.WorkflowType,
            groups,
            penalties,
            assignments,
            mentors);
    }

    public static SubjectCourseModel MapFrom(SubjectCourse entity)
    {
        return new SubjectCourseModel(entity.Id, entity.SubjectId, entity.Title, entity.WorkflowType);
    }
}