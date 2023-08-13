using Itmo.Dev.Asap.Core.Application.Dto.Users;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.UserAssociations;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class StudentMapping
{
    public static StudentDto ToDto(this Student student)
    {
        IsuUserAssociation? isuAssociation = student.User.FindAssociation<IsuUserAssociation>();

        return new StudentDto(
            student.User.ToDto(),
            student.Group?.Id,
            student.Group?.Name ?? string.Empty,
            isuAssociation?.UniversityId);
    }
}