using Itmo.Dev.Asap.Core.Domain.Study.Assignments.Results;
using Itmo.Dev.Asap.Core.Domain.ValueObject;
using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.Domain.Study.Assignments;

public partial class Assignment : IEntity<Guid>
{
    public Assignment(
        Guid id,
        string title,
        string shortName,
        int order,
        Points minPoints,
        Points maxPoints,
        Guid subjectCourseId)
        : this(id)
    {
        if (minPoints > maxPoints)
            throw new ArgumentException("minPoints must be less than or equal to maxPoints");

        SubjectCourseId = subjectCourseId;
        Title = title;
        ShortName = shortName;
        Order = order;
        MinPoints = minPoints;
        MaxPoints = maxPoints;
    }

    public Guid SubjectCourseId { get; }

    public string Title { get; set; }

    public string ShortName { get; set; }

    public int Order { get; set; }

    public Points MinPoints { get; protected set; }

    public Points MaxPoints { get; protected set; }

    public AssignmentInfo Info => new AssignmentInfo(Id, Title, ShortName);

    public UpdatePointsResult UpdatePoints(Points minPoints, Points maxPoints)
    {
        if (minPoints > maxPoints)
            return new UpdatePointsResult.MaxPointsLessThanMinPoints();

        MinPoints = minPoints;
        MaxPoints = maxPoints;

        return new UpdatePointsResult.Success();
    }

    public Points RatedWith(Fraction? value)
    {
        return MaxPoints * value ?? Points.None;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Title: {ShortName}";
    }
}