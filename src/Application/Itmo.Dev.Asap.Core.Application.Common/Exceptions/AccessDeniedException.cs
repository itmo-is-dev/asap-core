namespace Itmo.Dev.Asap.Core.Application.Common.Exceptions;

public class AccessDeniedException : ApplicationException
{
    public AccessDeniedException()
        : base("Access denied") { }

    public AccessDeniedException(string message)
        : base(message) { }
}