namespace Itmo.Dev.Asap.Core.Domain.Queue.FilterCriteria;

public class StudentGroupFilterCriteria : IFilterCriteria
{
    public StudentGroupFilterCriteria(IReadOnlyCollection<Guid> studentGroupIds)
    {
        StudentGroupIds = studentGroupIds;
    }

    public IReadOnlyCollection<Guid> StudentGroupIds { get; }

    public void Accept(IFilterCriteriaVisitor visitor)
    {
        visitor.Visit(this);
    }
}