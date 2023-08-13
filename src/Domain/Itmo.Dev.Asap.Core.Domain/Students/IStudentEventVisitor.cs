using Itmo.Dev.Asap.Core.Domain.Students.Events;

namespace Itmo.Dev.Asap.Core.Domain.Students;

public interface IStudentEventVisitor
{
    ValueTask VisitAsync(StudentTransferredEvent evt, CancellationToken cancellationToken);
}