using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.DataAccess.Models;

public partial class SubjectModel : IEntity<Guid>
{
    public SubjectModel(Guid id, string title) : this(id)
    {
        Title = title;
    }

    public string Title { get; set; }

    public virtual ICollection<SubjectCourseModel> SubjectCourses { get; init; }
}