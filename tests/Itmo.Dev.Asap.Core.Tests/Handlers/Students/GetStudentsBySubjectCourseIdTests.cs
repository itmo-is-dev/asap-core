using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Students.Queries;
using Itmo.Dev.Asap.Core.Application.Handlers.Students;
using Itmo.Dev.Asap.Core.Application.Specifications;
using Itmo.Dev.Asap.Core.Domain.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Itmo.Dev.Platform.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.Students;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetStudentsBySubjectCourseIdTests : CoreDatabaseTestBase, IAsyncDisposeLifetime
{
    public GetStudentsBySubjectCourseIdTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task Handle_Should_NoThrow()
    {
        Guid subjectCourseId = await Context.SubjectCourses
            .Where(sc => sc.SubjectCourseGroups.Any(g => g.StudentGroup.Students.Any()))
            .Select(x => x.Id)
            .FirstAsync();

        SubjectCourse subjectCourse = await PersistenceContext.SubjectCourses
            .GetByIdAsync(subjectCourseId, default);

        var query = new GetStudentsBySubjectCourseId.Query(subjectCourse.Id);
        var handler = new GetStudentsBySubjectCourseIdHandler(PersistenceContext);

        GetStudentsBySubjectCourseId.Response handle = await handler.Handle(query, CancellationToken.None);

        handle.Students.Should().NotBeEmpty();
    }
}