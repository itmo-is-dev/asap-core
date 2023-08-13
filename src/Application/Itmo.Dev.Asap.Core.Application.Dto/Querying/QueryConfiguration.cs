namespace Itmo.Dev.Asap.Core.Application.Dto.Querying;

public record QueryConfiguration<T>(IReadOnlyCollection<QueryParameter<T>> Parameters);