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
        ISubmissionState state)
        : this(id)
    {
        Code = code;
        SubmissionDate = submissionDate;
        Student = student;
        Payload = payload;

        GroupAssignment = groupAssignment;

        Rating = default;
        ExtraPoints = default;

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

    public void Rate(Fraction? rating, Points? extraPoints)
    {
        if (rating is null && extraPoints is null)
        {
            const string ratingName = nameof(rating);
            const string extraPointsName = nameof(extraPoints);
            const string message =
                $"Cannot update submission points, both {ratingName} and {extraPointsName} are null.";
            throw new DomainInvalidOperationException(message);
        }

        State = State.MoveToRated(rating, extraPoints);

        if (rating is not null)
            Rating = rating;

        if (extraPoints is not null)
            ExtraPoints = extraPoints;
    }

    public void UpdatePoints(Fraction? rating, Points? extraPoints)
    {
        if (rating is null && extraPoints is null)
        {
            const string ratingName = nameof(rating);
            const string extraPointsName = nameof(extraPoints);
            const string message =
                $"Cannot update submission points, both {ratingName} and {extraPointsName} are null.";
            throw new DomainInvalidOperationException(message);
        }

        State = State.MoveToPointsUpdated(rating, extraPoints);

        if (rating is not null)
            Rating = rating;

        if (extraPoints is not null)
            ExtraPoints = extraPoints;
    }

    public RatedSubmission CalculateRatedSubmission(Assignment assignment, DeadlinePolicy policy)
    {
        Points rawPoints = assignment.RatedWith(Rating);
        Points pointsWithPenalty = policy.ApplyPointPenalty(rawPoints, GroupAssignment.Deadline, SubmissionDateOnly);
        Points pointPenalty = rawPoints - pointsWithPenalty;

        Points totalPoints = pointsWithPenalty + (ExtraPoints ?? Points.None);

        return new RatedSubmission(this, totalPoints, pointsWithPenalty, pointPenalty, rawPoints);
    }

    public void UpdateDate(SpbDateTime newDate)
    {
        State = State.MoveToDateUpdated(newDate);
        SubmissionDate = newDate;
    }

    public void Activate()
    {
        State = State.MoveToActivated();
    }

    public void Deactivate()
    {
        State = State.MoveToDeactivated();
    }

    public void Ban()
    {
        State = State.MoveToBanned();
    }

    public void Delete()
    {
        State = State.MoveToDeleted();
    }

    public void Complete()
    {
        State = State.MoveToCompleted();
    }

    public void MarkAsReviewed()
    {
        State = State.MoveToReviewed();
    }
}