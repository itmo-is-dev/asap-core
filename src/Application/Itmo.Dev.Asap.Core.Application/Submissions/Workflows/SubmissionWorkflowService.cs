using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.Abstractions.Submissions;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.SubmissionStateWorkflows;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Submissions.Workflows;

public class SubmissionWorkflowService : ISubmissionWorkflowService
{
    private readonly IPersistenceContext _context;
    private readonly IPermissionValidator _permissionValidator;
    private readonly IPublisher _publisher;

    public SubmissionWorkflowService(
        IPersistenceContext context,
        IPermissionValidator permissionValidator,
        IPublisher publisher)
    {
        _context = context;
        _permissionValidator = permissionValidator;
        _publisher = publisher;
    }

    public async Task<ISubmissionWorkflow> GetSubmissionWorkflowAsync(
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetBySubmissionIdAsync(submissionId, cancellationToken);

        return GetSubmissionWorkflow(subjectCourse);
    }

    public async Task<ISubmissionWorkflow> GetAssignmentWorkflowAsync(
        Guid assignmentId,
        CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses.GetByAssignmentId(assignmentId, cancellationToken);
        return GetSubmissionWorkflow(subjectCourse);
    }

    private ISubmissionWorkflow GetSubmissionWorkflow(SubjectCourse subjectCourse)
    {
        return subjectCourse.WorkflowType switch
        {
            null or SubmissionStateWorkflowType.ReviewOnly
                => new ReviewOnlySubmissionWorkflow(_context, _permissionValidator, _publisher),

            SubmissionStateWorkflowType.ReviewWithDefense
                => new ReviewWithDefenceSubmissionWorkflow(_permissionValidator, _context, _publisher),

            _ => throw new ArgumentOutOfRangeException(
                nameof(subjectCourse),
                $@"Invalid WorkflowType {subjectCourse.WorkflowType:G}"),
        };
    }
}