using Itmo.Dev.Asap.Core.Application.DataAccess;
using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Submissions;
using Itmo.Dev.Asap.Core.Domain.Models;
using MediatR;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Submissions.Queries.QueryFirstCompletedSubmissions;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Submissions;

internal class QueryFirstCompletedSubmissionsHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public QueryFirstCompletedSubmissionsHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = FirstSubmissionQuery.Build(builder => builder
            .WithSubjectCourseId(request.SubjectCourseId)
            .WithState(SubmissionStateKind.Completed)
            .WithPageToken(MapToPageTokenModel(request.PageToken))
            .WithPageSize(request.PageSize));

        FirstSubmissionDto[] submissions = await _context.Submissions
            .QueryFirstSubmissionsAsync(query, cancellationToken)
            .Select(x => new FirstSubmissionDto(x.Id, x.Student.UserId, x.GroupAssignment.Id.AssignmentId))
            .ToArrayAsync(cancellationToken);

        PageToken? pageToken = submissions.Length.Equals(request.PageSize)
            ? MapToPageToken(submissions[^1])
            : null;

        return new Response(submissions, pageToken);
    }

    private static FirstSubmissionQuery.PageTokenModel? MapToPageTokenModel(PageToken? pageToken)
    {
        if (pageToken is null)
            return null;

        return new FirstSubmissionQuery.PageTokenModel(pageToken.UserId, pageToken.AssignmentId);
    }

    private static PageToken MapToPageToken(FirstSubmissionDto submission)
    {
        return new PageToken(submission.UserId, submission.AssignmentId);
    }
}