namespace Itmo.Dev.Asap.Core.Common.Exceptions;

public class AsapIdentityException : DomainException
{
    public AsapIdentityException(string? message)
        : base(message) { }
}