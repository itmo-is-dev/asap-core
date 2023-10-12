using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.Abstractions.Submissions;
using Itmo.Dev.Asap.Core.Application.Common.Exceptions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Models;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions.Workflow;
using Itmo.Dev.Asap.Core.Application.Factories;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Domain.ValueObject;
using Itmo.Dev.Asap.Core.Mapping;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Submissions.Workflows;

#pragma warning disable CA1506
public abstract class SubmissionWorkflowBase : ISubmissionWorkflow
{
    private readonly IPublisher _publisher;

    protected SubmissionWorkflowBase(
        IPermissionValidator permissionValidator,
        IPersistenceContext context,
        IPublisher publisher)
    {
        PermissionValidator = permissionValidator;
        Context = context;
        _publisher = publisher;
    }

    protected IPermissionValidator PermissionValidator { get; }

    protected IPersistenceContext Context { get; }

    public abstract Task<SubmissionApprovedResult> SubmissionApprovedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken);

    public async Task<SubmissionNotAcceptedResult> SubmissionNotAcceptedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        await PermissionValidator.EnsureSubmissionMentorAsync(issuerId, submissionId, cancellationToken);

        ExecuteSubmissionCommandResult result = await ExecuteSubmissionCommandAsync(
            submissionId,
            cancellationToken,
            static x => x.Rate(0, 0));

        if (result is ExecuteSubmissionCommandResult.InvalidMove invalidMove)
            return new SubmissionNotAcceptedResult.InvalidState(invalidMove.Submission.State.Kind.AsDto());

        if (result is not ExecuteSubmissionCommandResult.Success success)
            throw new UnexpectedOperationResultException();

        Submission submission = success.Submission;

        SubjectCourse subjectCourse = await Context.SubjectCourses
            .GetByAssignmentId(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        Assignment assignment = await Context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(assignment, subjectCourse.DeadlinePolicy);

        SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
            ratedSubmission,
            assignment);

        return new SubmissionNotAcceptedResult.Success(submissionRateDto);
    }

    public async Task<SubmissionReactivatedResult> SubmissionReactivatedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        ExecuteSubmissionCommandResult result = await ExecuteSubmissionCommandAsync(
            submissionId,
            cancellationToken,
            static x => x.Activate());

        return result switch
        {
            ExecuteSubmissionCommandResult.Success => new SubmissionReactivatedResult.Success(),

            ExecuteSubmissionCommandResult.InvalidMove invalidMove
                => new SubmissionReactivatedResult.InvalidState(invalidMove.Submission.State.Kind.AsDto()),

            _ => throw new UnexpectedOperationResultException(),
        };
    }

    public async Task<SubmissionAcceptedResult> SubmissionAcceptedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        await PermissionValidator.EnsureSubmissionMentorAsync(issuerId, submissionId, cancellationToken);

        ExecuteSubmissionCommandResult result = await ExecuteSubmissionCommandAsync(
            submissionId,
            cancellationToken,
            static x => x.Rating is null
                ? new SubmissionStateMoveResult.Success(x.State)
                : x.Rate(Fraction.FromDenormalizedValue(100), 0));

        if (result is ExecuteSubmissionCommandResult.InvalidMove invalidMove)
            return new SubmissionAcceptedResult.InvalidState(invalidMove.Submission.State.Kind.AsDto());

        if (result is not ExecuteSubmissionCommandResult.Success success)
            throw new UnexpectedOperationResultException();

        Submission submission = success.Submission;

        SubjectCourse subjectCourse = await Context.SubjectCourses
            .GetByAssignmentId(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        Assignment assignment = await Context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(assignment, subjectCourse.DeadlinePolicy);

        SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
            ratedSubmission,
            assignment);

        return new SubmissionAcceptedResult.Success(submissionRateDto);
    }

    public async Task<SubmissionRejectedResult> SubmissionRejectedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        await PermissionValidator.EnsureSubmissionMentorAsync(issuerId, submissionId, cancellationToken);

        ExecuteSubmissionCommandResult result = await ExecuteSubmissionCommandAsync(
            submissionId,
            cancellationToken,
            static x => x.Deactivate());

        return result switch
        {
            ExecuteSubmissionCommandResult.Success success
                => new SubmissionRejectedResult.Success(success.Submission.Code),

            ExecuteSubmissionCommandResult.InvalidMove invalidMove
                => new SubmissionRejectedResult.InvalidState(invalidMove.Submission.State.Kind.AsDto()),

            _ => throw new UnexpectedOperationResultException(),
        };
    }

    public async Task<SubmissionAbandonedResult> SubmissionAbandonedAsync(
        Guid issuerId,
        Guid submissionId,
        bool isTerminal,
        CancellationToken cancellationToken)
    {
        ExecuteSubmissionCommandResult result = await ExecuteSubmissionCommandAsync(
            submissionId,
            cancellationToken,
            x => isTerminal ? x.Complete() : x.Deactivate());

        return result switch
        {
            ExecuteSubmissionCommandResult.Success success
                => new SubmissionAbandonedResult.Success(success.Submission.Code),

            ExecuteSubmissionCommandResult.InvalidMove invalidMove
                => new SubmissionAbandonedResult.InvalidState(invalidMove.Submission.State.Kind.AsDto()),

            _ => throw new UnexpectedOperationResultException(),
        };
    }

    public async Task<SubmissionUpdatedResult> SubmissionUpdatedAsync(
        Guid issuerId,
        Guid userId,
        Guid assignmentId,
        string payload,
        CancellationToken cancellationToken)
    {
        SubmissionStateKind[] acceptedStates =
        {
            SubmissionStateKind.Active,
            SubmissionStateKind.Reviewed,
        };

        var submissionsQuery = SubmissionQuery.Build(x => x
            .WithUserId(userId)
            .WithAssignmentId(assignmentId)
            .WithSubmissionStates(acceptedStates)
            .WithOrderByCode(OrderDirection.Descending)
            .WithLimit(1));

        Submission? submission = await Context.Submissions
            .QueryAsync(submissionsQuery, cancellationToken)
            .FirstOrDefaultAsync(cancellationToken);

        SubjectCourse subjectCourse = await Context.SubjectCourses.GetByAssignmentId(assignmentId, cancellationToken);

        bool triggeredByMentor = subjectCourse.Mentors.Any(x => x.UserId.Equals(issuerId));
        bool triggeredByAnotherUser = issuerId.Equals(userId) is false;

        if (submission is null || submission.IsRated)
        {
            if (triggeredByAnotherUser && triggeredByMentor is false)
            {
                string message = $"User {issuerId} is not allowed to create new submission for user {userId}";
                throw new UnauthorizedException(message);
            }

            var submissionCodeQuery = SubmissionQuery.Build(x => x
                .WithUserId(userId)
                .WithAssignmentId(assignmentId));

            int code = await Context.Submissions.CountAsync(submissionCodeQuery, cancellationToken);

            Student student = await Context.Students.GetByIdAsync(userId, cancellationToken);

            if (student.Group is null)
                throw new EntityNotFoundException("Assignment not found");

            SubjectCourseAssignment? subjectCourseAssignment = subjectCourse.Assignments
                .Where(x => x.AssignmentId.Equals(assignmentId))
                .SingleOrDefault(x => x.Groups.Any(g => g.Id.Equals(student.Group.Id)));

            if (subjectCourseAssignment is null)
                throw EntityNotFoundException.For<Assignment>(assignmentId);

            var groupAssignmentId = new GroupAssignmentId(student.Group.Id, subjectCourseAssignment.AssignmentId);

            GroupAssignment groupAssignment = await Context.GroupAssignments
                .GetByIdAsync(groupAssignmentId, cancellationToken);

            submission = new Submission(
                Guid.NewGuid(),
                code + 1,
                student,
                Calendar.CurrentDateTime,
                payload,
                groupAssignment);

            Context.Submissions.Add(submission);
            await Context.SaveChangesAsync(cancellationToken);

            Assignment assignment = await Context.Assignments
                .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

            RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(
                assignment,
                subjectCourse.DeadlinePolicy);

            SubmissionDto dto = ratedSubmission.ToDto();

            var notification = new SubmissionStateUpdated.Notification(dto);
            await _publisher.PublishAsync(notification, default);

            SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
                ratedSubmission,
                assignment);

            return new SubmissionUpdatedResult.Success(submissionRateDto, true);
        }

        if (triggeredByMentor is false)
        {
            submission.UpdateDate(Calendar.CurrentDateTime);

            Context.Submissions.Update(submission);
            await Context.SaveChangesAsync(cancellationToken);

            Assignment assignment = await Context.Assignments
                .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

            RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(
                assignment,
                subjectCourse.DeadlinePolicy);

            SubmissionDto dto = ratedSubmission.ToDto();

            await NotifySubmissionUpdated(dto, cancellationToken);

            if (triggeredByAnotherUser)
                throw new UnauthorizedException("Submission updated by another user");

            SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
                ratedSubmission,
                assignment);

            return new SubmissionUpdatedResult.Success(submissionRateDto, false);
        }

        // TODO: Proper mentor update handling
        {
            Assignment assignment = await Context.Assignments
                .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

            RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(
                assignment,
                subjectCourse.DeadlinePolicy);

            SubmissionDto dto = ratedSubmission.ToDto();
            await NotifySubmissionUpdated(dto, cancellationToken);

            SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
                ratedSubmission,
                assignment);

            return new SubmissionUpdatedResult.Success(submissionRateDto, false);
        }
    }

    protected async Task<ExecuteSubmissionCommandResult> ExecuteSubmissionCommandAsync(
        Guid submissionId,
        CancellationToken cancellationToken,
        Func<Submission, SubmissionStateMoveResult> action)
    {
        Submission submission = await Context.Submissions.GetByIdAsync(submissionId, cancellationToken);
        SubmissionStateMoveResult result = action(submission);

        if (result is SubmissionStateMoveResult.InvalidMove)
            return new ExecuteSubmissionCommandResult.InvalidMove(submission);

        Context.Submissions.Update(submission);
        await Context.SaveChangesAsync(cancellationToken);

        Assignment assignment = await Context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Assignment.Id, cancellationToken);

        SubjectCourse subjectCourse = await Context.SubjectCourses
            .GetByAssignmentId(submission.GroupAssignment.Assignment.Id, cancellationToken);

        RatedSubmission rated = submission.CalculateRatedSubmission(assignment, subjectCourse.DeadlinePolicy);
        SubmissionDto dto = rated.ToDto();

        await NotifySubmissionUpdated(dto, cancellationToken);

        return new ExecuteSubmissionCommandResult.Success(submission);
    }

    protected async Task NotifySubmissionUpdated(
        SubmissionDto submission,
        CancellationToken cancellationToken)
    {
        var notification = new SubmissionUpdated.Notification(submission);
        await _publisher.Publish(notification, cancellationToken);
    }

    protected abstract record ExecuteSubmissionCommandResult
    {
        private ExecuteSubmissionCommandResult() { }

        public sealed record Success(Submission Submission) : ExecuteSubmissionCommandResult;

        public sealed record InvalidMove(Submission Submission) : ExecuteSubmissionCommandResult;
    }
}