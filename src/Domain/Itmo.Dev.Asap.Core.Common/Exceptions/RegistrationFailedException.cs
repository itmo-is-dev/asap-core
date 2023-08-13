namespace Itmo.Dev.Asap.Core.Common.Exceptions;

public class RegistrationFailedException : DomainException
{
    public RegistrationFailedException(string? message) : base(message) { }
}