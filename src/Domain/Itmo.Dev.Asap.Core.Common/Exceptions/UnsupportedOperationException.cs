namespace Itmo.Dev.Asap.Core.Common.Exceptions;

public class UnsupportedOperationException : DomainException
{
    public UnsupportedOperationException(string? message)
        : base(message) { }
}