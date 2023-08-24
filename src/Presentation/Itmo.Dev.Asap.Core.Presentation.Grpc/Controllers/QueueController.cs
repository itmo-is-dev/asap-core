using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Itmo.Dev.Asap.Core.Application.Contracts.Study.Queues.Notifications;
using Itmo.Dev.Asap.Core.Models;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Mapping;
using Itmo.Dev.Asap.Core.Queue;
using System.Runtime.CompilerServices;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;

public class QueueController : QueueService.QueueServiceBase
{
    private readonly IObservable<QueueUpdated.Notification> _observable;

    public QueueController(IObservable<QueueUpdated.Notification> observable)
    {
        _observable = observable;
    }

    public override async Task QueueUpdates(
        Empty request,
        IServerStreamWriter<QueueUpdatedResponse> responseStream,
        ServerCallContext context)
    {
        ConfiguredCancelableAsyncEnumerable<QueueUpdated.Notification> enumerable = _observable
            .ToAsyncEnumerable()
            .WithCancellation(context.CancellationToken);

        await foreach (QueueUpdated.Notification notification in enumerable)
        {
            IEnumerable<Student> students = notification.SubmissionsQueue.Students
                .Select(x => x.MapToProtoStudent());

            IEnumerable<QueueUpdatedResponse.Types.Submission> submissions = notification.SubmissionsQueue.Submissions
                .Select(x => x.MapToProtoQueueSubmission());

            var queue = new QueueUpdatedResponse.Types.SubmissionQueue
            {
                Submissions = { submissions },
            };

            foreach (Student student in students)
            {
                queue.Students.Add(student.User.Id, student);
            }

            var model = new QueueUpdatedResponse
            {
                SubjectCourseId = notification.SubjectCourseId.ToString(),
                StudentGroupId = notification.StudentGroupId.ToString(),
                StudentGroupName = notification.SubmissionsQueue.GroupName,
                SubmissionsQueue = queue,
            };

            await responseStream.WriteAsync(model);
        }
    }
}