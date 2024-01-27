using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Core.Presentation.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaPresentation(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        const string configurationKey = "Presentation:Kafka:Producers";

        collection.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<IAssemblyMarker>());

        collection.AddKafka(builder => builder
            .ConfigureOptions(b => b.BindConfiguration("Presentation:Kafka"))
            .AddProducer<AssignmentCreatedKey, AssignmentCreatedValue>(selector => selector
                .SerializeKeyWithProto()
                .SerializeValueWithProto()
                .UseNamedOptionsConfiguration(
                    "AssignmentCreated",
                    configuration.GetSection($"{configurationKey}:AssignmentCreated")))
            .AddProducer<QueueUpdatedKey, QueueUpdatedValue>(selector => selector
                .SerializeKeyWithProto()
                .SerializeValueWithProto()
                .UseNamedOptionsConfiguration(
                    "QueueUpdated",
                    configuration.GetSection($"{configurationKey}:QueueUpdated")))
            .AddProducer<SubjectCourseCreatedKey, SubjectCourseCreatedValue>(selector => selector
                .SerializeKeyWithProto()
                .SerializeValueWithProto()
                .UseNamedOptionsConfiguration(
                    "SubjectCourseCreated",
                    configuration.GetSection($"{configurationKey}:SubjectCourseCreated")))
            .AddProducer<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>(selector => selector
                .SerializeKeyWithProto()
                .SerializeValueWithProto()
                .UseNamedOptionsConfiguration(
                    "SubjectCoursePointsUpdated",
                    configuration.GetSection($"{configurationKey}:SubjectCoursePointsUpdated")))
            .AddProducer<StudentPointsUpdatedKey, StudentPointsUpdatedValue>(selector => selector
                .SerializeKeyWithProto()
                .SerializeValueWithProto()
                .UseNamedOptionsConfiguration(
                    "StudentPointsUpdated",
                    configuration.GetSection($"{configurationKey}:StudentPointsUpdated"))));

        return collection;
    }
}