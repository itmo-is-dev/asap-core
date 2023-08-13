using FluentChaining;
using Itmo.Dev.Asap.Core.Application.Dto.Querying;
using Itmo.Dev.Asap.Core.Application.Queries.Requests;

namespace Itmo.Dev.Asap.Core.Application.Queries.BaseLinks;

public abstract class FilterLinkBase<TEntity, TParameter> :
    ILink<EntityFilterRequest<TEntity, TParameter>, IEnumerable<TEntity>>
{
    public IEnumerable<TEntity> Process(
        EntityFilterRequest<TEntity, TParameter> request,
        SynchronousContext context,
        LinkDelegate<EntityFilterRequest<TEntity, TParameter>, SynchronousContext, IEnumerable<TEntity>> next)
    {
        IEnumerable<TEntity>? result = TryApply(request.Data, request.Parameter);
        return result ?? next.Invoke(request, context);
    }

    protected abstract IEnumerable<TEntity>? TryApply(
        IEnumerable<TEntity> data,
        QueryParameter<TParameter> parameter);
}