using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Groups;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class StudyGroupMapping
{
    public static StudyGroupDto ToDto(this StudentGroup group)
    {
        return new StudyGroupDto(group.Id, group.Name);
    }
}