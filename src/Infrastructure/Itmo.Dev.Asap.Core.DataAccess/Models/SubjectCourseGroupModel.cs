using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.DataAccess.Models;

public partial class SubjectCourseGroupModel : IEntity
{
    public SubjectCourseGroupModel(Guid subjectCourseId, Guid studentGroupId)
    {
        StudentGroupId = studentGroupId;
        SubjectCourseId = subjectCourseId;
        SubjectCourse = null!;
        StudentGroup = null!;
    }

    [KeyProperty]
    public virtual SubjectCourseModel SubjectCourse { get; set; }

    [KeyProperty]
    public virtual StudentGroupModel StudentGroup { get; set; }
}