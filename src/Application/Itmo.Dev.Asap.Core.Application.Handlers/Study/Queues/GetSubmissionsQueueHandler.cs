using Itmo.Dev.Asap.Core.Application.Abstractions.Queue;
using Itmo.Dev.Asap.Core.Application.Dto.Tables;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Queries.GetSubmissionsQueue;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.Queues;

internal class GetSubmissionsQueueHandler : IRequestHandler<Query, Response>
{
    private readonly IMemoryCache _cache;
    private readonly IQueueService _queueService;

    public GetSubmissionsQueueHandler(IQueueService queueService, IMemoryCache cache)
    {
        _queueService = queueService;
        _cache = cache;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        string cacheKey = string.Concat(request.SubjectCourseId, request.StudentGroupId);

        if (_cache.TryGetValue(cacheKey, out SubmissionsQueueDto submissionsQueue))
            return new Response(submissionsQueue);

        submissionsQueue = await _queueService.GetSubmissionsQueueAsync(
            request.SubjectCourseId,
            request.StudentGroupId,
            cancellationToken);

        _cache.Set(cacheKey, submissionsQueue);

        return new Response(submissionsQueue);
    }
}