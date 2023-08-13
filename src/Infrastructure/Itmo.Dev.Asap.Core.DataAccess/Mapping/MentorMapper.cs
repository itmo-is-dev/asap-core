using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Domain.Users;

namespace Itmo.Dev.Asap.Core.DataAccess.Mapping;

public static class MentorMapper
{
    public static Mentor MapTo(MentorModel model)
    {
        return new Mentor(model.UserId, model.SubjectCourseId);
    }

    public static MentorModel MapFrom(Mentor entity)
    {
        return new MentorModel(entity.UserId, entity.SubjectCourseId);
    }
}