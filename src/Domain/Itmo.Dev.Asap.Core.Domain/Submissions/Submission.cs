using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Deadlines.DeadlinePolicies;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.Study.GroupAssignments;
using Itmo.Dev.Asap.Core.Domain.Submissions.States;
using Itmo.Dev.Asap.Core.Domain.Tools;
using Itmo.Dev.Asap.Core.Domain.ValueObject;
using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.Domain.Submissions;

public partial class Submission : IEntity<Guid>
{
    public Submission(
        Guid id,
        int code,
        Student student,
        SpbDateTime submissionDate,
        string payload,
        GroupAssignment groupAssignment,
        Fraction? rating,
        Points? extraPoints,
        ISubmissionState state)
        : this(id)
    {
        Code = code;
        SubmissionDate = submissionDate;
        Student = student;
        Payload = payload;

        GroupAssignment = groupAssignment;

        Rating = rating;
        ExtraPoints = extraPoints;

        State = state;
    }

    public Submission(
        Guid id,
        int code,
        Student student,
        SpbDateTime submissionDate,
        string payload,
        GroupAssignment groupAssignment)
        : this(
            id,
            code,
            student,
            submissionDate,
            payload,
            groupAssignment,
            rating: default,
            extraPoints: default,
            new ActiveSubmissionState()) { }

    public int Code { get; }

    public string Payload { get; }

    public Fraction? Rating { get; private set; }

    public Points? ExtraPoints { get; private set; }

    public SpbDateTime SubmissionDate { get; private set; }

    public Student Student { get; }

    public GroupAssignment GroupAssignment { get; }

    public bool IsRated => Rating is not null;

    public DateOnly SubmissionDateOnly => SubmissionDate.AsDateOnly();

    public ISubmissionState State { get; private set; }

    public override string ToString()
    {
        return $"{Code} ({Id})";
    }

    public SubmissionStateMoveResult Rate(Fraction? rating, Points? extraPoints)
    {
        if (rating is null && extraPoints is null)
        {
            const string ratingName = nameof(rating);
            const string extraPointsName = nameof(extraPoints);
            const string message =
                $"Cannot update submission points, both {ratingName} and {extraPointsName} are null.";

            throw new DomainInvalidOperationException(message);
        }

        SubmissionStateMoveResult result = State.MoveToRated(rating, extraPoints);

        if (result is not SubmissionStateMoveResult.Success success)
            return result;

        State = success.State;

        if (rating is not null)
            Rating = rating;

        if (extraPoints is not null)
            ExtraPoints = extraPoints;

        return success;
    }

    public SubmissionStateMoveResult UpdatePoints(Fraction? rating, Points? extraPoints)
    {
        if (rating is null && extraPoints is null)
        {
            const string ratingName = nameof(rating);
            const string extraPointsName = nameof(extraPoints);
            const string message =
                $"Cannot update submission points, both {ratingName} and {extraPointsName} are null.";
            throw new DomainInvalidOperationException(message);
        }

        SubmissionStateMoveResult result = State.MoveToPointsUpdated(rating, extraPoints);

        if (result is not SubmissionStateMoveResult.Success success)
            return result;

        State = success.State;

        if (rating is not null)
            Rating = rating;

        if (extraPoints is not null)
            ExtraPoints = extraPoints;

        return success;
    }

    public RatedSubmission CalculateRatedSubmission(Assignment assignment, DeadlinePolicy policy)
    {
        Points rawPoints = assignment.RatedWith(Rating);
        Points pointsWithPenalty = policy.ApplyPointPenalty(rawPoints, GroupAssignment.Deadline, SubmissionDateOnly);
        Points pointPenalty = rawPoints - pointsWithPenalty;

        Points totalPoints = pointsWithPenalty + (ExtraPoints ?? Points.None);

        return new RatedSubmission(this, totalPoints, pointsWithPenalty, pointPenalty, rawPoints);
    }

    public SubmissionStateMoveResult UpdateDate(SpbDateTime newDate)
    {
        SubmissionStateMoveResult result = State.MoveToDateUpdated(newDate);

        if (result is not SubmissionStateMoveResult.Success success)
            return result;

        State = success.State;
        SubmissionDate = newDate;

        return success;
    }

    public SubmissionStateMoveResult Activate()
        => HandleMoveResult(State.MoveToActivated());

    public SubmissionStateMoveResult Deactivate()
        => HandleMoveResult(State.MoveToDeactivated());

    public SubmissionStateMoveResult Ban()
        => HandleMoveResult(State.MoveToBanned());

    public SubmissionStateMoveResult Delete()
        => HandleMoveResult(State.MoveToDeleted());

    public SubmissionStateMoveResult Complete()
        => HandleMoveResult(State.MoveToCompleted());

    public SubmissionStateMoveResult MarkAsReviewed()
        => HandleMoveResult(State.MoveToReviewed());

    private SubmissionStateMoveResult HandleMoveResult(SubmissionStateMoveResult result)
    {
        if (result is not SubmissionStateMoveResult.Success success)
            return result;

        State = success.State;

        return success;
    }
}