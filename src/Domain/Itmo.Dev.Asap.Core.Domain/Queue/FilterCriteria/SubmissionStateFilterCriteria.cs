using Itmo.Dev.Asap.Core.Domain.Models;

namespace Itmo.Dev.Asap.Core.Domain.Queue.FilterCriteria;

public class SubmissionStateFilterCriteria : IFilterCriteria
{
    public SubmissionStateFilterCriteria(params SubmissionStateKind[] states)
    {
        States = states;
    }

    public IReadOnlyCollection<SubmissionStateKind> States { get; }

    public void Accept(IFilterCriteriaVisitor visitor)
    {
        visitor.Visit(this);
    }
}