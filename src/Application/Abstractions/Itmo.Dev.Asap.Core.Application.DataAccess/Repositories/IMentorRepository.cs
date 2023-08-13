using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Users;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface IMentorRepository
{
    IAsyncEnumerable<Mentor> QueryAsync(MentorQuery query, CancellationToken cancellationToken);
}