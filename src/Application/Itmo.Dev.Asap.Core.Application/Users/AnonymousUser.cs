using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.Common.Exceptions;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;

namespace Itmo.Dev.Asap.Core.Application.Users;

internal class AnonymousUser : ICurrentUser
{
#pragma warning disable CA1065
    public Guid Id => throw new UnauthorizedException("Tried to access anonymous user Id");
#pragma warning restore CA1065

    public bool CanUpdateAllDeadlines => false;

    public bool CanManageStudents => false;

    public SubjectQuery.Builder FilterAvailableSubjects(SubjectQuery.Builder queryBuilder)
        => throw UserHasNotAccessException.AnonymousUserHasNotAccess();

    public bool CanCreateUserWithRole(string roleName)
        => throw UserHasNotAccessException.AnonymousUserHasNotAccess();

    public bool CanChangeUserRole(string currentRoleName, string newRoleName)
        => throw UserHasNotAccessException.AnonymousUserHasNotAccess();

    public bool HasAccessToSubjectCourse(SubjectCourse subjectCourse)
        => throw UserHasNotAccessException.AnonymousUserHasNotAccess();
}