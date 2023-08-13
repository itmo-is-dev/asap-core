using Itmo.Dev.Asap.Core.Domain.Groups;
using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.Domain.Study;

public partial class SubjectCourseGroup : IEntity
{
    public SubjectCourseGroup(Guid subjectCourseId, StudentGroupInfo studentGroup)
        : this(subjectCourseId: subjectCourseId, studentGroupId: studentGroup.Id)
    {
        StudentGroup = studentGroup;
    }

    [KeyProperty]
    public Guid SubjectCourseId { get; }

    [KeyProperty]
    public StudentGroupInfo StudentGroup { get; }
}