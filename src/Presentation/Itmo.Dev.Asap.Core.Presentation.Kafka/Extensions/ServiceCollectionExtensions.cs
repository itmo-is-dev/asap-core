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
        const string assignmentCreatedKey = "Presentation:Kafka:Producers:AssignmentCreated";
        const string queueUpdatedKey = "Presentation:Kafka:Producers:QueueUpdated";
        const string subjectCourseCreatedKey = "Presentation:Kafka:Producers:SubjectCourseCreated";
        const string subjectCoursePointsUpdatedKey = "Presentation:Kafka:Producers:SubjectCoursePointsUpdated";

        string host = configuration.GetSection("Presentation:Kafka:Host").Get<string>() ?? string.Empty;

        collection.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<IAssemblyMarker>());

        collection.AddKafkaProducer<AssignmentCreatedKey, AssignmentCreatedValue>(selector => selector
            .SerializeKeyWithProto()
            .SerializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "AssignmentCreated",
                configuration.GetSection(assignmentCreatedKey),
                c => c.WithHost(host)));

        collection.AddKafkaProducer<QueueUpdatedKey, QueueUpdatedValue>(selector => selector
            .SerializeKeyWithProto()
            .SerializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "QueueUpdated",
                configuration.GetSection(queueUpdatedKey),
                c => c.WithHost(host)));

        collection.AddKafkaProducer<SubjectCourseCreatedKey, SubjectCourseCreatedValue>(selector => selector
            .SerializeKeyWithProto()
            .SerializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "SubjectCourseCreated",
                configuration.GetSection(subjectCourseCreatedKey),
                c => c.WithHost(host)));

        collection.AddKafkaProducer<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>(selector => selector
            .SerializeKeyWithProto()
            .SerializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "SubjectCoursePointsUpdated",
                configuration.GetSection(subjectCoursePointsUpdatedKey),
                c => c.WithHost(host)));

        return collection;
    }
}