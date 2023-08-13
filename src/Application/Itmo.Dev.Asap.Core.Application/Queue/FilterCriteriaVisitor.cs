using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Queue;
using Itmo.Dev.Asap.Core.Domain.Queue.FilterCriteria;

namespace Itmo.Dev.Asap.Core.Application.Queue;

public class FilterCriteriaVisitor : IFilterCriteriaVisitor
{
    public FilterCriteriaVisitor(SubmissionQuery.Builder builder)
    {
        Builder = builder;
    }

    public SubmissionQuery.Builder Builder { get; }

    public void Visit(AssignmentFilterCriteria criteria)
    {
        Builder.WithAssignmentIds(criteria.AssignmentIds);
    }

    public void Visit(StudentGroupFilterCriteria criteria)
    {
        Builder.WithStudentGroupIds(criteria.StudentGroupIds);
    }

    public void Visit(SubjectCourseFilterCriteria criteria)
    {
        Builder.WithSubjectCourseId(criteria.SubjectCourseId);
    }

    public void Visit(SubmissionStateFilterCriteria criteria)
    {
        Builder.WithSubmissionStates(criteria.States);
    }
}