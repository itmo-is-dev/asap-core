using Itmo.Dev.Asap.Core.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Core.Domain.Students;

namespace Itmo.Dev.Asap.Core.Application.DataAccess.Repositories;

public interface IStudentRepository
{
    IAsyncEnumerable<Student> QueryAsync(StudentQuery query, CancellationToken cancellationToken);

    ValueTask ApplyAsync(IStudentEvent evt, CancellationToken cancellationToken);

    void Add(Student student);

    void Update(Student student);
}