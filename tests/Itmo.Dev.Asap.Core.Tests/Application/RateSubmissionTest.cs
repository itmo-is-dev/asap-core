using FluentAssertions;
using Itmo.Dev.Asap.Core.Domain.Models;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Asap.Core.Domain.ValueObject;
using Itmo.Dev.Asap.Core.Tests.Extensions;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Application;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class RateSubmissionTest : CoreDatabaseTestBase
{
    public RateSubmissionTest(CoreDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task UpdateSubmission_Should_NoThrow()
    {
        Submission first = await PersistenceContext.GetSubmissionAsync(SubmissionStateKind.Active);

        first.Rate(new Fraction(0.5), Points.None);
        first.State.Kind.Should().Be(SubmissionStateKind.Completed);

        first.UpdatePoints(new Fraction(0.5), Points.None);
        first.State.Kind.Should().Be(SubmissionStateKind.Completed);
    }
}