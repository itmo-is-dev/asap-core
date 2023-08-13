using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;

namespace Itmo.Dev.Asap.Core.Mapping;

public static class AssignmentMapping
{
    public static AssignmentDto ToDto(this Assignment assignment)
    {
        return new AssignmentDto(
            assignment.SubjectCourseId,
            assignment.Id,
            assignment.Title,
            assignment.ShortName,
            assignment.Order,
            assignment.MinPoints.AsDto(),
            assignment.MaxPoints.AsDto());
    }
}