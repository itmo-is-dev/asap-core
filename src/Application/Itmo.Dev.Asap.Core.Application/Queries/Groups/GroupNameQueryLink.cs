using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Application.Dto.Querying;
using Itmo.Dev.Asap.Core.Application.Queries.BaseLinks;

namespace Itmo.Dev.Asap.Core.Application.Queries.Groups;

public class GroupNameQueryLink : QueryLinkBase<StudentGroupQuery.Builder, GroupQueryParameter>
{
    protected override StudentGroupQuery.Builder? TryApply(
        StudentGroupQuery.Builder queryBuilder,
        QueryParameter<GroupQueryParameter> parameter)
    {
        return parameter.Type is not GroupQueryParameter.Name
            ? null
            : queryBuilder.WithNamePattern(parameter.Pattern);
    }
}