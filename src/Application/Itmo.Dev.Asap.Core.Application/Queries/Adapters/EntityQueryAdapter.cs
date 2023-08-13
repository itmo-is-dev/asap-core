using FluentChaining;
using Itmo.Dev.Asap.Core.Application.Dto.Querying;
using Itmo.Dev.Asap.Core.Application.Queries.Requests;

namespace Itmo.Dev.Asap.Core.Application.Queries.Adapters;

public class EntityQueryAdapter<TBuilder, TParameter> : IEntityQuery<TBuilder, TParameter>
{
    private readonly IChain<EntityQueryRequest<TBuilder, TParameter>, TBuilder> _chain;

    public EntityQueryAdapter(IChain<EntityQueryRequest<TBuilder, TParameter>, TBuilder> chain)
    {
        _chain = chain;
    }

    public TBuilder Apply(TBuilder queryBuilder, QueryConfiguration<TParameter> configuration)
    {
        foreach (QueryParameter<TParameter> parameter in configuration.Parameters)
        {
            var request = new EntityQueryRequest<TBuilder, TParameter>(queryBuilder, parameter);
            queryBuilder = _chain.Process(request);
        }

        return queryBuilder;
    }
}