using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.DataAccess.Models.Users;

public partial class MentorModel : IEntity
{
    public MentorModel(Guid userId, Guid subjectCourseId)
    {
        SubjectCourseId = subjectCourseId;
        UserId = userId;
        User = null!;
        SubjectCourse = null!;
    }

    [KeyProperty]
    public virtual UserModel User { get; set; }

    [KeyProperty]
    public virtual SubjectCourseModel SubjectCourse { get; set; }
}