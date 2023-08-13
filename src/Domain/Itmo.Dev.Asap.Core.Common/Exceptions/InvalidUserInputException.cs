using Itmo.Dev.Asap.Core.Common.Resources;

namespace Itmo.Dev.Asap.Core.Common.Exceptions;

public class InvalidUserInputException : DomainException
{
    public InvalidUserInputException(string? message)
        : base(message) { }

    public static InvalidUserInputException FailedToParseUserCommand()
    {
        return new InvalidUserInputException(UserMessages.FailedToParseUserCommand);
    }
}