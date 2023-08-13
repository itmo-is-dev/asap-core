using Itmo.Dev.Asap.Core.Application.Dto.Querying;

namespace Itmo.Dev.Asap.Core.Application.Queries;

public interface IEntityFilter<TEntity, TParameter>
{
    IEnumerable<TEntity> Apply(
        IEnumerable<TEntity> data,
        QueryConfiguration<TParameter> configuration);
}