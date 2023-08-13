using Itmo.Dev.Asap.Core.DataAccess.Contexts;
using Itmo.Dev.Asap.Core.DataAccess.Mapping;
using Itmo.Dev.Asap.Core.DataAccess.Models.Users;
using Itmo.Dev.Asap.Core.DataAccess.Tools;
using Itmo.Dev.Asap.Core.Domain.Students;
using Itmo.Dev.Asap.Core.Domain.Students.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Itmo.Dev.Asap.Core.DataAccess.Repositories.Students;

public class StudentRepositoryEventVisitor : IStudentEventVisitor
{
    private readonly DatabaseContext _context;

    public StudentRepositoryEventVisitor(DatabaseContext context)
    {
        _context = context;
    }

    public ValueTask VisitAsync(StudentTransferredEvent evt, CancellationToken cancellationToken)
    {
        EntityEntry<StudentModel> entry = RepositoryTools.GetEntry(
            _context,
            x => x.UserId.Equals(evt.Student.UserId),
            () => StudentMapper.MapFrom(evt.Student));

        entry.Entity.StudentGroupId = evt.Student.Group?.Id;
        entry.State = EntityState.Modified;

        return ValueTask.CompletedTask;
    }
}