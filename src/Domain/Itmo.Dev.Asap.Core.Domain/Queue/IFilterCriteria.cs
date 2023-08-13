namespace Itmo.Dev.Asap.Core.Domain.Queue;

public interface IFilterCriteria
{
    void Accept(IFilterCriteriaVisitor visitor);
}