using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

namespace Itmo.Dev.Asap.Core.Application.DataAccess;

public interface IPersistenceContext
{
    IUserRepository Users { get; }

    IStudentRepository Students { get; }

    IMentorRepository Mentors { get; }

    IAssignmentRepository Assignments { get; }

    IGroupAssignmentRepository GroupAssignments { get; }

    IStudentGroupRepository StudentGroups { get; }

    ISubjectRepository Subjects { get; }

    ISubjectCourseRepository SubjectCourses { get; }

    ISubmissionRepository Submissions { get; }

    IUserAssociationRepository UserAssociations { get; }

    IStudentAssignmentRepository StudentAssignments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}