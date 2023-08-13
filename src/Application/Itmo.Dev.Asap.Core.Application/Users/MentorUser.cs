using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;

namespace Itmo.Dev.Asap.Core.Application.Users;

internal class MentorUser : ICurrentUser
{
    public MentorUser(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }

    public bool CanManageStudents => false;

    public bool CanUpdateAllDeadlines => false;

    public bool HasAccessToSubjectCourse(SubjectCourse subjectCourse)
    {
        return subjectCourse.Mentors.Any(m => m.UserId == Id);
    }

    public SubjectQuery.Builder FilterAvailableSubjects(SubjectQuery.Builder queryBuilder)
    {
        return queryBuilder.WithMentorId(Id);
    }

    public bool CanCreateUserWithRole(string roleName)
    {
        return false;
    }

    public bool CanChangeUserRole(string currentRoleName, string newRoleName)
    {
        return false;
    }
}