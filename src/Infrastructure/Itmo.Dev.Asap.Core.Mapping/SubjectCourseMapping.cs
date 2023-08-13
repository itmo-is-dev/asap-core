using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class SubjectCourseMapping
{
    public static SubjectCourseDto ToDto(this SubjectCourse subjectCourse)
    {
        return new SubjectCourseDto(
            subjectCourse.Id,
            subjectCourse.SubjectId,
            subjectCourse.Title,
            subjectCourse.WorkflowType?.AsDto());
    }
}