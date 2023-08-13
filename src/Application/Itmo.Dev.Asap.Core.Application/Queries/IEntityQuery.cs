using Itmo.Dev.Asap.Core.Application.Dto.Querying;

namespace Itmo.Dev.Asap.Core.Application.Queries;

public interface IEntityQuery<TBuilder, TParameter>
{
    TBuilder Apply(TBuilder queryBuilder, QueryConfiguration<TParameter> configuration);
}