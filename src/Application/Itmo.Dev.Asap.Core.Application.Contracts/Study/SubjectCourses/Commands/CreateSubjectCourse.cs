using Itmo.Dev.Asap.Core.Application.Dto.Study;
using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands;

internal static class CreateSubjectCourse
{
    public record Command(
        string CorrelationId,
        Guid SubjectId,
        string Title,
        SubmissionStateWorkflowTypeDto WorkflowType) : IRequest<Response>;

    public record Response(SubjectCourseDto SubjectCourse);
}