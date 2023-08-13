using Itmo.Dev.Asap.Core.Application.Abstractions.Permissions;
using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;

namespace Itmo.Dev.Asap.Core.Application.Validators;

public class PermissionValidator : IPermissionValidator
{
    private readonly IPersistenceContext _context;

    public PermissionValidator(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<bool> IsSubmissionMentorAsync(Guid userId, Guid submissionId, CancellationToken cancellationToken)
    {
        SubjectCourse subjectCourse = await _context.SubjectCourses
            .GetBySubmissionIdAsync(submissionId, cancellationToken);

        return subjectCourse.Mentors.Any(x => x.UserId.Equals(userId));
    }

    public async Task EnsureSubmissionMentorAsync(
        Guid userId,
        Guid submissionId,
        CancellationToken cancellationToken)
    {
        if (await IsSubmissionMentorAsync(userId, submissionId, cancellationToken) is false)
            throw new UnauthorizedException();
    }
}