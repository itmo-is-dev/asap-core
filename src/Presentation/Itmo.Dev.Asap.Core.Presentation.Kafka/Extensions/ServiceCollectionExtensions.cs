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

        string host = configuration.GetSection("Presentation:Kafka:Host").Get<string>() ?? string.Empty;

        collection.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<IAssemblyMarker>());

        collection.AddKafkaProducer<AssignmentCreatedKey, AssignmentCreatedValue>(selector => selector
            .SerializeKeyWithProto()
            .SerializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "AssignmentCreated",
                configuration.GetSection($"{configurationKey}:AssignmentCreated"),
                c => c.WithHost(host)));

        collection.AddKafkaProducer<QueueUpdatedKey, QueueUpdatedValue>(selector => selector
            .SerializeKeyWithProto()
            .SerializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "QueueUpdated",
                configuration.GetSection($"{configurationKey}:QueueUpdated"),
                c => c.WithHost(host)));

        collection.AddKafkaProducer<SubjectCourseCreatedKey, SubjectCourseCreatedValue>(selector => selector
            .SerializeKeyWithProto()
            .SerializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "SubjectCourseCreated",
                configuration.GetSection($"{configurationKey}:SubjectCourseCreated"),
                c => c.WithHost(host)));

        collection.AddKafkaProducer<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>(selector => selector
            .SerializeKeyWithProto()
            .SerializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "SubjectCoursePointsUpdated",
                configuration.GetSection($"{configurationKey}:SubjectCoursePointsUpdated"),
                c => c.WithHost(host)));

        collection.AddKafkaProducer<StudentPointsUpdatedKey, StudentPointsUpdatedValue>(selector => selector
            .SerializeKeyWithProto()
            .SerializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "StudentPointsUpdated",
                configuration.GetSection($"{configurationKey}:StudentPointsUpdated"),
                c => c.WithHost(host)));

        return collection;
    }
}