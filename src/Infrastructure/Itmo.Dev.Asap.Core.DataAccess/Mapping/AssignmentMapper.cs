using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Study.Assignments;
using Itmo.Dev.Asap.Core.Domain.ValueObject;

namespace Itmo.Dev.Asap.Core.DataAccess.Mapping;

public static class AssignmentMapper
{
    public static Assignment MapTo(AssignmentModel model)
    {
        return new Assignment(
            model.Id,
            model.Title,
            model.ShortName,
            model.Order,
            new Points(model.MinPoints),
            new Points(model.MaxPoints),
            model.SubjectCourseId);
    }

    public static AssignmentModel MapFrom(Assignment entity, Guid subjectCourseId)
    {
        return new AssignmentModel(
            entity.Id,
            subjectCourseId,
            entity.Title,
            entity.ShortName,
            entity.Order,
            entity.MinPoints.Value,
            entity.MaxPoints.Value);
    }
}