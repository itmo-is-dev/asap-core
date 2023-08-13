namespace Itmo.Dev.Asap.Core.Domain.Students;

public interface IStudentEvent
{
    ValueTask AcceptAsync(IStudentEventVisitor visitor, CancellationToken cancellationToken);
}