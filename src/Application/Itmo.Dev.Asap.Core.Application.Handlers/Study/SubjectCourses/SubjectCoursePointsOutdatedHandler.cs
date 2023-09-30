using Itmo.Dev.Asap.Core.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications;
using Itmo.Dev.Asap.Core.Application.Dto.SubjectCourses;
using MediatR;
using Microsoft.Extensions.Logging;
using static Itmo.Dev.Asap.Core.Application.Contracts.Study.SubjectCourses.Notifications.SubjectCoursePointsOutdated;

namespace Itmo.Dev.Asap.Core.Application.Handlers.Study.SubjectCourses;

internal class SubjectCoursePointsOutdatedHandler : INotificationHandler<Notification>
{
    private readonly ILogger<SubjectCoursePointsOutdatedHandler> _logger;
    private readonly ISubjectCourseService _service;
    private readonly IPublisher _publisher;

    public SubjectCoursePointsOutdatedHandler(
        ILogger<SubjectCoursePointsOutdatedHandler> logger,
        ISubjectCourseService service,
        IPublisher publisher)
    {
        _logger = logger;
        _service = service;
        _publisher = publisher;
    }

    public async Task Handle(Notification notification, CancellationToken cancellationToken)
    {
        try
        {
            await ExecuteAsync(notification, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(
                e,
                "Error updating course points for subject course {SubjectCourseId}",
                notification.SubjectCourseId);
        }
    }

    private async Task ExecuteAsync(
        Notification notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Start updating for points sheet of course {SubjectCourseId}",
            notification.SubjectCourseId);

        _logger.LogInformation("Started collecting all course {CourseId} points", notification.SubjectCourseId);

        SubjectCoursePointsDto points = await _service.CalculatePointsAsync(
            notification.SubjectCourseId, cancellationToken);

        _logger.LogInformation("Finished collecting all course {CourseId} points", notification.SubjectCourseId);

        var updatedNotification = new SubjectCoursePointsUpdated.Notification(notification.SubjectCourseId, points);
        await _publisher.Publish(updatedNotification, default);
    }
}