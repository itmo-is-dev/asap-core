namespace Itmo.Dev.Asap.Core.Domain.Study.Assignments.Results;

public abstract record UpdatePointsResult
{
    private UpdatePointsResult() { }

    public sealed record Success : UpdatePointsResult;

    public sealed record MaxPointsLessThanMinPoints : UpdatePointsResult;
}