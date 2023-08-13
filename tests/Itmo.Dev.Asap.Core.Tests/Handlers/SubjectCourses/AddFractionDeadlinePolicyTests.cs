using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Commands;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.DataAccess.Models.DeadlinePenalties;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.SubjectCourses;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class AddFractionDeadlinePolicyTests : CoreDatabaseTestBase
{
    public AddFractionDeadlinePolicyTests(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task HandleAsync_ShouldAddDeadlinePolicyCorrectly()
    {
        // Arrange
        SubjectCourseModel subjectCourse = await Context.SubjectCourses
            .OrderBy(x => x.Id)
            .Where(x => x.DeadlinePenalties.Count != 0)
            .FirstAsync();

        TimeSpan span = subjectCourse.DeadlinePenalties
            .Select(x => x.SpanBeforeActivation)
            .OrderDescending()
            .First()
            .Add(TimeSpan.FromDays(2));

        const double fraction = 0.5;

        var command = new AddFractionDeadlinePolicy.Command(subjectCourse.Id, span, fraction);
        var handler = new AddFractionDeadlinePolicyHandler(PersistenceContext);

        // Act
        await handler.Handle(command, default);

        // Assert
        DeadlinePenaltyModel penalty = await Context.DeadlinePenalties
            .Where(x => x.SubjectCourseId.Equals(subjectCourse.Id))
            .OrderByDescending(x => x.SpanBeforeActivation)
            .FirstAsync();

        penalty.Should()
            .BeAssignableTo<FractionDeadlinePenaltyModel>()
            .Which.Fraction.Should()
            .Be(fraction);
    }
}