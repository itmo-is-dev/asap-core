using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using System.Security.Claims;

namespace Itmo.Dev.Asap.Core.Application.Users;

public class CurrentUserProxy : ICurrentUser, ICurrentUserManager
{
    private ICurrentUser _user = new AnonymousUser();

    public Guid Id => _user.Id;

    public bool CanManageStudents => _user.CanManageStudents;

    public bool CanUpdateAllDeadlines => _user.CanUpdateAllDeadlines;

    public SubjectQuery.Builder FilterAvailableSubjects(SubjectQuery.Builder queryBuilder)
    {
        return _user.FilterAvailableSubjects(queryBuilder);
    }

    public bool HasAccessToSubjectCourse(SubjectCourse subjectCourse)
    {
        return _user.HasAccessToSubjectCourse(subjectCourse);
    }

    public bool CanCreateUserWithRole(string roleName)
    {
        return _user.CanCreateUserWithRole(roleName);
    }

    public bool CanChangeUserRole(string currentRoleName, string newRoleName)
    {
        return _user.CanChangeUserRole(currentRoleName, newRoleName);
    }

    public void Authenticate(ClaimsPrincipal principal)
    {
        var roles = principal.Claims
            .Where(x => x.Type.Equals(ClaimTypes.Role, StringComparison.OrdinalIgnoreCase))
            .Select(x => x.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        string nameIdentifier = principal.Claims
            .Single(x => x.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))
            .Value;

        if (!Guid.TryParse(nameIdentifier, out Guid id))
        {
            throw new UnauthorizedException("Failed to parse user NameIdentifier to Guid");
        }

        if (roles.Contains(AsapIdentityRoleNames.AdminRoleName))
        {
            _user = new AdminUser(id);
        }
        else if (roles.Contains(AsapIdentityRoleNames.ModeratorRoleName))
        {
            _user = new ModeratorUser(id);
        }
        else if (roles.Contains(AsapIdentityRoleNames.MentorRoleName))
        {
            _user = new MentorUser(id);
        }
        else
        {
            _user = new AnonymousUser();
        }
    }
}