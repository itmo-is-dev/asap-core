using Itmo.Dev.Asap.Core.Common.Resources;

namespace Itmo.Dev.Asap.Core.Common.Exceptions;

public class StudentAssignmentException : DomainException
{
    private StudentAssignmentException(string message) : base(message) { }

    public static StudentAssignmentException StudentGroupAssignmentMismatch(string student, string group)
    {
        string message = string.Format(UserMessages.StudentGroupAssignmentMismatch, student, group);
        return new StudentAssignmentException(message);
    }
}