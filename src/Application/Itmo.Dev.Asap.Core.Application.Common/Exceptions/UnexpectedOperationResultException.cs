namespace Itmo.Dev.Asap.Core.Application.Common.Exceptions;

public class UnexpectedOperationResultException : ApplicationException
{
    public const string ErrorMessage = "Operation finished with unexpected result";

    public UnexpectedOperationResultException() : base(ErrorMessage) { }
}