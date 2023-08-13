using Itmo.Dev.Asap.Core.Domain.Queue.FilterCriteria;

namespace Itmo.Dev.Asap.Core.Domain.Queue;

public interface IFilterCriteriaVisitor
{
    void Visit(AssignmentFilterCriteria criteria);

    void Visit(StudentGroupFilterCriteria criteria);

    void Visit(SubjectCourseFilterCriteria criteria);

    void Visit(SubmissionStateFilterCriteria criteria);
}