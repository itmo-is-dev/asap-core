using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Queue.EvaluationCriteria;
using Itmo.Dev.Asap.Core.Domain.Queue.FilterCriteria;
using Itmo.Dev.Asap.Core.Domain.Queue.Models;

namespace Itmo.Dev.Asap.Core.Domain.Queue.Building;

public class DefaultQueueBuilder : QueueBuilder
{
    public DefaultQueueBuilder(
        Guid studentGroupId,
        Guid subjectCourseId)
    {
        AddFilter(new StudentGroupFilterCriteria(new[] { studentGroupId }));
        AddFilter(new SubmissionStateFilterCriteria(SubmissionStateKind.Active, SubmissionStateKind.Reviewed));
        AddFilter(new SubjectCourseFilterCriteria(subjectCourseId));

        AddEvaluator(new SubmissionStateEvaluationCriteria(SortingOrder.Descending));
        AddEvaluator(new AssignmentDeadlineEvaluationCriteria(SortingOrder.Descending));
        AddEvaluator(new SubmissionDateTimeEvaluationCriteria(SortingOrder.Ascending));
    }
}