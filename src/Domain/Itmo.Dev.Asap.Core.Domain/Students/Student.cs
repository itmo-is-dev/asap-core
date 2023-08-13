using Itmo.Dev.Asap.Core.Common.Exceptions;
using Itmo.Dev.Asap.Core.Domain.Groups;
using Itmo.Dev.Asap.Core.Domain.Students.Events;
using Itmo.Dev.Asap.Core.Domain.Users;
using RichEntity.Annotations;
using System.Text;

namespace Itmo.Dev.Asap.Core.Domain.Students;

public partial class Student : IEntity
{
    public Student(User user, StudentGroupInfo? group)
        : this(user.Id)
    {
        User = user;
        Group = group;
    }

    [KeyProperty]
    public User User { get; }

    public StudentGroupInfo? Group { get; private set; }

    public void DismissFromStudyGroup(Groups.StudentGroup group)
    {
        if (Group is null)
            throw new DomainInvalidOperationException("Student is not in any group");

        if (group.Id.Equals(Group.Id) is false)
            throw new DomainInvalidOperationException("Trying to dismiss student from invalid group");

        group.RemoveStudent(this);
        Group = null;
    }

    public (Student Student, IStudentEvent Event) TransferToAnotherGroup(Groups.StudentGroup? from, Groups.StudentGroup to)
    {
        if (Nullable.Equals(Group?.Id, from?.Id) is false)
            throw new DomainInvalidOperationException("Trying to transfer student from invalid group");

        Student student = to.AddStudent(User);
        from?.RemoveStudent(this);

        Group = to.Info;

        var evt = new StudentTransferredEvent(this);

        return (student, evt);
    }

    public override string ToString()
    {
        var builder = new StringBuilder($"{User.FirstName} {User.LastName}");

        if (Group is not null)
            builder.Append($" from Group = {Group.Name}({Group.Id}), UserId = ({UserId})");

        return builder.ToString();
    }
}