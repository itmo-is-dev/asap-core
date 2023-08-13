using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;

namespace Itmo.Dev.Asap.Core.Application.Abstractions.SubjectCourses;

public interface ISubjectCourseService
{
    Task<SubjectCoursePointsDto> CalculatePointsAsync(Guid subjectCourseId, CancellationToken cancellationToken);
}