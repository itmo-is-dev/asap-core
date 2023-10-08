using FluentAssertions;
using Itmo.Dev.Asap.Core.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.GroupAssignments.Commands;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Handlers.Study.GroupAssignments;
using Itmo.Dev.Asap.Core.Application.Users;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.Domain.Users;
using Itmo.Dev.Asap.Core.Seeding.Options;
using Itmo.Dev.Asap.Core.Tests.Fixtures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Handlers.GroupAssignments;

public class UpdateGroupAssignmentDeadlineTest :
    CoreDatabaseTestBase,
    IClassFixture<UpdateGroupAssignmentDeadlineTest.UpdateGroupAssignmentDatabaseFixture>
{
    private readonly DateOnly _newDeadline = DateOnly.MaxValue;
    private readonly IPublisher _publisher = new Mock<IPublisher>().Object;

    public UpdateGroupAssignmentDeadlineTest(UpdateGroupAssignmentDatabaseFixture database) : base(database) { }

    [Fact]
    public async Task Handle_ByMentorOfThisCourse_ShouldUpdateDeadline()
    {
        // Arrange
        GroupAssignmentModel groupAssignment = await GetGroupAssignment();
        var query = MentorQuery.Build(x => x.WithSubjectCourseId(groupAssignment.Assignment.SubjectCourseId));

        Mentor mentor = await PersistenceContext.Mentors
            .QueryAsync(query, default)
            .FirstAsync();

        var currentUser = new MentorUser(mentor.UserId);

        // Act
        UpdateGroupAssignmentDeadlines.Response response = await HandleByCurrentUser(currentUser);

        // Assert
        response.Should()
            .BeOfType<UpdateGroupAssignmentDeadlines.Response.Success>()
            .Which.GroupAssignments.Single()
            .Deadline.Should()
            .Be(_newDeadline);
    }

    [Fact]
    public async Task Handle_ByMentorOfNotThisCourse_ShouldThrow()
    {
        // Arrange
        GroupAssignmentModel groupAssignment = await GetGroupAssignment();

        IEnumerable<Guid> mentorIds = groupAssignment.Assignment.SubjectCourse.Mentors
            .Select(x => x.UserId);

        var mentorId = await Context.Mentors
            .Where(x => mentorIds.Contains(x.UserId) == false)
            .Select(x => new { x.UserId, x.SubjectCourseId })
            .FirstAsync();

        var query = MentorQuery.Build(x => x.WithUserId(mentorId.UserId).WithSubjectCourseId(mentorId.SubjectCourseId));

        Mentor mentor = await PersistenceContext.Mentors
            .QueryAsync(query, default)
            .FirstAsync();

        var currentUser = new MentorUser(mentor.UserId);

        // Act
        UpdateGroupAssignmentDeadlines.Response response = await HandleByCurrentUser(currentUser);

        // Assert
        response.Should().BeOfType<UpdateGroupAssignmentDeadlines.Response.Unauthorized>();
    }

    [Fact]
    public async Task Handle_ByModerator_ShouldUpdateDeadline()
    {
        // Arrange
        var currentUser = new ModeratorUser(Guid.NewGuid());

        // Act
        UpdateGroupAssignmentDeadlines.Response response = await HandleByCurrentUser(currentUser);

        // Assert
        response.Should()
            .BeOfType<UpdateGroupAssignmentDeadlines.Response.Success>()
            .Which.GroupAssignments.Single()
            .Deadline.Should()
            .Be(_newDeadline);
    }

    [Fact]
    public async Task Handle_ByAdmin_ShouldUpdateDeadline()
    {
        // Arrange
        var currentUser = new AdminUser(Guid.NewGuid());

        // Act
        UpdateGroupAssignmentDeadlines.Response response = await HandleByCurrentUser(currentUser);

        // Assert
        response.Should()
            .BeOfType<UpdateGroupAssignmentDeadlines.Response.Success>()
            .Which.GroupAssignments.Single()
            .Deadline.Should()
            .Be(_newDeadline);
    }

    private Task<GroupAssignmentModel> GetGroupAssignment()
    {
        return Context.GroupAssignments.FirstAsync();
    }

    private async Task<UpdateGroupAssignmentDeadlines.Response> HandleByCurrentUser(ICurrentUser currentUser)
    {
        GroupAssignmentModel groupAssignment = await GetGroupAssignment();
        var handler = new UpdateGroupAssignmentDeadlinesHandler(PersistenceContext, currentUser, _publisher);

        var command = new UpdateGroupAssignmentDeadlines.Command(
            groupAssignment.AssignmentId,
            _newDeadline,
            new[] { groupAssignment.StudentGroupId });

        return await handler.Handle(command, default);
    }

    // Custom fixture for custom seeding config
    public class UpdateGroupAssignmentDatabaseFixture : CoreDatabaseFixture
    {
        protected override void ConfigureSeeding(EntityGenerationOptions options)
        {
            options.ConfigureEntityGenerator<SubjectCourseModel>(x => x.Count = 2);
        }
    }
}