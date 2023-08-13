using Itmo.Dev.Asap.Core.Application.Dto.Study;

namespace Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;

public record SubjectCourseDto(
    Guid Id,
    Guid SubjectId,
    string Title,
    SubmissionStateWorkflowTypeDto? WorkflowType);