using Itmo.Dev.Asap.Core.Domain.Queue.Models;
using Itmo.Dev.Asap.Core.Domain.Submissions;

namespace Itmo.Dev.Asap.Core.Domain.Queue;

public interface IEvaluationCriteria
{
    SortingOrder Order { get; }

    IAsyncEnumerable<EvaluatedSubmission> AcceptAsync(
        IAsyncEnumerable<Submission> submissions,
        IEvaluationCriteriaVisitor visitor,
        CancellationToken cancellationToken);
}