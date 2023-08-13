using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Study;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class SubjectCourseGroupMapping
{
    public static SubjectCourseGroupDto ToDto(this SubjectCourseGroup subjectCourseGroup)
    {
        return new SubjectCourseGroupDto(
            subjectCourseGroup.SubjectCourseId,
            subjectCourseGroup.StudentGroupId,
            subjectCourseGroup.StudentGroup.Name);
    }
}