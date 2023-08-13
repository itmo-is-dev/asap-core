using Itmo.Dev.Asap.Core.Application.Dto.Querying;

namespace Itmo.Dev.Asap.Core.Application.Queries.Requests;

public record struct EntityFilterRequest<TEntity, TParameter>(
    IEnumerable<TEntity> Data,
    QueryParameter<TParameter> Parameter);