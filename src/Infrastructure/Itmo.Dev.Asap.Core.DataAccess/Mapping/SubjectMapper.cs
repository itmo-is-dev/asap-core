using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Study;

namespace Itmo.Dev.Asap.Core.DataAccess.Mapping;

public static class SubjectMapper
{
    public static Subject MapTo(SubjectModel model)
    {
        return new Subject(model.Id, model.Title);
    }

    public static SubjectModel MapFrom(Subject entity)
    {
        return new SubjectModel(entity.Id, entity.Title);
    }
}