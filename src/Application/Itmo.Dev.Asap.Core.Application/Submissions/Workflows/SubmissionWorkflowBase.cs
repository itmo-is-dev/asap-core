using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.Abstractions.Submissions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Notifications;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Models;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Application.Factories;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Common.Resources;
using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePolicies;
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

    public abstract Task<SubmissionActionMessageDto> SubmissionApprovedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken);

    public async Task<SubmissionActionMessageDto> SubmissionNotAcceptedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        await PermissionValidator.EnsureSubmissionMentorAsync(issuerId, submissionId, cancellationToken);

        Submission submission = await ExecuteSubmissionCommandAsync(
            submissionId,
            cancellationToken,
            static x => x.Rate(0, 0));

        SubjectCourse subjectCourse = await Context.SubjectCourses
            .GetByAssignmentId(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        Assignment assignment = await Context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(assignment, subjectCourse.DeadlinePolicy);

        SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
            ratedSubmission,
            assignment);

        string message = UserCommandProcessingMessage.SubmissionMarkAsNotAccepted(submission.Code);

        message = $"{message}\n{submissionRateDto.ToDisplayString()}";

        return new SubmissionActionMessageDto(message);
    }

    public async Task<SubmissionActionMessageDto> SubmissionReactivatedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        await ExecuteSubmissionCommandAsync(submissionId, cancellationToken, static x => x.Activate());

        string message = UserCommandProcessingMessage.SubmissionActivatedSuccessfully();
        return new SubmissionActionMessageDto(message);
    }

    public async Task<SubmissionActionMessageDto> SubmissionAcceptedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        await PermissionValidator.EnsureSubmissionMentorAsync(issuerId, submissionId, cancellationToken);

        Submission submission = await ExecuteSubmissionCommandAsync(
            submissionId,
            cancellationToken,
            static x =>
            {
                if (x.Rating is null)
                    x.Rate(Fraction.FromDenormalizedValue(100), 0);
            });

        SubjectCourse subjectCourse = await Context.SubjectCourses
            .GetByAssignmentId(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        Assignment assignment = await Context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

        RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(assignment, subjectCourse.DeadlinePolicy);

        SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
            ratedSubmission,
            assignment);

        string message = UserCommandProcessingMessage.SubmissionRated(submissionRateDto.ToDisplayString());

        return new SubmissionActionMessageDto(message);
    }

    public async Task<SubmissionActionMessageDto> SubmissionRejectedAsync(
        Guid issuerId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        await PermissionValidator.EnsureSubmissionMentorAsync(issuerId, submissionId, cancellationToken);

        Submission submission = await ExecuteSubmissionCommandAsync(
            submissionId,
            cancellationToken,
            static x => x.Deactivate());

        string message = UserCommandProcessingMessage.ClosePullRequestWithUnratedSubmission(submission.Code);

        return new SubmissionActionMessageDto(message);
    }

    public async Task<SubmissionActionMessageDto> SubmissionAbandonedAsync(
        Guid issuerId,
        Guid submissionId,
        bool isTerminal,
        CancellationToken cancellationToken)
    {
        Submission submission = await ExecuteSubmissionCommandAsync(
            submissionId,
            cancellationToken,
            x =>
            {
                if (isTerminal)
                    x.Complete();
                else
                    x.Deactivate();
            });

        string message = UserCommandProcessingMessage.MergePullRequestWithoutRate(submission.Code);
        return new SubmissionActionMessageDto(message);
    }

    public async Task<SubmissionUpdateResult> SubmissionUpdatedAsync(
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

            SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
                ratedSubmission,
                assignment);

            return new SubmissionUpdateResult(submissionRateDto, true);
        }

        if (triggeredByMentor is false)
        {
            submission.UpdateDate(Calendar.CurrentDateTime);

            Context.Submissions.Update(submission);
            await Context.SaveChangesAsync(cancellationToken);

            Assignment assignment = await Context.Assignments
                .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

            await NotifySubmissionUpdated(submission, assignment, subjectCourse.DeadlinePolicy, cancellationToken);

            if (triggeredByAnotherUser)
                throw new UnauthorizedException("Submission updated by another user");

            RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(
                assignment,
                subjectCourse.DeadlinePolicy);

            SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
                ratedSubmission,
                assignment);

            return new SubmissionUpdateResult(submissionRateDto, false);
        }

        // TODO: Proper mentor update handling
        {
            Assignment assignment = await Context.Assignments
                .GetByIdAsync(submission.GroupAssignment.Id.AssignmentId, cancellationToken);

            RatedSubmission ratedSubmission = submission.CalculateRatedSubmission(
                assignment,
                subjectCourse.DeadlinePolicy);

            SubmissionRateDto submissionRateDto = SubmissionRateDtoFactory.CreateFromRatedSubmission(
                ratedSubmission,
                assignment);

            return new SubmissionUpdateResult(submissionRateDto, false);
        }
    }

    protected async Task<Submission> ExecuteSubmissionCommandAsync(
        Guid submissionId,
        CancellationToken cancellationToken,
        Action<Submission> action)
    {
        Submission submission = await Context.Submissions.GetByIdAsync(submissionId, cancellationToken);
        action(submission);

        Context.Submissions.Update(submission);
        await Context.SaveChangesAsync(cancellationToken);

        Assignment assignment = await Context.Assignments
            .GetByIdAsync(submission.GroupAssignment.Assignment.Id, cancellationToken);

        SubjectCourse subjectCourse = await Context.SubjectCourses
            .GetByAssignmentId(submission.GroupAssignment.Assignment.Id, cancellationToken);

        await NotifySubmissionUpdated(submission, assignment, subjectCourse.DeadlinePolicy, cancellationToken);

        return submission;
    }

    protected async Task NotifySubmissionUpdated(
        Submission submission,
        Assignment assignment,
        DeadlinePolicy deadlinePolicy,
        CancellationToken cancellationToken)
    {
        Points points = submission.CalculateRatedSubmission(assignment, deadlinePolicy).TotalPoints;
        var notification = new SubmissionUpdated.Notification(submission.ToDto(points));

        await _publisher.Publish(notification, cancellationToken);
    }
}