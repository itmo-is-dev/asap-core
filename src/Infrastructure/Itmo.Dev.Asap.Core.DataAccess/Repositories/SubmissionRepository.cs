using Itmo.Dev.Asap.Core.Application.DataAccess.Models;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Mapping;
using Itmo.Dev.Asap.Core.DataAccess.Models;
using Itmo.Dev.Asap.Core.DataAccess.Tools;
using Itmo.Dev.Asap.Core.Domain.Submissions;
using Itmo.Dev.Platform.Postgres.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;
using System.Runtime.CompilerServices;

namespace Itmo.Dev.Asap.Core.DataAccess.Repositories;

public class SubmissionRepository : ISubmissionRepository
{
    private readonly DatabaseContext _context;

    public SubmissionRepository(DatabaseContext context)
    {
        _context = context;
    }

    public IAsyncEnumerable<Submission> QueryAsync(SubmissionQuery query, CancellationToken cancellationToken)
    {
        IQueryable<SubmissionModel> queryable = ApplyQuery(_context.Submissions, query);

        queryable = queryable
            .Include(x => x.Student)
            .ThenInclude(x => x.User)
            .ThenInclude(x => x.Associations);

        var finalQueryable = queryable.Select(submission => new
        {
            submission,
            groupAssignment = submission.GroupAssignment,
            groupName = submission.GroupAssignment.StudentGroup.Name,
            assignmentTitle = submission.GroupAssignment.Assignment.Title,
            assignmentShortName = submission.GroupAssignment.Assignment.ShortName,
        });

        return finalQueryable
            .AsAsyncEnumerable()
            .Select(x => SubmissionMapper.MapTo(
                x.submission,
                GroupAssignmentMapper.MapTo(x.groupAssignment, x.groupName, x.assignmentTitle, x.assignmentShortName),
                StudentMapper.MapTo(x.submission.Student)));
    }

    public async IAsyncEnumerable<FirstSubmissionModel> QueryFirstSubmissionsAsync(
        FirstSubmissionQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
        select nth_value(s."Id", 1) over (partition by (s."StudentId", s."AssignmentId") order by s."SubmissionDate") as "Id", 
               nth_value(s."StudentId", 1) over (partition by (s."StudentId", s."AssignmentId") order by s."SubmissionDate") as "StudentId", 
               nth_value(s."AssignmentId", 1) over (partition by (s."StudentId", s."AssignmentId") order by s."SubmissionDate") as "AssignmentId" 
        from "Submissions" s
        join "Assignments" a on a."Id" = s."AssignmentId"
        where
            a."SubjectCourseId" = :subject_course_id
            and s."State" = any (:states)
            and (:skip_user_id_filter or s."StudentId" >= :user_id) 
            and (:skip_assignment_id_filter or s."AssignmentId" > :assignment_id)
        order by (s."StudentId", s."AssignmentId")
        limit :limit
        """;

        var connection = (NpgsqlConnection)_context.Database.GetDbConnection();

        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("subject_course_id", query.SubjectCourseId)
            .AddParameter("states", query.States.Select(x => (int)x).ToArray())
            .AddParameter("skip_user_id_filter", query.PageToken is null)
            .AddParameter("skip_assignment_id_filter", query.PageToken is null)
            .AddParameter("user_id", query.PageToken?.UserId ?? Guid.Empty)
            .AddParameter("assignment_id", query.PageToken?.AssignmentId ?? Guid.Empty)
            .AddParameter("limit", query.PageSize);

        await _context.Database.OpenConnectionAsync(cancellationToken);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        int id = reader.GetOrdinal("Id");
        int studentId = reader.GetOrdinal("StudentId");
        int assignmentId = reader.GetOrdinal("AssignmentId");

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new FirstSubmissionModel(
                Id: reader.GetGuid(id),
                StudentId: reader.GetGuid(studentId),
                AssignmentId: reader.GetGuid(assignmentId));
        }
    }

    public Task<int> CountAsync(SubmissionQuery query, CancellationToken cancellationToken)
    {
        IQueryable<SubmissionModel> queryable = ApplyQuery(_context.Submissions, query);
        return queryable.CountAsync(cancellationToken);
    }

    public void Add(Submission submission)
    {
        SubmissionModel model = SubmissionMapper.MapFrom(submission);
        _context.Submissions.Add(model);
    }

    public void Update(Submission submission)
    {
        EntityEntry<SubmissionModel> entry = RepositoryTools.GetEntry(
            _context,
            x => x.Id.Equals(submission.Id),
            () => SubmissionMapper.MapFrom(submission));

        SubmissionModel model = entry.Entity;
        model.Rating = submission.Rating?.Value;
        model.ExtraPoints = submission.ExtraPoints?.Value;
        model.SubmissionDate = submission.SubmissionDate;
        model.State = submission.State.Kind;

        entry.State = EntityState.Modified;
    }

    private IQueryable<SubmissionModel> ApplyQuery(IQueryable<SubmissionModel> queryable, SubmissionQuery query)
    {
        if (query.Ids is not [])
        {
            queryable = queryable.Where(x => query.Ids.Contains(x.Id));
        }

        if (query.Codes is not [])
        {
            queryable = queryable.Where(x => query.Codes.Contains(x.Code));
        }

        if (query.UserIds is not [])
        {
            queryable = queryable.Where(x => query.UserIds.Contains(x.StudentId));
        }

        if (query.SubjectCourseIds is not [])
        {
            queryable = queryable
                .Where(x => query.SubjectCourseIds.Contains(x.GroupAssignment.Assignment.SubjectCourseId));
        }

        if (query.AssignmentIds is not [])
        {
            queryable = queryable.Where(x => query.AssignmentIds.Contains(x.AssignmentId));
        }

        if (query.StudentGroupIds is not [])
        {
            queryable = queryable.Where(x => query.StudentGroupIds.Contains(x.StudentGroupId));
        }

        if (query.SubmissionStates is not [])
        {
            queryable = queryable.Where(x => query.SubmissionStates.Contains(x.State));
        }

        if (query.SubjectCourseWorkflows is not [])
        {
            queryable = queryable
                .Where(x => query.SubjectCourseWorkflows
                    .Contains(x.GroupAssignment.Assignment.SubjectCourse.WorkflowType!.Value));
        }

        if (query.OrderByCode is not null)
        {
            queryable = query.OrderByCode.Value switch
            {
                OrderDirection.Ascending => queryable.OrderBy(x => x.Code),
                OrderDirection.Descending => queryable.OrderByDescending(x => x.Code),
                _ => throw new ArgumentOutOfRangeException(nameof(query)),
            };
        }

        if (query.Limit is not null)
        {
            queryable = queryable.Take(query.Limit.Value);
        }

        return queryable;
    }
}