using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.Domain.Study;

public partial class Subject : IEntity<Guid>
{
    public Subject(Guid id, string title) : this(id)
    {
        ArgumentNullException.ThrowIfNull(title);

        Title = title;
    }

    public string Title { get; set; }

    public SubjectCourse AddCourse(SubjectCourseBuilder builder)
    {
        return builder.Build(this);
    }
}