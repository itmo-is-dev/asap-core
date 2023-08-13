namespace Itmo.Dev.Asap.Core.Domain.Queue.Building;

public interface IQueueEvaluatorBuilder
{
    IQueueEvaluatorBuilder AddEvaluator(IEvaluationCriteria evaluator);

    SubmissionQueue Build();
}

public interface IQueueFilterBuilder : IQueueEvaluatorBuilder
{
    IQueueFilterBuilder AddFilter(IFilterCriteria filter);
}