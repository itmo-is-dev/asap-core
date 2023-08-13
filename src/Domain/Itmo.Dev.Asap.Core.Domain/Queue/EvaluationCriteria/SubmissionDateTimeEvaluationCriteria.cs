using Itmo.Dev.Asap.Core.Domain.Queue.Models;
using Itmo.Dev.Asap.Core.Domain.Submissions;

namespace Itmo.Dev.Asap.Core.Domain.Queue.EvaluationCriteria;

public class SubmissionDateTimeEvaluationCriteria : IEvaluationCriteria
{
    public SubmissionDateTimeEvaluationCriteria(SortingOrder order)
    {
        Order = order;
    }

    public SortingOrder Order { get; }

    public IAsyncEnumerable<EvaluatedSubmission> AcceptAsync(
        IAsyncEnumerable<Submission> submissions,
        IEvaluationCriteriaVisitor visitor,
        CancellationToken cancellationToken)
    {
        return visitor.VisitAsync(submissions, this, cancellationToken);
    }
}