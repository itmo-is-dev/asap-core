using Itmo.Dev.Asap.Core.Domain.Study;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface IStudentAssignmentRepository
{
    IAsyncEnumerable<StudentAssignment> GetBySubjectCourseIdAsync(
        Guid subjectCourseId,
        CancellationToken cancellationToken);
}