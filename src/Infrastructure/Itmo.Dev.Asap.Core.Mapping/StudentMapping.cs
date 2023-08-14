using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Domain.Students;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class StudentMapping
{
    public static StudentDto ToDto(this Student student)
    {
        return new StudentDto(
            student.User.ToDto(),
            student.Group?.Id,
            student.Group?.Name ?? string.Empty);
    }
}