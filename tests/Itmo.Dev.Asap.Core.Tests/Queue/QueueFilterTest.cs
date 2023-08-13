using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Queue;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Queue;
using Itmo.Dev.Asap.Core.Domain.Queue.Building;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Queue;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class QueueFilterTest : CoreDatabaseTestBase
{
    public QueueFilterTest(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task DefaultQueue_Should_NotThrow()
    {
        SubjectCourseModel subjectCourse = await Context.SubjectCourses.FirstAsync();

        StudentGroupModel group = subjectCourse.SubjectCourseGroups
            .Select(x => x.StudentGroup)
            .First(group => subjectCourse.Assignments
                .SelectMany(x => x.GroupAssignments)
                .SelectMany(x => x.Submissions)
                .Any(x => x.Student.StudentGroup?.Equals(group) ?? false));

        SubmissionQueue queue = new DefaultQueueBuilder(group.Id, subjectCourse.Id).Build();

        var visitor = new FilterCriteriaVisitor(new SubmissionQuery.Builder());
        queue.AcceptFilterCriteriaVisitor(visitor);

        Submission[] submissions = await PersistenceContext.Submissions
            .QueryAsync(visitor.Builder.Build(), default)
            .ToArrayAsync();

        submissions.Should().NotBeEmpty();
    }
}