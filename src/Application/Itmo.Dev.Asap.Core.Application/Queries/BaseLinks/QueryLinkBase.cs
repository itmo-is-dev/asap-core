using FluentChaining;
using Itmo.Dev.Asap.Core.Application.Dto.Querying;
using Itmo.Dev.Asap.Core.Application.Queries.Requests;

namespace Itmo.Dev.Asap.Core.Application.Queries.BaseLinks;

public abstract class QueryLinkBase<TBuilder, TParameter> : ILink<EntityQueryRequest<TBuilder, TParameter>, TBuilder>
{
    public TBuilder Process(
        EntityQueryRequest<TBuilder, TParameter> request,
        SynchronousContext context,
        LinkDelegate<EntityQueryRequest<TBuilder, TParameter>, SynchronousContext, TBuilder> next)
    {
        TBuilder? result = TryApply(request.QueryBuilder, request.Parameter);
        return result ?? next.Invoke(request, context);
    }

    protected abstract TBuilder? TryApply(TBuilder queryBuilder, QueryParameter<TParameter> parameter);
}