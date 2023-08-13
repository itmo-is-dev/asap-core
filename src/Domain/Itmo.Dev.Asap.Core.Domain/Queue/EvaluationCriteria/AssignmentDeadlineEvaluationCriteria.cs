using Itmo.Dev.Asap.Core.Domain.Queue.Models;
using Itmo.Dev.Asap.Core.Domain.Submissions;

namespace Itmo.Dev.Asap.Core.Domain.Queue.EvaluationCriteria;

public class AssignmentDeadlineEvaluationCriteria : IEvaluationCriteria
{
    public AssignmentDeadlineEvaluationCriteria(SortingOrder order)
    {
        Order = order;
    }

    public const double CurrentAssignmentPriority = 3;
    public const double ProperlySubmittedAssignmentPriority = 2;
    public const double ExpiredAssignmentPriority = 1;
    public const double OtherAssignmentPriority = 0;

    public SortingOrder Order { get; }

    public IAsyncEnumerable<EvaluatedSubmission> AcceptAsync(
        IAsyncEnumerable<Submission> submissions,
        IEvaluationCriteriaVisitor visitor,
        CancellationToken cancellationToken)
    {
        return visitor.VisitAsync(submissions, this, cancellationToken);
    }
}