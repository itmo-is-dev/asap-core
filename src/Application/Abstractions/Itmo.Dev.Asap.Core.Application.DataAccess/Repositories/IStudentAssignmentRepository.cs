using Itmo.Dev.Asap.Core.Domain.Study;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface IStudentAssignmentRepository
{
    Task<StudentAssignment> GetByIdAsync(Guid studentId, Guid assignmentId, CancellationToken cancellationToken);

    IAsyncEnumerable<StudentAssignment> GetBySubjectCourseIdAsync(
        Guid subjectCourseId,
        CancellationToken cancellationToken);
}