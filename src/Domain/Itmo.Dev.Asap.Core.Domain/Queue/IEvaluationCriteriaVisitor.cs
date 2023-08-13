using Itmo.Dev.Asap.Core.Domain.Queue.EvaluationCriteria;
using Itmo.Dev.Asap.Core.Domain.Queue.Models;
using Itmo.Dev.Asap.Core.Domain.Submissions;

namespace Itmo.Dev.Asap.Core.Domain.Queue;

public interface IEvaluationCriteriaVisitor
{
    IAsyncEnumerable<EvaluatedSubmission> VisitAsync(
        IAsyncEnumerable<Submission> submissions,
        AssignmentDeadlineEvaluationCriteria criteria,
        CancellationToken cancellationToken);

    IAsyncEnumerable<EvaluatedSubmission> VisitAsync(
        IAsyncEnumerable<Submission> submissions,
        SubmissionDateTimeEvaluationCriteria criteria,
        CancellationToken cancellationToken);

    IAsyncEnumerable<EvaluatedSubmission> VisitAsync(
        IAsyncEnumerable<Submission> submissions,
        SubmissionStateEvaluationCriteria criteria,
        CancellationToken cancellationToken);
}