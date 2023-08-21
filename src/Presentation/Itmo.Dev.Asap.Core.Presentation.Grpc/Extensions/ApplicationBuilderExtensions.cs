using Itmo.Dev.Asap.Core.Presentation.Grpc.Controllers;
using Microsoft.AspNetCore.Builder;

namespace Itmo.Dev.Asap.Core.Presentation.Grpc.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGrpcPresentation(this IApplicationBuilder builder)
    {
        builder.UseEndpoints(x =>
        {
            x.MapGrpcService<AssignmentsController>();
            x.MapGrpcService<StudentController>();
            x.MapGrpcService<StudentGroupController>();
            x.MapGrpcService<SubjectController>();
            x.MapGrpcService<SubjectCourseController>();
            x.MapGrpcService<SubjectCourseGroupController>();
            x.MapGrpcService<UserController>();
            x.MapGrpcService<PermissionsController>();
            x.MapGrpcService<QueueController>();

            x.MapGrpcReflectionService();
        });

        return builder;
    }
}