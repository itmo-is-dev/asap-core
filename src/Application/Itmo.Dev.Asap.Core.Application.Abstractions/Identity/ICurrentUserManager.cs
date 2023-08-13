using System.Security.Claims;

namespace Itmo.Dev.Asap.Core.Application.Abstractions.Identity;

public interface ICurrentUserManager
{
    void Authenticate(ClaimsPrincipal principal);
}