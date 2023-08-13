using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Groups;

namespace Itmo.Dev.Asap.Core.DataAccess.Mapping;

public static class StudentGroupMapper
{
    public static StudentGroup MapTo(StudentGroupModel model, HashSet<Guid> studentIds)
    {
        return new StudentGroup(model.Id, model.Name, studentIds);
    }

    public static StudentGroupModel MapFrom(StudentGroup entity)
    {
        return new StudentGroupModel(entity.Id, entity.Name);
    }
}