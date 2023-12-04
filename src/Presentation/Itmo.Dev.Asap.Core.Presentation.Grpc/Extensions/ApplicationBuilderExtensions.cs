using Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;
using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGrpcPresentation(this IApplicationBuilder builder)
    {
        builder.UseEndpoints(x =>
        {
            x.MapGrpcService<AssignmentsController>();
            x.MapGrpcService<PermissionsController>();
            x.MapGrpcService<QueueController>();
            x.MapGrpcService<StudentController>();
            x.MapGrpcService<StudentGroupController>();
            x.MapGrpcService<SubjectController>();
            x.MapGrpcService<SubjectCourseController>();
            x.MapGrpcService<SubjectCourseGroupController>();
            x.MapGrpcService<SubmissionController>();
            x.MapGrpcService<SubmissionWorkflowController>();
            x.MapGrpcService<UserController>();

            x.MapMetrics();

            x.MapGrpcReflectionService();
        });

        return builder;
    }
}