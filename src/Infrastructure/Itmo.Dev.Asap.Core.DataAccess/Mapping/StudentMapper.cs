using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.Users;

namespace Itmo.Dev.Asap.Core.DataAccess.Mapping;

public static class StudentMapper
{
    public static Student MapTo(StudentModel model)
    {
        User user = UserMapper.MapTo(model.User);

        StudentGroupInfo? group = model.StudentGroup is null
            ? null
            : new StudentGroupInfo(model.StudentGroup.Id, model.StudentGroup.Name);

        return new Student(user, group);
    }

    public static StudentModel MapFrom(Student entity)
    {
        return new StudentModel(entity.UserId, entity.Group?.Id);
    }
}