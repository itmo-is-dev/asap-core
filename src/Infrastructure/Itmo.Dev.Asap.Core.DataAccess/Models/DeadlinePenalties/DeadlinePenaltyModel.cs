using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.DataAccess.Models.DeadlinePenalties;

public partial class DeadlinePenaltyModel : IEntity<Guid>
{
    public DeadlinePenaltyModel(
        Guid id,
        Guid subjectCourseId,
        TimeSpan spanBeforeActivation)
        : this(id)
    {
        SubjectCourseId = subjectCourseId;
        SpanBeforeActivation = spanBeforeActivation;
        Id = id;
    }

    public Guid SubjectCourseId { get; set; }

    public virtual SubjectCourseModel SubjectCourse { get; set; }

    public TimeSpan SpanBeforeActivation { get; set; }
}