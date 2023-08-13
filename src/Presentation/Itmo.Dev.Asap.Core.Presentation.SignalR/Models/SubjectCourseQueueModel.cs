using Itmo.Dev.Asap.Core.Application.Dto.Tables;

namespace Itmo.Dev.Asap.Core.WebApi.Abstractions.Models.Queue;

public record SubjectCourseQueueModel(Guid SubjectCourseId, Guid StudyGroupId, SubmissionsQueueDto Queue);