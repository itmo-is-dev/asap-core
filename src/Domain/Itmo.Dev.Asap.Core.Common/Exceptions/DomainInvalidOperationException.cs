using Itmo.Dev.Asap.Core.Common.Resources;

namespace Itmo.Dev.Asap.Core.Common.Exceptions;

public class DomainInvalidOperationException : DomainException
{
    public DomainInvalidOperationException(string? message)
        : base(message) { }

    public static DomainInvalidOperationException UserNotFoundByGithubUsername(string githubUsername)
    {
        return new DomainInvalidOperationException(string.Format(
            UserMessages.UserNotFoundByGithubUsername,
            githubUsername));
    }

    public static DomainInvalidOperationException RepositoryAssignedToAnotherUserClosePullRequest(
        string repository,
        string sender)
    {
        return new DomainInvalidOperationException(
            string.Format(UserMessages.RepositoryAssignedToAnotherUserClosePullRequest, repository, sender));
    }

    public static DomainInvalidOperationException RepositoryAssignedToAnotherUserSubmissionUpdated(
        string repository,
        string sender)
    {
        return new DomainInvalidOperationException(
            string.Format(UserMessages.RepositoryAssignedToAnotherUserSubmissionUpdated, repository, sender));
    }
}