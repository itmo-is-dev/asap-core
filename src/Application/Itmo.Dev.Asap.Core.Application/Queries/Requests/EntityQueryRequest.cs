using Itmo.Dev.Asap.Core.Application.Dto.Querying;

namespace Itmo.Dev.Asap.Core.Application.Queries.Requests;

public record struct EntityQueryRequest<TBuilder, TParameter>(
    TBuilder QueryBuilder,
    QueryParameter<TParameter> Parameter);