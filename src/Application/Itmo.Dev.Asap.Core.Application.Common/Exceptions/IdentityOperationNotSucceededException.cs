namespace Itmo.Dev.Asap.Core.Application.Common.Exceptions;

public class IdentityOperationNotSucceededException : ApplicationException
{
    public IdentityOperationNotSucceededException(string? message)
        : base(message) { }
}