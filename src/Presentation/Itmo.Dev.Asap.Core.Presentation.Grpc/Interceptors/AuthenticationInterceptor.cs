using Grpc.Core;
using Grpc.Core.Interceptors;
using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Interceptors;

public class AuthenticationInterceptor : Interceptor
{
    private readonly ICurrentUserManager _currentUserManager;

    public AuthenticationInterceptor(ICurrentUserManager currentUserManager)
    {
        _currentUserManager = currentUserManager;
    }

    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        Metadata.Entry? authorizationHeader = context.RequestHeaders.FirstOrDefault(
            x => x.Key.Equals("authorization", StringComparison.OrdinalIgnoreCase));

        if (authorizationHeader is null)
            return continuation.Invoke(request, context);

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken token = handler.ReadJwtToken(authorizationHeader.Value);

        var principal = new ClaimsPrincipal(new ClaimsIdentity(token.Claims));
        _currentUserManager.Authenticate(principal);

        return continuation.Invoke(request, context);
    }
}