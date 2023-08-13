using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;

namespace Itmo.Dev.Asap.Core.Application.Abstractions.Identity;

public interface ICurrentUser
{
    Guid Id { get; }

    bool CanManageStudents { get; }

    bool HasAccessToSubjectCourse(SubjectCourse subjectCourse);

    public SubjectQuery.Builder FilterAvailableSubjects(SubjectQuery.Builder queryBuilder);

    bool CanUpdateAllDeadlines { get; }

    bool CanCreateUserWithRole(string roleName);

    bool CanChangeUserRole(string currentRoleName, string newRoleName);
}