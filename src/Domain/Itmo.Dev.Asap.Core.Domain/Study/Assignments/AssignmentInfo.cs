using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Core.Domain.Study.Assignments;

public partial class AssignmentInfo : IEntity<Guid>
{
    public AssignmentInfo(Guid id, string title, string shortName) : this(id)
    {
        Title = title;
        ShortName = shortName;
    }

    public string Title { get; }

    public string ShortName { get; }
}