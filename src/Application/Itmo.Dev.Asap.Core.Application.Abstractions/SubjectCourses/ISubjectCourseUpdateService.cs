namespace Itmo.Dev.Asap.Core.Application.Abstractions.SubjectCourses;

public interface ISubjectCourseUpdateService
{
    void UpdatePoints(Guid subjectCourseId);
}