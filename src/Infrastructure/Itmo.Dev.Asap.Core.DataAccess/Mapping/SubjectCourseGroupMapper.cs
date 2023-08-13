using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Study;

namespace Itmo.Dev.Asap.Core.DataAccess.Mapping;

public static class SubjectCourseGroupMapper
{
    public static SubjectCourseGroup MapTo(SubjectCourseGroupModel model)
    {
        var groupInfo = new StudentGroupInfo(model.StudentGroupId, model.StudentGroup.Name);
        return new SubjectCourseGroup(model.SubjectCourseId, groupInfo);
    }

    public static SubjectCourseGroupModel MapFrom(SubjectCourseGroup entity)
    {
        return new SubjectCourseGroupModel(
            subjectCourseId: entity.SubjectCourseId,
            studentGroupId: entity.StudentGroupId);
    }
}